using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using Unity.VisualScripting;
using UnityEngine;

public class SeaGod : MonoBehaviour
{

    [SerializeField] List<GameObject> summonSeaZombiesList;

    [SerializeField] List<GameObject> tsunamiList;

    [SerializeField] LayerMask tileMask;

    public Transform[] spawnPoints;

    [SerializeField] int numOfZombiesSummon = 0;

    [SerializeField] bool StartToSummon = false;

    [SerializeField] float CooldownTime = 10f;

    [SerializeField] float range = 10;

    private GameManager gms;

    [SerializeField] private int count = 0;

    [SerializeField] List<int> skills;

    [SerializeField] int skill = 0;

    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Idle");
        count = 0;
        StartToSummon = true;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gms.IsStart) return;
        if (!gms.StartSpawningZombie) return;
        if (!StartToSummon) return;
        if (summonSeaZombiesList.Count == 0 || tsunamiList.Count == 0) return;

        if (count >= numOfZombiesSummon)
        {
            skill = Random.Range(2, 5);
        }
        else
        {
            skill = Random.Range(0, skills.Count);
        }

        switch (skill)
        {
            case 0: // Summon sea Zombie in all lanes immediately
                {
                    animator.Play("SummonZombies");
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        GameObject seaZombie = Instantiate(summonSeaZombiesList[Random.Range(0, summonSeaZombiesList.Count)], spawnPoints[i].position, Quaternion.identity);
                        seaZombie.GetComponent<Zombie>().lane = (i + 1).ToString();
                        count++;
                    }
                    break;
                }
            case 1:// Summon random sea Zombie in random lanes
                {
                    animator.Play("SummonZombies");
                    for (int i = 0; i < Random.Range(1, spawnPoints.Length); i++)
                    {
                        int random = Random.Range(0, spawnPoints.Length);
                        GameObject seaZombie = Instantiate(summonSeaZombiesList[Random.Range(0, summonSeaZombiesList.Count)], spawnPoints[random].position, Quaternion.identity);
                        seaZombie.GetComponent<Zombie>().lane = (random + 1).ToString();
                        count++;
                    }
                    break;
                }
            case 2: // Slash instantly one plant
                {
                    animator.Play("SummonWaves");
                    Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                    foreach (Collider2D collider2D in colliderArray)
                    {
                        if (collider2D.TryGetComponent<Tile>(out Tile tile))
                        {
                            if (tile.plantObject != null)
                            {
                                Debug.Log("Skill 2 - Tile summon: " + tile.name);
                                GameObject tsunami = Instantiate(tsunamiList[1], tile.transform.position, Quaternion.identity);
                                tsunami.GetComponent<BigWave>().isReverse = true;
                                tsunami.GetComponent<BigWave>().justKillOneZombie = true;
                            }

                            break;
                        }
                    }
                    break;
                }
            case 3: // Slash instantly some plant in row
                {
                    animator.Play("WaveBack");
                    Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                    foreach (Collider2D collider2D in colliderArray)
                    {
                        if (collider2D.TryGetComponent<Tile>(out Tile tile))
                        {
                            if(tile.plantObject != null)
                            {
                                Debug.Log("Skill 3 - Tile summon: " + tile.name);
                                GameObject tsunami = Instantiate(tsunamiList[0], tile.transform.position, Quaternion.identity);
                                tsunami.GetComponent<BigWave>().isReverse = true;
                                tsunami.GetComponent<BigWave>().justKillOneZombie = false;
                            }

                            break;
                        }
                    }
                    break;
                }
            case 4: // Summon a tsunami in one lane
                {
                    animator.Play("WaveSlash");
                    int i = Random.Range(0, spawnPoints.Length);
                    GameObject tsunami = Instantiate(tsunamiList[0], new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y - 0.75f, 0), Quaternion.identity);
                    tsunami.transform.rotation = Quaternion.Euler(0, 180, 0); 
                    tsunami.GetComponent<BigWave>().isReverse = false;
                    tsunami.GetComponent<BigWave>().justKillOneZombie = false;
                    break;
                }
            case 5: // Summon a tsunami in all lane
                {
                    animator.Play("WaveSlash");
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        GameObject tsunami = Instantiate(tsunamiList[0], new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y - 0.75f, 0), Quaternion.identity);
                        tsunami.transform.rotation = Quaternion.Euler(0, 180, 0);
                        tsunami.GetComponent<BigWave>().isReverse = false;
                        tsunami.GetComponent<BigWave>().justKillOneZombie = false;
                    }
                    break;
                }
            default:
                break;
        }
        animator.Play("Idle");
        StartToSummon = false;
        Invoke("ResetCooldown", CooldownTime);
    }

    private void ResetCooldown()
    {
        StartToSummon = true;
    }
}
