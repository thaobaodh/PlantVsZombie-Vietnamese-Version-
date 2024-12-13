using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieBoat : MonoBehaviour
{

    [SerializeField] List<GameObject> summonBoatList;

    public Transform cannonBallPosition;

    public Transform[] spawnPoints;

    [SerializeField] int health = 500;

    [SerializeField] int numCannonBall = 3;

    [SerializeField] int numOfZombiesSummon = 0;

    [SerializeField] bool StartToSummon = false;

    [SerializeField] float CooldownTime = 10f;

    [SerializeField] float range = 10;

    [SerializeField] float speed = 2;

    [SerializeField] int damage = 10;

    private GameManager gms;

    private Vector3 targetPosition;

    private bool goBack = false;

    private bool TheBoatIsComing = false;

    private bool comingToLand = false;

    [SerializeField] float distanceToGo = 7;
    
    [SerializeField] int count = 0;

    [SerializeField] List<int> skills;

    [SerializeField] int skill = 0;

    [SerializeField] Animator animator;

    public bool dead = false;

    private Vector3 firstPosition;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Idle");
        count = 0; 
        dead = false;
        StartToSummon = true;
        comingToLand = false;
        TheBoatIsComing = false;
        firstPosition = transform.position;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gms.IsStart) return;
        if (!gms.StartSpawningZombie) return;
        if (!StartToSummon) return;
        if (summonBoatList.Count == 0) return;

        if (count >= numOfZombiesSummon)
        {
            skill = Random.Range(2, skills.Count);
        }
        else
        {
            skill = Random.Range(0, skills.Count);
        }

        switch (skill)
        {
            case 0: // Summon sea Zombie in all lanes immediately
                {
                    // animator.Play("SummonZombies");
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        GameObject tinyBoat = Instantiate(summonBoatList[0], spawnPoints[i].position, Quaternion.identity);
                        tinyBoat.GetComponent<TinyZombieBoat>().lane = (i + 1).ToString();
                        count++;
                    }
                    StartToSummon = false;
                    Invoke("ResetCooldown", CooldownTime);
                    break;
                }
            case 1:// Summon random sea Zombie in random lanes
                {
                    // animator.Play("SummonZombies");
                    for (int i = 0; i < Random.Range(1, spawnPoints.Length); i++)
                    {
                        int random = Random.Range(0, spawnPoints.Length);
                        GameObject tinyBoat = Instantiate(summonBoatList[0], spawnPoints[random].position, Quaternion.identity);
                        tinyBoat.GetComponent<TinyZombieBoat>().lane = (random + 1).ToString();
                        count++;
                    }
                    StartToSummon = false;
                    Invoke("ResetCooldown", CooldownTime);
                    break;
                }
            case 2: // Shoot a single cannon ball
                {
                    //// animator.Play("SummonWaves");
                    Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                    foreach (Collider2D collider2D in colliderArray)
                    {
                        if (collider2D.TryGetComponent<Tile>(out Tile tile))
                        {
                            if (tile.plantObject != null || tile.backgroundObject != null || tile.pumpkinObject != null)
                            {
                                Debug.Log("Skill 2 - Tile summon: " + tile.name);
                                GameObject cannonBall = Instantiate(summonBoatList[1], cannonBallPosition.position, Quaternion.identity);
                                cannonBall.GetComponent<CannonBall>().target = tile.transform.position;

                                StartToSummon = false;
                                Invoke("ResetCooldown", CooldownTime);
                                break;
                            }
                        }
                    }
                    break;
                }
            case 3: // Shoot a numCannonBall cannon ball
                {
                    int cnt = 0;
                    // animator.Play("WaveBack");
                    Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                    foreach (Collider2D collider2D in colliderArray)
                    {
                        if (collider2D.TryGetComponent<Tile>(out Tile tile))
                        {
                            if (tile.plantObject != null || tile.backgroundObject != null || tile.pumpkinObject != null)
                            {
                                Debug.Log("Skill 3 - Tile summon: " + tile.name);
                                GameObject cannonBall = Instantiate(summonBoatList[1], cannonBallPosition.position, Quaternion.identity);
                                cannonBall.GetComponent<CannonBall>().target = tile.transform.position;
                                cnt++;
                                if(cnt >= numCannonBall)
                                {
                                    StartToSummon = false;
                                    Invoke("ResetCooldown", CooldownTime);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            case 4:
                {
                    comingToLand = true;
                    TheBoatIsComing = true;
                    targetPosition = new Vector3(transform.position.x - distanceToGo, transform.position.y, 0);
                    Debug.Log("Coming To Land " + comingToLand + " / " + targetPosition);
                    StartToSummon = false;
                    break;
                }
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!TheBoatIsComing) return;

        if (comingToLand)
        {
            if (transform.position.x <= targetPosition.x)
            {
                comingToLand = false;
                targetPosition = new Vector3(transform.position.x + distanceToGo, transform.position.y, 0);
            }
            else
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
            }
        }
        else
        {
            if (transform.position.x >= targetPosition.x)
            {
                // animator.Play("Idle");
                Debug.Log("You Can See From Here");
                TheBoatIsComing = false;
                StartToSummon = false;
                Invoke("ResetCooldown", CooldownTime);
            }
            else
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<Plant>(out Plant plant))
        {
            if(plant != null)
            {
                if(plant.name == "PileField(Clone)")
                {
                    plant.Hit(10, 1);
                    comingToLand = false;
                    targetPosition = new Vector3(transform.position.x + Vector3.Distance(transform.position, firstPosition), transform.position.y, 0);
                }
                else
                {
                    Debug.Log("Collision: " + plant.name);
                    plant.Hit(damage, 1);
                }
            }
        }
    }

    private void ResetCooldown()
    {
        Debug.Log("StartToSummon " + StartToSummon);
        StartToSummon = true;
    }

    public void Hit(int damage)
    {
        if (dead) return;
        if (health > 0)
        {
            health -= damage;
            if(health <= 0)
            {
                dead = true;
                Destroy(gameObject, 2);
            }
        }
    }
}
