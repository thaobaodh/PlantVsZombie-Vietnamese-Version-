using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SkeletonDemonZombie : MonoBehaviour
{
    [SerializeField] private int formName = 0; // 0 Old man, 1 Little Girl, 2 Lady

    [SerializeField] private bool isAttack = false;

    [SerializeField] private bool isSpecialAttack = false; 

    [SerializeField] private int damage = 5;
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth;
    [SerializeField] private int skill;
    [SerializeField] private int numOfSkills = 4;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float cooldownTime = 10;
    [SerializeField] private float range = 0.5f;

    [SerializeField] private string lane;

    [SerializeField] private Transform[] positionList;

    [SerializeField] private GameObject skeletonZombie;

    [SerializeField] private GameObject[] toolObjects;

    [SerializeField] private LayerMask plantMask;

    [SerializeField] private GameManager gms;

    [SerializeField] private Plant targetPlant;
    // Start is called before the first frame update
    void Start()
    {
        this.LoadSystemParam();
        this.LoadZombieParam();
        this.ChangeForm();
    }

    private void LoadSystemParam()
    {
        isAttack = true;
        isSpecialAttack = true;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void LoadZombieParam()
    {
        switch (formName)
        {
            case 0:
                {
                    speed = 0.25f;
                    cooldownTime = 1f;
                    health = 100;
                    maxHealth = health;
                    break;
                }
            case 1:
                {
                    speed = 0.125f;
                    cooldownTime = 20f;
                    health = 150;
                    maxHealth = health;
                    break;
                }
            case 2:
                {
                    speed = 0.25f;
                    cooldownTime = 10f;
                    health = 200;
                    maxHealth = health;
                    break;
                }
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // this.Hit();
        this.Movement();
    }

    public void Hit()
    {
        switch (formName)
        {
            case 0:
                {
                    if(health >= 4 * (maxHealth / 5))
                    {
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(2).gameObject.SetActive(false);
                    }
                    else if (health >= 3 * (maxHealth / 5) && health < 4 * (maxHealth / 5))
                    {
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(true);
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(2).gameObject.SetActive(false);
                    }
                    else if (health >= 2 * (maxHealth / 5) && health < 3 * (maxHealth / 5))
                    {
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(2).gameObject.SetActive(true);
                    }
                    else
                    {
                        speed = 1f; //Increase speed movement
                        cooldownTime = 0.5f; //Increase eating speed 

                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(false);
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(false);
                        gameObject.transform.GetChild(0).transform.GetChild(4).transform.GetChild(2).gameObject.SetActive(false);
                    }
                    break;
                }
            case 1:
                {
                    break;
                }
            case 2:
                {
                    break;
                }
            default:
                break;
        }

        if (health <= 0)
        {
            switch (formName)
            {
                case 0:
                    {
                        Debug.Log("Change to Little Girl form");
                        formName = 1;
                        lane = (UnityEngine.Random.Range(0, positionList.Length) + 1).ToString();
                        transform.position = positionList[int.Parse(lane) - 1].position;
                        break;
                    }
                case 1:
                    {
                        Debug.Log("Change to Lady form");
                        formName = 2;
                        lane = (UnityEngine.Random.Range(0, positionList.Length) + 1).ToString();
                        transform.position = positionList[int.Parse(lane) - 1].position;
                        break;
                    }
                case 2:
                    {
                        Debug.Log("Skeleton Demon Zombie is death!");
                        Destroy(gameObject, 1f);
                        break;
                    }
                default:
                    break;
            }
            this.LoadZombieParam();
            this.ChangeForm();
        }
    }

    private void ChangeForm()
    {
        switch (formName)
        {
            case 0:
                {
                    Debug.Log("Old man active: " + gameObject.transform.GetChild(0).name);
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                }
            case 1:
                {
                    Debug.Log("Little Girl active: " + gameObject.transform.GetChild(1).name);
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                }
            case 2:
                {
                    Debug.Log("Lady active: " + gameObject.transform.GetChild(2).name);
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gameObject.transform.GetChild(2).gameObject.SetActive(true);
                    break;
                }
            default:
                break;
        }
    }

    private void Movement()
    {
        if (!gms.IsStart) return;
        if (!gms.StartSpawningZombie) return;

        if (!isAttack) return;

        this.checkNormalAttackCondition();

        switch (formName)
        {
            case 0:
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
                    break;
                }
            case 1:
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
                    break;
                }
            case 2:
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
                    if(transform.position.x >= 3.5f && transform.position.x <= 5f)
                    {
                        speed = 0;
                        isAttack = false;
                        Invoke(nameof(MoveToSpawnPoint), 1f);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

        if (!isSpecialAttack) return;

        this.checkSpecialAttackCondition();

    }

    private void MoveToSpawnPoint()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(8.5f, 9.5f), positionList[UnityEngine.Random.Range(0, 4)].transform.position.y, 0);
        Invoke(nameof(StartToMove), 1f);
    }

    private void StartToMove()
    {
        speed = 0.25f;
        isAttack = true;
    }
    private void checkNormalAttackCondition()
    {
        RaycastHit2D plantHit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);

        if (plantHit.collider)
        {
            if (plantHit.collider.GetComponent<Plant>() != null)
            {
                targetPlant = plantHit.collider.GetComponent<Plant>();
                this.Attack(targetPlant);
            }
        }
        else
        {

        }
    }
    private void checkSpecialAttackCondition()
    {
        switch (formName)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    if (transform.position.x <= 7.5f)
                    {
                        this.summonZombie();
                    }
                    break;
                }
            case 2:
                {
                    // Skeleton Demon Skill:
                    // 1: Hút máu từ cây (trừ máu cây + thêm máu cho bạch cốt tinh)
                    // 2: Thả khói vào cây làm cho cây bị độc từ từ cho tới chết
                    // 3: Ném xương ra rơi xuống đâu thì triệu hồi zombie xương ở đó
                    // 4: Biến cây thành zombie xương

                    if (transform.position.x <= 6.5f)
                    {
                        switch (skill)
                        {
                            case 0:
                                {
                                    Debug.Log("Skill 0");
                                    this.Vampire();
                                    break;
                                }
                            case 1:
                                {
                                    Debug.Log("Skill 1");
                                    this.ReleaseSmoke();
                                    break;
                                }
                            case 2:
                                {
                                    Debug.Log("Skill 2");
                                    this.ThrowABoneToSummonSkeletonZombie();
                                    break;
                                }
                            case 3:
                                {
                                    Debug.Log("Skill 3");
                                    this.ChangePlantToSkeletonZombie();
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void Vampire()
    {
        int count = 0;
        int RandomHealth = UnityEngine.Random.Range(0, 3);
        isSpecialAttack = false;

        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, 8f);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Plant>(out Plant plant))
            {
                if (count <= RandomHealth)
                {
                    Debug.Log(count + " / " + RandomHealth + " - " + plant.tile.name);
                    count++;
                    GameObject health = Instantiate(toolObjects[0], plant.transform.position, Quaternion.identity);
                    health.GetComponent<Flame>().targetPoint = transform.position;
                    plant.Hit(10, 1);
                }
                else
                {
                    count = 0;
                    return;
                }
            }

        }

        Invoke(nameof(ResetSpecialAttackCooldown), cooldownTime);
    }
    private void ReleaseSmoke()
    {
        isSpecialAttack = false;
        Instantiate(toolObjects[1], transform.position, Quaternion.identity);
        Invoke(nameof(ResetSpecialAttackCooldown), cooldownTime);
    }
    private void ThrowABoneToSummonSkeletonZombie()
    {
        isSpecialAttack = false;
        for(int i = 0; i <= UnityEngine.Random.Range(1,3); i ++)
        {
            GameObject bone = Instantiate(toolObjects[2], transform.position, Quaternion.identity);
            bone.GetComponent<CannonBall>().target = new Vector3(UnityEngine.Random.Range(2f, 8f), positionList[UnityEngine.Random.Range(0, 4)].position.y, 0f);
        }
        Invoke(nameof(ResetSpecialAttackCooldown), cooldownTime);
    }
    private void ChangePlantToSkeletonZombie()
    {
        int count = 0;
        int RandomHealth = UnityEngine.Random.Range(0, 3);
        isSpecialAttack = false;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, 8f);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Plant>(out Plant plant))
            {
                if (count <= RandomHealth)
                {
                    Debug.Log(count + " / " + RandomHealth + " - " + plant.tile.name);
                    count++;
                    GameObject skeletonZom = Instantiate(skeletonZombie, plant.transform.position, Quaternion.identity);
                    skeletonZom.GetComponent<Zombie>().lane = plant.tile.lane;
                    plant.Hit(100, 1);
                }
                else
                {
                    count = 0;
                    return;
                }
            }

        }
        Invoke(nameof(ResetSpecialAttackCooldown), cooldownTime);
    }

    private void summonZombie()
    {
        isSpecialAttack = false;

        GameObject skeletonZom1 = Instantiate(skeletonZombie, new Vector3(transform.position.x - 1.8f, transform.position.y, transform.position.z), Quaternion.identity);
        skeletonZom1.GetComponent<Zombie>().lane = lane;

        GameObject skeletonZom2 = Instantiate(skeletonZombie, new Vector3(transform.position.x + 1.8f, transform.position.y, transform.position.z), Quaternion.identity);
        skeletonZom2.GetComponent<Zombie>().lane = lane;

        if((int.Parse(lane) < 2))
        {    

        }
        else
        {
            GameObject skeletonZom3 = Instantiate(skeletonZombie, new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z), Quaternion.identity);
            skeletonZom3.GetComponent<Zombie>().lane = (int.Parse(lane) + 1).ToString();
        }

        if ((int.Parse(lane) > 5))
        {

        }
        else
        {
            GameObject skeletonZom4 = Instantiate(skeletonZombie, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity);
            skeletonZom4.GetComponent<Zombie>().lane = (int.Parse(lane) - 1).ToString();
        }

        Invoke(nameof(ResetSpecialAttackCooldown), cooldownTime);
    }

    private void Attack(Plant targetPlant)
    {
        isAttack = false;
        switch (formName)
        {
            case 0:
                {
                    targetPlant.Hit(damage, 1);
                    break;
                }
            case 1:
                {
                    targetPlant.Hit(damage, 1);
                    break;
                }
            case 2:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
        Invoke(nameof(ResetCooldown), 1f);
    }

    private void ResetCooldown()
    {
        isAttack = true;
    }
    private void ResetSpecialAttackCooldown()
    {
        isSpecialAttack = true;
        switch (formName)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    break;
                }
            case 2:
                {
                    // skill = UnityEngine.Random.Range(0, 3);
                    skill = 3;
                    break;
                }
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<Flame>(out Flame flame))
        {
            if(health + 10 >= maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += 10;
            }
            Destroy(flame.gameObject, 0.5f);
        }
    }
}
