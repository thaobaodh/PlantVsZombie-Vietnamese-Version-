using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RedImp : MonoBehaviour
{
    [SerializeField] List<GameObject> flamesList;

    [SerializeField] LayerMask shootMask;

    public Transform[] spawnPoints;

    public Transform breathPoint;

    public Transform castAbilityPoint;

    [SerializeField] bool StartToSummon = false;

    [SerializeField] float CooldownTime = 10f;

    [SerializeField] float speed = 1f;

    [SerializeField] int damage = 100;

    [SerializeField] float range = 10;

    private GameManager gms;

    [SerializeField] private int count = 0;

    [SerializeField] private int countFlame = 0;

    [SerializeField] List<int> skills;

    [SerializeField] int skill = 0;

    [SerializeField] Animator animator;

    [SerializeField] Vector3 targetPoint;

    [SerializeField] bool isReachTarget;

    [SerializeField] bool isReadyToCastAbility = false;

    // Start is called before the first frame update
    void Start()
    {
        StartToSummon = false;
        isReadyToCastAbility = false;
        animator.Play("Idle");
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        ResetCooldown();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gms.IsStart) return;
        if (!gms.StartSpawningZombie) return;
        if (flamesList.Count == 0) return;

        if (!StartToSummon) return;

        if (CheckReachTarget())
        {
            if (isReadyToCastAbility)
            {
                CastAbility();
            }
            else
            {
                switch (skill)
                {
                    case 0:
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            targetPoint.x = transform.position.x - 5f;
                            Debug.Log("New Target Point " + targetPoint);
                            isReachTarget = false;
                            break;
                        }
                    case 1:
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            targetPoint = castAbilityPoint.position;
                            Debug.Log("New Target Point " + targetPoint);
                            isReachTarget = false;
                            break;
                        }
                    case 2:
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            targetPoint.x = transform.position.x - 22.5f;
                            Debug.Log("New Target Point " + targetPoint);
                            isReachTarget = false;
                            break;
                        }
                    case 3:
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                            foreach (Collider2D collider2D in colliderArray)
                            {
                                if (collider2D.TryGetComponent<Plant>(out Plant plant))
                                {
                                    if (plant.gameObject != null)
                                    {
                                        targetPoint = plant.transform.position;
                                        Debug.Log("New Target Point " + targetPoint);
                                        isReachTarget = false;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        else
        {
            // Debug.Log("Going to target Point " +  transform.position.x + " / " + targetPoint.x); 
            switch (skill)
            {
                case 0:
                    {
                        speed = 2;
                        break;
                    }
                case 1:
                    {
                        speed = 2;
                        break;
                    }
                case 2:
                    {
                        speed = 5;
                        break;
                    }
                case 3:
                    {
                        speed = 2;
                        break;
                    }
                default:
                    break;
            }
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
        }

    }


    private bool CheckReachTarget()
    {
        if (isReachTarget) return true;

        if (transform.position.x >= targetPoint.x - 0.02 && transform.position.x < targetPoint.x + 0.02)
        {
            count++;
            switch (skill)
            {
                case 0:
                    {
                        if (count == 2)
                        {
                            isReadyToCastAbility = true;
                            count = 0;
                        }
                        break;
                    }
                case 1:
                    {
                        if (count == 2)
                        {
                            isReadyToCastAbility = true;
                            count = 0;
                        }
                        break;
                    }
                case 2:
                    {
                        if (count == 2)
                        {
                            isReadyToCastAbility = true;
                            count = 0;
                        }
                        break;
                    }
                case 3:
                    {
                        if (count == 2)
                        {
                            isReadyToCastAbility = true;
                            count = 0;
                        }
                        break;
                    }
                default:
                    { break; }
            }

            isReachTarget = true;
            return true;
        }
        return false;
    }
    private void CastAbility()
    {
        if (!StartToSummon) return;
        switch (skill)
        {
            case 0: // Breath a small flame
                {
                    // animator.Play("SummonWaves");
                    Debug.Log("Skill 0 ");
                    GameObject normalFlame = Instantiate(flamesList[0], breathPoint.position, Quaternion.identity);
                    normalFlame.GetComponent<Flame>().isNormalFlame = true;
                    normalFlame.GetComponent<Flame>().targetLock = false;
                    break;
                }
            case 1: //Breath 3 special flames in random tile
                {
                    // animator.Play("WaveBack");
                    Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                    foreach (Collider2D collider2D in colliderArray)
                    {
                        if (collider2D.TryGetComponent<Plant>(out Plant plant))
                        {
                            if (plant.gameObject != null)
                            {
                                if (countFlame >= 3)
                                {
                                    countFlame = 0;
                                    break;
                                }
                                else
                                {
                                    countFlame++;
                                    Debug.Log("Skill 1 - Tile summon: " + plant.gameObject.name);
                                    GameObject flame = Instantiate(flamesList[0], breathPoint.position, Quaternion.identity);
                                    flame.GetComponent<Flame>().transform.rotation = Quaternion.Euler(0, 180, 0);
                                    flame.GetComponent<Flame>().isNormalFlame = false;
                                    flame.GetComponent<Flame>().targetLock = true;
                                    flame.GetComponent<Flame>().target = plant.gameObject.transform.position;
                                }
                            }
                        }
                    }
                    break;
                }
            case 2: // Fly over 1 row from start to end.
                {
                    // animator.Play("WaveBack");
                    break;
                }
            case 3: // Use spear to attack the random plant.
                {
                    // animator.Play("WaveBack");
                    break;
                }
            default:
                break;
        }
        animator.Play("Idle");
        StartToSummon = false;
        isReadyToCastAbility = false;
        Invoke("ResetCooldown", CooldownTime);
    }

    private void ResetCooldown()
    {
        StartToSummon = true;

        skill = Random.Range(0, skills.Count);

        switch (skill)
        {
            case 0:
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    isReachTarget = false;
                    break;
                }
            case 1:
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    isReachTarget = false;
                    break;
                }
            case 2:
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    isReachTarget = false;
                    break;
                }
            case 3:
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    isReachTarget = false;
                    break;
                }
            default:
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Tile>(out Tile tile))
        {
            if (transform.position.x >= targetPoint.x && skill == 2)
            {
                GameObject flame = Instantiate(flamesList[0], tile.transform.position, Quaternion.identity);
                flame.GetComponent<Flame>().isNormalFlame = false;
                flame.GetComponent<Flame>().shooted = true;
                flame.GetComponent<Flame>().animator.Play("BigFlame");
            }
        }
        else
        {
            if (other.TryGetComponent<Plant>(out Plant plant))
            {
                if (plant.gameObject != null)
                {
                    if (plant.gameObject.transform.position == targetPoint)
                    {
                        plant.Hit(damage, 1);
                    }
                }
            }
        }
    }
}
