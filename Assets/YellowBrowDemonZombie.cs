using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBrowDemonZombie : MonoBehaviour
{
    [SerializeField] public int health;

    [SerializeField] public int maxHealth = 300;

    [SerializeField] public int damage = 30;

    [SerializeField] private bool isReadyToLanding = false;

    [SerializeField] private bool isSpecialAttackReady = false;

    [SerializeField] private Transform[] positionList;

    [SerializeField] private int skill;

    [SerializeField] private int numOfSkills = 5;

    [SerializeField] private float cooldownTime = 20f;

    [SerializeField] private GameObject[] toolObjects;

    [SerializeField] private float speed = 5f;

    [SerializeField] private Vector3 targetPoint;

    [SerializeField] private GameManager gms;

    private void LoadParams()
    {
        isSpecialAttackReady = false;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
        this.LoadParams();
        this.TimeCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gms.IsStart) return;
        if (!gms.StartSpawningZombie) return;
        this.CastAbility();
        if (!isReadyToLanding) return;
        this.LandingOnGround();
    }

    private void CastAbility()
    {
        if (!isSpecialAttackReady) return;

        //Chiêu thức:
        //1: Triệu hồi zombie bằng cách giơ chuỳ răng sói về phía trước
        //2: Đỡ đòn bằng chuỳ răng sói
        //3: Triệu hồi túi thần kỳ hút cây vào trong
        //4: Triệu hồi não bạt úp cây vào
        //5: Gần chết (còn 1/3 máu) thì ăn đào hồi máu -> tăng damage
        //6: Bay lên trời đạp xuống 1 vùng cây rồi bay trở về vị trí cũ
        // Start is called before the first frame update

        Debug.Log("Cast a skill: " + skill);
        switch(skill)
        {
            case 0:
                {
                    this.SummonZombie();
                    break;
                }
            case 1:
                {
                    this.SwingTheAxe();
                    break;
                }
            case 2:
                {
                    this.SummonAIncredibleBag();
                    break;
                }
            case 3:
                {
                    this.SummonABronzeGong();
                    break;
                }
            case 4:
                {
                    this.EatPeach();
                    break;
                }
            case 5:
                {
                    this.FindTargetToLandingOnGround();
                    break;
                }
            default:
                break;
        }
        isSpecialAttackReady = false;

        this.TimeCountdown();
    }

    private void SummonZombie()
    {
        int count = 0;
        int RandomHealth = UnityEngine.Random.Range(0, 3);

        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, 4f);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Tile>(out Tile tile))
            {
                if (count <= RandomHealth)
                {
                    Debug.Log(count + " / " + RandomHealth + " - " + tile.name);
                    count++;
                    GameObject zombie = Instantiate(toolObjects[0], tile.transform.position, Quaternion.identity);
                    zombie.GetComponent<Zombie>().lane = tile.lane;
                }
                else
                {
                    count = 0;
                    return;
                }
            }

        }
    }
    private void SwingTheAxe()
    {
        Instantiate(toolObjects[1], new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z), Quaternion.identity);
    }
    private void SummonAIncredibleBag()
    {
        Instantiate(toolObjects[2], new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z), Quaternion.identity);
    }
    private void SummonABronzeGong()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Plant>(out Plant plant))
            {
                GameObject bronzeGong = Instantiate(toolObjects[3], new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z), Quaternion.identity);
                bronzeGong.GetComponent<BronzeGong>().target = plant.transform.position;
                break;
                // bronzeGong.GetComponent<BronzeGong>().target = new Vector3(UnityEngine.Random.Range(-3.5f, 5.5f), UnityEngine.Random.Range(-4f, 3f), 0f);
            }
        }
    }
    private void EatPeach()
    {
        GameObject peach = Instantiate(toolObjects[4], new Vector3(UnityEngine.Random.Range(-3.5f, 5.5f), UnityEngine.Random.Range(-4f, 3f), 0f), Quaternion.identity);
        peach.GetComponent<Peach>().targetPoint = transform.position;
    }
    private void FindTargetToLandingOnGround()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Plant>(out Plant plant))
            {
                targetPoint = plant.transform.position;
                break;
            }
        }
        Invoke(nameof(PrepareLanding), 1f);
    }

    private void PrepareLanding()
    {
        speed = 5f;
        transform.position = new Vector3(targetPoint.x, 9f, 0f);
        isReadyToLanding = true;
    }

    private void LandingOnGround()
    {
        if(transform.position.y >= targetPoint.y - 0.15f && transform.position.y <= targetPoint.y + 0.15f)
        {
            speed = 0;
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, 2f);
            foreach (Collider2D collider2D in colliderArray)
            {
                if (collider2D.TryGetComponent<Plant>(out Plant plant))
                {
                    plant.Hit(damage, 1);
                }
            }
            isReadyToLanding = false;
        }
        else
        {
            transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
        }
    }

    private void TimeCountdown()
    {
        Invoke(nameof(ResetSpecialAbilityCooldown), cooldownTime);
    }

    private void ResetSpecialAbilityCooldown()
    {
        Invoke(nameof(MoveToSpawnPoint), 2f);
    }
    private void MoveToSpawnPoint()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(8.5f, 9.5f), positionList[UnityEngine.Random.Range(0, 4)].transform.position.y, 0);
        isSpecialAttackReady = true;
        this.ChooseASkill();
    }

    private void ChooseASkill()
    {
        skill = UnityEngine.Random.Range(0, numOfSkills);
        // skill = 5;
    }
}
