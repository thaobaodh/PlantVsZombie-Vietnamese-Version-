using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Fixed an error when clicking on the Mountain God and summoning the Tallnut wall -> Delay
// Fix Cob Cannon error -> It doesn't recover as I expected

public class MoutainGod : MonoBehaviour
{
    public bool canRaise = false;

    public int ActiveStep;

    public Vector3 shootDestination;

    public Animator animator;

    [SerializeField] GameObject[] listOfObjects;

    [SerializeField] Transform throwPosition;

    [SerializeField] Transform[] wallnutPosition;

    public float range;

    public LayerMask shootMask;

    private AudioSource source;

    public AudioClip[] shootClips;

    public int skill = 0;

    [SerializeField] int numOfSkills = 5;

    [SerializeField] int numOfTrees = 15;

    public int tileChoiceName;

    private GameManager gms;

    [SerializeField] bool StartToSummon = false;

    [SerializeField] float CooldownRaiseTime = 10f;

    [SerializeField] float CooldownTime = 10f;

    private float targetX;

    private float targetY;

    private GameObject[] targets;

    [SerializeField] List<int> TallnutPositionLists;

    private void Start()
    {
        animator.Play("Idle");
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        canRaise = false;
        ActiveStep = 0;
        Invoke("CannonSet", 1);
        source = gameObject.AddComponent<AudioSource>();
        StartToSummon = true;
    }
    void CannonSet()
    {
        canRaise = true;
        ActiveStep = 1;
        Debug.Log("MountainGod Set");
    }

    private void Update()
    {
        if (!gms.IsStart) return;

        if (ActiveStep == 3)
        {
            animator.Play("RaiseTheWall");

            Debug.Log("Mountain God - Raise the wall");

            SummonARowofTallnut(listOfObjects[3], tileChoiceName);
        }
        else
        {
            if (!gms.StartSpawningZombie) return;
            if (!StartToSummon) return;
            skill = Random.Range(0, numOfSkills);
        }

        switch (skill)
        {
            case 0: // Throw a stone to only one zombie
                {
                    ThrowAThing(listOfObjects[0]);
                    break;
                }
            case 1: // Throw some stone to zombie position
                {
                    ThrowSomeThing(listOfObjects[0]);
                    break;
                }
            case 2: // Summon a rolling wallnut in some lane to destroy zombie 
                {
                    SummonRollingWallnut(listOfObjects[2]);
                    break;
                }
            case 3: // Summon lightning strike to some position
                {
                    SummonSomeLightningStrike(listOfObjects[1], 3);
                    break;
                }
            case 4: // Summon lightning strike to some position
                {
                    SummonSomeLightningStrike(listOfObjects[1], numOfTrees);
                    break;
                }
            default:
                break;
        }
    }
    private void ResetRaiseCooldown()
    {
        canRaise = true;
        ActiveStep = 1; 
        animator.Play("Idle");
        Debug.Log("MountainGod Ready");
    }
    private void ResetCooldown()
    {
        StartToSummon = true;
    }

    void SummonRollingWallnut(GameObject thing)
    {
        for(int i = 0; i < Random.Range(1, Random.Range(2,5)); i++)
        {
            GameObject nut = Instantiate(thing, wallnutPosition[Random.Range(0, wallnutPosition.Length)].position, Quaternion.identity);
            nut.GetComponent<Nuts>().Roll = true;
        }
        StartToSummon = false;
        Invoke("ResetCooldown", CooldownTime);
        animator.Play("Waiting");
    }

    void SummonARowofTallnut(GameObject thing, int tileChoiceName)
    {
        if (!canRaise) return;

        int count = 0;
        int num = tileChoiceName % 9;

        for(int i = 0; i < 5; i ++)
        {
            TallnutPositionLists.Add(num + 9 * i);
            // Debug.Log("Position: " + TallnutPositionLists[i]);
        }

        targets = GameObject.FindGameObjectsWithTag("Tile");
        if (targets == null)
        {

        }
        else
        {
            foreach (GameObject _target in targets)
            {
                // Debug.Log("_target: " + _target.name);
                for(int j = 0; j < TallnutPositionLists.Count; j++)
                {
                    if (_target.name == TallnutPositionLists[j].ToString())
                    {
                        // Debug.Log("_target matched: " + _target.name + " / j:" + j + " / Position: " + TallnutPositionLists[j].ToString());
                        count++;
                        if(_target.GetComponent<Tile>().plantObject == null)
                        {
                            GameObject TallnutRow = Instantiate(thing, _target.transform.position, Quaternion.identity);
                            TallnutRow.GetComponent<Plant>().tile = _target.GetComponent<Tile>();
                            TallnutRow.GetComponent<Plant>().tile.plantObject = TallnutRow;
                            TallnutRow.GetComponent<Plant>().tile.hasPlant = true;
                            break;
                        }
                        else
                        {
                            j++;
                        }

                    }
                }

                if (count >= TallnutPositionLists.Count)
                {
                    count = 0;
                    TallnutPositionLists.Clear();
                    ActiveStep = 0;
                    Invoke(nameof(ResetRaiseCooldown), CooldownRaiseTime);
                }
            }
        }
    }

    void ThrowAThing(GameObject thing)
    {
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
            {
                GameObject tree = Instantiate(thing, throwPosition.position, Quaternion.identity);
                tree.GetComponent<Trees>().target = zombie.gameObject;
                break;
            }
            else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
            {
                GameObject tree = Instantiate(thing, throwPosition.position, Quaternion.identity);
                tree.GetComponent<Trees>().target = balloonZombie.gameObject;
                break;
            }
        }

        StartToSummon = false;
        Invoke("ResetCooldown", CooldownTime);
    }

    void ThrowSomeThing(GameObject thing)
    {
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);

        for (int i = 0; i < numOfTrees; i++)
        {

            foreach (Collider2D collider2D in colliderArray)
            {
                if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
                {
                    targetX = zombie.transform.position.x + Random.Range(-3f, 3f);
                    targetY = zombie.transform.position.y + Random.Range(-3f, 3f);
                    GameObject tree = Instantiate(thing, throwPosition.position, Quaternion.identity);
                    tree.GetComponent<Trees>().destinationPosition = new Vector3(targetX, targetY, 0f);
                    tree.GetComponent<Trees>().target = null;
                    break;
                }
                else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                {
                    targetX = balloonZombie.transform.position.x + Random.Range(-3f, 3f);
                    targetY = balloonZombie.transform.position.y + Random.Range(-3f, 3f);
                    GameObject tree = Instantiate(thing, throwPosition.position, Quaternion.identity);
                    tree.GetComponent<Trees>().destinationPosition = new Vector3(targetX, targetY, 0f);
                    tree.GetComponent<Trees>().target = null;
                    break;
                }
            }
        }
        StartToSummon = false;
        Invoke("ResetCooldown", CooldownTime);
    }

    void SummonSomeLightningStrike(GameObject thing, int numOfLightningStrike)
    {
        int count = 0;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
            {
                GameObject lightning = Instantiate(thing, zombie.transform.position, Quaternion.identity);
                lightning.GetComponent<LightningStrike>().Destination = zombie.transform.position;
                count++;
                if(count >= numOfLightningStrike)
                {
                    break;
                }
            }
            else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
            {
                GameObject lightning = Instantiate(thing, balloonZombie.transform.position, Quaternion.identity);
                lightning.GetComponent<LightningStrike>().Destination = balloonZombie.transform.position;
                count++;
                if (count >= numOfLightningStrike)
                {
                    break;
                }
            }
        }
        StartToSummon = false;
        Invoke("ResetCooldown", CooldownTime);
    }
}
