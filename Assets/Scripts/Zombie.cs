using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public string lane;

    private bool ZombieInPool = false;

    private bool hasLeftHand = true;

    private float speed;

    private int health;

    private string zName;

    private int damage;

    private float range;

    public GameObject LeftHandObject;

    public GameObject ButterObject;

    public GameObject IceObject;

    private GameObject butterOnHead;

    private GameObject iceOnFoot;

    public GameObject MetalObject;

    private GameObject NormalObject;

    [SerializeField] LayerMask plantMask;

    [SerializeField] LayerMask zombieMask;

    [SerializeField] LayerMask tileMask;

    private bool isFreeze = false;

    private bool canCastSpecialAbility = true;

    private bool canEat = true;

    private Plant targetPlant;

    private Zombie targetZombie;

    private Zombie targetPoisonedZombie;

    public ZombieType type;

    private AudioSource source;

    [SerializeField] GameObject attackToolsList;

    [SerializeField] Transform attackPosition;
    
    [SerializeField] AudioClip[] groans;

    [SerializeField] AudioClip[] chomps;

    [SerializeField] AudioClip[] yuck;

    [SerializeField] Animator animator;

    [SerializeField] bool readyAnimator = false;

    public bool lastZombie;

    public bool dead;

    private int max_health;

    private bool eatZombie = false;

    private bool switchLand = false;

    private bool goUpOrDown;

    private float DestY;

    private float distance = 0;

    private GameObject[] targets;

    private Vector3 zombieLeftUpperHandPosition;

    private Vector3 zombieHairPosition;

    private Vector3 zombieHeadPosition;

    private Vector3 zombieConePosition;

    private Vector3 zombieFootPosition;

    private Vector3 zombieHipPosition;

    private GameManager gms;

    private float cooldown;

    private float castCooldown;
    void Groans()
    {
        source.PlayOneShot(groans[Random.Range(0, groans.Length)]);
    }

    void Chomps()
    {
        source.PlayOneShot(chomps[Random.Range(0, chomps.Length)]);
        source.volume = 0.3f;
    }
    void Yuck()
    {
        source.PlayOneShot(yuck[Random.Range(0, yuck.Length)]);
    }
    protected virtual void Start()
    {
        if (readyAnimator)
        {
            animator.SetInteger("Status", 0);
            zombieHeadPosition = gameObject.transform.GetChild(0).transform.GetChild(0).transform.position;
            zombieConePosition = gameObject.transform.GetChild(0).transform.GetChild(1).transform.position;
        }

        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        eatZombie = false;
        zName = type.zName;
        health = type.health;
        max_health = health;
        speed = type.speed;
        damage = type.damage;
        range = type.range;
        cooldown = type.eatCooldown;
        castCooldown = type.eatCooldown * 20;

        if (zName == "BucketZombie")
        {
            MetalObject = Instantiate(type.tool, zombieHeadPosition, Quaternion.identity);
        }
        else if (zName == "ConeZombie")
        {
            NormalObject = Instantiate(type.tool, zombieConePosition, Quaternion.identity);
        }

        source = GetComponent<AudioSource>();
        Invoke("Groans", Random.Range(1, 20));
    }


    void InPool()
    {
        if (readyAnimator)
        {
            animator.SetInteger("Status", 9);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (gms.IsStart)
        {

            if (canCastSpecialAbility)
            {
                if (zName == "SpiderDemonZombie" || zName == "OldSpiderDemonZombie")
                {
                    ThrowSpiderWeb();
                }
            }

            if (readyAnimator)
            {
                if (animator.GetInteger("Status") < 1)
                {
                    animator.SetInteger("Status", 1);
                }
                else if (animator.GetInteger("Status") == 2)
                {
                    animator.SetInteger("Status", 3);
                }
                zombieHeadPosition = gameObject.transform.GetChild(0).transform.GetChild(0).transform.position;
                zombieConePosition = gameObject.transform.GetChild(0).transform.GetChild(1).transform.position;
                // Debug.Log(gameObject.transform.GetChild(0).transform.GetChild(0).name + " --- " + zombieHeadPosition); // Get Zombie Hair Position


                if (MetalObject != null)
                {
                    if (health == max_health && MetalObject.name == "Bucket(Clone)" && MetalObject.GetComponent<Bucket>().isDrop == false)
                    {
                        MetalObject.transform.SetPositionAndRotation(zombieHeadPosition, gameObject.transform.GetChild(0).transform.GetChild(0).transform.rotation);
                    }
                    else
                    {

                    }
                }
                else if (NormalObject != null)
                {
                    if (health == max_health && NormalObject.name == "Cone(Clone)" && NormalObject.GetComponent<Cone>().isDrop == false)
                    {
                        NormalObject.transform.SetPositionAndRotation(zombieConePosition, gameObject.transform.GetChild(0).transform.GetChild(1).transform.rotation);
                    }
                    else
                    {

                    }
                }
            }
            RaycastHit2D tileHit = Physics2D.Raycast(transform.position, Vector2.left, range, tileMask);

            if (tileHit.collider && !ZombieInPool)
            {
                if (tileHit.collider.GetComponent<Tile>() != null)
                {
                    if (tileHit.collider.GetComponent<Tile>().isPool && tileHit.collider.GetComponent<Tile>().transform.position.y >= gameObject.transform.position.y - 0.7)
                    {
                        if (gameObject.transform.position.x <= 8.5f)
                        {
                            ZombieInPool = true;
                            if (readyAnimator)
                            {
                                animator.SetInteger("Status", 8);
                            }
                            Invoke("InPool", 1);
                        }
                    }
                }
            }

            if (eatZombie)
            {
                RaycastHit2D zombieHit = Physics2D.Raycast(transform.position, Vector2.right, range, zombieMask);

                if (zombieHit.collider)
                {
                    if (readyAnimator)
                    {
                        if (animator.GetInteger("Status") == 1 || animator.GetInteger("Status") == 3)
                        {
                            animator.SetInteger("Status", 2);
                        }
                    }
                    targetZombie = zombieHit.collider.GetComponent<Zombie>();
                    EatZombie();
                }
                else
                {
                    if (readyAnimator)
                    {
                        if (animator.GetInteger("Status") == 2)
                        {
                            animator.SetInteger("Status", 3);
                        }
                    }
                    if(gms.Roof)
                    {
                        if (gameObject.transform.position.x >= 2.7)
                        {
                            gameObject.transform.position += new Vector3(speed, 0, 0);
                        }
                        else
                        {
                            gameObject.transform.position += new Vector3(speed, speed / 5, 0);
                        }
                    }
                    else
                    {
                        gameObject.transform.position += new Vector3(speed, 0, 0);
                    }
                }
            }
            else
            {

                if (transform.position.x <= 8.5)
                {
                    RaycastHit2D plantHit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);

                    if (plantHit.collider)
                    {
                        if (plantHit.collider.GetComponent<Plant>() != null)
                        {
                            if (ZombieInPool)
                            {
                                if (readyAnimator)
                                {
                                    // Debug.Log("Animator integer: " + animator.GetInteger("Status"));
                                    if (animator.GetInteger("Status") == 9 || animator.GetInteger("Status") == 13)
                                    {
                                        animator.SetInteger("Status", 10);
                                    }
                                }
                            }
                            else
                            {
                                if (plantHit.collider.gameObject.name == "Caltrop(Clone)" || plantHit.collider.gameObject.name == "SpikeRock(Clone)")
                                {
                                    if (readyAnimator)
                                    {
                                        animator.SetInteger("Status", 1);
                                    }
                                }
                                else
                                {
                                    if (readyAnimator)
                                    {
                                        if (animator.GetInteger("Status") == 1 || animator.GetInteger("Status") == 3)
                                        {
                                            animator.SetInteger("Status", 2);
                                        }
                                    }
                                }
                            }

                            if (plantHit.collider.gameObject.name == "Caltrop(Clone)" || plantHit.collider.gameObject.name == "SpikeRock(Clone)")
                            {
                                canEat = false;

                                gameObject.transform.position -= new Vector3(speed, 0, 0);
                            }
                            else if (plantHit.collider.gameObject.name == "BossVase(Clone)" || plantHit.collider.gameObject.name == "MysticVase(Clone)" || plantHit.collider.gameObject.name == "PlantVase(Clone)")
                            {
                                canEat = false;

                                gameObject.transform.position -= new Vector3(speed, 0, 0);
                            }
                            else if (plantHit.collider.gameObject.name == "Garlic(Clone)")
                            {
                                if (switchLand)
                                {

                                }
                                else
                                {
                                    targetPlant = plantHit.collider.GetComponent<Plant>();
                                    targetPlant.GetComponent<BoxCollider2D>().isTrigger = false;
                                    //targetPlant.gameObject.layer = 12;
                                    EatPlantAndGoosebump();
                                }
                            }
                            else
                            {
                                targetPlant = plantHit.collider.GetComponent<Plant>();
                                switch (zName)
                                {
                                    case "SwordZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "CannonShooterZombie":
                                        {
                                            AttackPlant(targetPlant);
                                            // Debug.Log("Cannon Shooter Zombie attack Plant");
                                            break;
                                        }
                                    case "SoldierZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "SpearZombie":
                                        {
                                            AttackPlant(targetPlant);
                                            // Debug.Log("Spear Zombie attack Plant");
                                            break;
                                        }
                                    case "StoneThrowerZombie":
                                        {
                                            AttackPlant(targetPlant);
                                            // Debug.Log("Stone Thrower Zombie attack Plant");
                                            break;
                                        }
                                    case "WarChariotZombie":
                                        {
                                            AttackPlant(targetPlant);
                                            // Debug.Log("War Chariot Zombie attack Plant");
                                            break;
                                        }
                                    case "SpiderDemonZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "BalloonZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "BucketParachuteZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "BucketZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "ClassicZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "ConeParachuteZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "ConeZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    case "ParachuteZombie":
                                        {
                                            EatPlant();
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (plantHit.collider.GetComponent<Zombie>() != null)
                        {
                            if (ZombieInPool)
                            {
                                if (readyAnimator)
                                {
                                    if (animator.GetInteger("Status") == 9)
                                    {
                                        animator.SetInteger("Status", 10);
                                    }
                                }
                            }
                            else
                            {
                                if (readyAnimator)
                                {
                                    if (animator.GetInteger("Status") == 1 || animator.GetInteger("Status") == 3)
                                    {
                                        animator.SetInteger("Status", 2);
                                    }
                                }
                            }
                            targetPoisonedZombie = plantHit.collider.GetComponent<Zombie>();
                            EatPoisonedZombie();
                        }
                    }
                    else
                    {
                        if (ZombieInPool)
                        {
                            if (readyAnimator)
                            {
                                if (animator.GetInteger("Status") == 10)
                                {
                                    animator.SetInteger("Status", 13);
                                }
                            }
                        }
                        else
                        {
                            if (readyAnimator)
                            {
                                if (animator.GetInteger("Status") == 2)
                                {
                                    animator.SetInteger("Status", 3);
                                }
                            }
                        }

                        if (gms.Roof)
                        {
                            if (gameObject.transform.position.x >= 2.7)
                            {
                                gameObject.transform.position -= new Vector3(speed, 0, 0);
                            }
                            else
                            {
                                gameObject.transform.position -= new Vector3(speed, speed / 5, 0);
                            }
                        }
                        else
                        {
                            gameObject.transform.position -= new Vector3(speed, 0, 0);
                        }
                    }
                }
                else
                {
                    gameObject.transform.position -= new Vector3(speed, 0, 0);
                }

                if (switchLand)
                {
                    if (readyAnimator)
                    {
                        if (animator.GetInteger("Status") == 2)
                        {
                            animator.SetInteger("Status", 3);
                        }
                    }
                    if (goUpOrDown) // Go Up
                    {
                        distance += 0.1f;
                        // Debug.Log("distance go up: " + distance);

                        gameObject.transform.position += new Vector3(-speed, speed, 0);
                        if (distance >= 34f)
                        {
                            switchLand = false;
                            //targetPlant.gameObject.layer = 9;
                            targetPlant.GetComponent<BoxCollider2D>().isTrigger = true;
                            distance = 0;
                            this.lane = (int.Parse(this.lane) - 1).ToString();
                        }

                    }
                    else // Go Down
                    {
                        distance += 0.1f;
                        // Debug.Log("distance go down: " + distance);
                        gameObject.transform.position -= new Vector3(speed, speed, 0);
                        if (distance >= 34f)
                        {
                            switchLand = false;
                            targetPlant.GetComponent<BoxCollider2D>().isTrigger = true;
                            //targetPlant.gameObject.layer = 9;
                            distance = 0;
                            this.lane = (int.Parse(this.lane) + 1).ToString();
                        }
                    }
                }
            }
        }
    }
    void EatPlantAndGoosebump()
    {
        if (!canEat || !targetPlant)
            return;
        canEat = false;
        Chomps();
        Invoke("ResetEatCooldown", cooldown);
        targetPlant.Hit(damage, 0);
        Yuck();
        switchLand = true;

        if (switchLand)
        {
            if (gameObject.transform.position.y >= 3.0f) // Go down
            {
                goUpOrDown = false;
                // Debug.Log("First Land 1-> go down");
                //gameObject.transform.position -= new Vector3(1.7f, 1.7f, 0);
                //switchLand = false;
            }
            else if (gameObject.transform.position.y <= -3.5f) // Go up
            {

                goUpOrDown = true;

                // Debug.Log("Last Land 1-> go up");
                //gameObject.transform.position += new Vector3(-1.7f, 1.7f, 0);
                //switchLand = false;
            }
            else
            {
                if (Random.Range(0, 1) == 0) // Go up
                {
                    // Debug.Log("3 Middle Land - Go Up");
                    if (gameObject.transform.position.y + 1.7 <= 3.28)
                    {
                        goUpOrDown = true;

                        // Debug.Log("2,3,4 Land -> go up");
                        //gameObject.transform.position += new Vector3(-1.7f, 1.7f, 0);
                        //switchLand = false;
                    }
                    else
                    {
                        goUpOrDown = false;
                        // Debug.Log("First Land 2-> go down");
                        //gameObject.transform.position -= new Vector3(1.7f, 1.7f, 0);
                        //switchLand = false;
                    }
                }
                else // Go down
                {
                    // Debug.Log("3 Middle Land - Go Down");
                    if (gameObject.transform.position.y + 1.7 >= -3.84)
                    {
                        goUpOrDown = false;

                        // Debug.Log("2,3,4 Land -> go down");
                        //gameObject.transform.position -= new Vector3(1.7f, 1.7f, 0);
                        //switchLand = false;
                    }
                    else
                    {
                        goUpOrDown = true;

                        // Debug.Log("Last Land 2 -> go up");
                        //gameObject.transform.position += new Vector3(-1.7f, 1.7f, 0);
                        //switchLand = false;
                    }
                }

            }
        }
    }

    void AttackPlant(Plant targetPlant)
    {
        if (!canEat || !targetPlant)
            return;
        if (attackToolsList == null || attackPosition == null) return;
        canEat = false;
        Chomps();
        GameObject attackTool = Instantiate(attackToolsList, attackPosition.position, Quaternion.identity);
        attackTool.GetComponent<CannonBall>().targetLock = true;
        attackTool.GetComponent<CannonBall>().target = targetPlant.transform.position;
        // Debug.Log("Attack: " + targetPlant.name + " / " + attackTool.GetComponent<CannonBall>().target);
        Invoke(nameof(ResetEatCooldown), cooldown);
    }

    void ThrowSpiderWeb()
    {
        if (!canCastSpecialAbility)
            return;
        if (attackToolsList == null || attackPosition == null) return;
        canCastSpecialAbility = false;
        // Chomps();
        GameObject attackTool = Instantiate(attackToolsList, attackPosition.position, Quaternion.identity);
        Invoke(nameof(ResetCastCooldown), castCooldown);
    }

    protected virtual void EatPlant()
    {
        if (!canEat || !targetPlant)
            return;
        // Debug.Log("Eat " + targetPlant.name);

        if (targetPlant.name == "TheBut(Clone)")
        {
            if ((Random.Range(0, 5) % 2) == 0)
            {
                // Debug.Log("Teaching Zombie");
                BePoisoned();
            }
            else
            {
                // Debug.Log("Destroy Zombie");
                Hit(100, false, false, false);
            }
            targetPlant.GetComponent<TheBut>().animator.SetInteger("State", 6);
            targetPlant.GetComponent<Plant>().Hit(100, 2);
            return;
        }
        canEat = false;
        Chomps();
        targetPlant.Hit(damage, 0);
        Invoke(nameof(ResetEatCooldown), cooldown);
        if (targetPlant.name == "HypnoShroom(Clone)")
        {
            BePoisoned();
        }
    }

    protected virtual void EatZombie()
    {
        if (!canEat || !targetZombie)
            return;
        canEat = false;
        Chomps();
        Invoke("ResetEatCooldown", cooldown);
        targetZombie.Hit(damage, false, false, false);
    }


    protected virtual void EatPoisonedZombie()
    {
        if (!canEat || !targetPoisonedZombie)
            return;
        canEat = false;
        Chomps();
        Invoke("ResetEatCooldown", cooldown);
        targetPoisonedZombie.Hit(damage, false, false, false);
    }

    void ResetEatCooldown()
    {
        if (isFreeze)
        {
            //Debug.Log("Freeze - Stop Eat");
            canEat = false;
        }
        else
        {
            //Debug.Log("Not Freeze - Eat");
            canEat = true;
        }
    }

    void ResetCastCooldown()
    {
        canCastSpecialAbility = false;
    }

    public bool getZombieStatus()
    {
        return eatZombie;
    }

    public void Recover()
    {
        transform.Rotate(new Vector3(0, 180, 0));
        eatZombie = false;
        gameObject.layer = LayerMask.NameToLayer("Target");
    }

    public void BePoisoned()
    {
        //Debug.Log("Zombie be poisoned");
        GetComponent<SpriteRenderer>().color = Color.magenta;
        transform.Rotate(new Vector3(0, 180, 0));
        eatZombie = true;
        gameObject.layer = LayerMask.NameToLayer("Plant");
    }

    public void Hit(int damage, bool beCold, bool flame, bool freeze)
    {

        //if(gms.notPreviewZombie)
        //{
        //    if (!gms.StartSpawningZombie) return;
        //}

        if (dead) return;

        if (damage <= 1) // FumeShroom
        {

        }
        else if (damage >= 998) // Squash, PotatoMine, CherryBoom, DoomShroom
        {

        }
        else
        {
            if (zName == "BucketZombie")
            {
                if (MetalObject != null)
                {
                    if (MetalObject.name == "Bucket(Clone)")
                    {
                        source.PlayOneShot(type.hitClips[Random.Range(0, 1)]);
                    }
                }
                else
                {
                    source.PlayOneShot(type.hitClips[Random.Range(2, type.hitClips.Length)]);
                }
            }
            else if (zName == "ConeZombie")
            {
                if (NormalObject != null)
                {
                    if (NormalObject.name == "Cone(Clone)")
                    {
                        source.PlayOneShot(type.hitClips[Random.Range(0, 1)]);
                    }
                }
                else
                {
                    source.PlayOneShot(type.hitClips[Random.Range(2, type.hitClips.Length)]);
                }
            }
            else //if (zName == "ClassicZombie")
            {
                source.PlayOneShot(type.hitClips[Random.Range(0, type.hitClips.Length)]);
            }
        }

        if (MetalObject != null)
        {
            if (MetalObject.name == "Bucket(Clone)")
            {
                if(damage >= MetalObject.GetComponent<Bucket>().durability)
                {
                    MetalObject.GetComponent<Bucket>().durability -= damage;
                    health -= (damage - MetalObject.GetComponent<Bucket>().durability);
                }
                else
                {
                    MetalObject.GetComponent<Bucket>().durability -= damage;
                }
            }
        }
        else if (NormalObject != null)
        {
            if (NormalObject.name == "Cone(Clone)")
            {
                if (damage >= NormalObject.GetComponent<Cone>().durability)
                {
                    NormalObject.GetComponent<Cone>().durability -= damage;
                    health -= (damage - NormalObject.GetComponent<Cone>().durability);
                }
                else
                {
                    NormalObject.GetComponent<Cone>().durability -= damage;
                }
            }
        }
        else
        {
            health -= damage;
        }

        if (freeze)
        {
            Freeze(damage);
        }

        if (beCold)
        {
            BeCold();
        }

        if (flame)
        {
            Burn();
        }


        if (health <= 0)
        {
            speed = 0;
            if (ZombieInPool)
            {
                if (readyAnimator)
                {
                    if (animator.GetInteger("Status") == 9 || animator.GetInteger("Status") == 13) //In Pool
                    {
                        animator.SetInteger("Status", 12);
                    }
                    else if (animator.GetInteger("Status") == 10) // Eat In Pool
                    {
                        animator.SetInteger("Status", 11);
                    }
                }
            }
            else
            {
                if (readyAnimator)
                {
                    if (animator.GetInteger("Status") == 1 || animator.GetInteger("Status") == 3)
                    {
                        if (damage == 1000)
                        {
                            animator.SetInteger("Status", 6);
                        }
                        else
                        {
                            animator.SetInteger("Status", 5);
                        }
                    }
                    else if (animator.GetInteger("Status") == 2)
                    {
                        if (damage == 1000)
                        {
                            animator.SetInteger("Status", 7);
                        }
                        else
                        {
                            animator.SetInteger("Status", 4);
                        }
                    }
                }
            }


            dead = true;

            if (damage == 1000)
            {
                Destroy(gameObject, 2);
            }
            else if (damage == 999)
            {
                Destroy(gameObject, 1);
            }
            else if (damage == 998)
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                Destroy(gameObject, 3);
            }
            else if (damage == 997)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, 1);
            }
        }
        else if (health <= max_health / 2)
        {
            if (readyAnimator)
            {

                if (!hasLeftHand)
                {

                }
                else
                {

                    hasLeftHand = false;
                    zombieLeftUpperHandPosition = gameObject.transform.GetChild(3).transform.GetChild(0).transform.position;
                    butterOnHead = Instantiate(LeftHandObject, zombieLeftUpperHandPosition, Quaternion.identity);
                    //Debug.Log("Left Hand dropped");
                    Destroy(gameObject.transform.GetChild(3).transform.GetChild(0).gameObject);
                    //Debug.Log("Upper Hand1 is deactive");
                    Destroy(gameObject.transform.GetChild(3).transform.GetChild(1).gameObject);
                    //Debug.Log("Lower Hand1 is deactive");
                    Destroy(gameObject.transform.GetChild(3).transform.GetChild(2).gameObject);
                    //Debug.Log("Hand1 is deactive");
                    Destroy(gameObject.transform.GetChild(3).transform.GetChild(3).gameObject);
                }
            }
        }

        if (dead)
        {
            GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().zombiesDie++;
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            return;
        }
    }

    void Burn()
    {
        CancelInvoke("Unburn");
        GetComponent<SpriteRenderer>().color = Color.red;
        speed = type.speed / 2;
        Invoke("Unburn", 5);
    }

    void Unburn()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = type.speed;
    }

    void BeCold()
    {
        CancelInvoke("Warming");
        GetComponent<SpriteRenderer>().color = Color.blue;
        speed = type.speed / 2;
        Invoke("Warming", 5);
    }

    void Warming()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = type.speed;
    }

    void Freeze(int damage)
    {
        isFreeze = true;
        canEat = false;
        if(damage == 4)
        {
            if (readyAnimator)
            {
                zombieHairPosition = gameObject.transform.GetChild(0).transform.GetChild(2).transform.position;
                butterOnHead = Instantiate(ButterObject, zombieHairPosition, Quaternion.identity);
            }
        }
        else if(damage == 0)
        {
            if(ZombieInPool)
            {
                if (readyAnimator)
                {
                    zombieHipPosition = gameObject.transform.GetChild(7).transform.GetChild(0).transform.position;
                    iceOnFoot = Instantiate(IceObject, zombieHipPosition, Quaternion.identity);
                }
            }
            else
                {
                if (readyAnimator)
                {
                    zombieFootPosition = gameObject.transform.GetChild(5).transform.GetChild(2).transform.position;
                    iceOnFoot = Instantiate(IceObject, zombieFootPosition, Quaternion.identity);
                }
            }
        }
        CancelInvoke("Unfreeze");
        GetComponent<SpriteRenderer>().color = Color.cyan;
        speed = 0;
        Invoke("Unfreeze", 5);
    }

    void Unfreeze()
    {
        isFreeze = false;
        canEat = true;
        if(butterOnHead != null)
        {
            Destroy(butterOnHead);
        }
        else if(iceOnFoot != null)
        {
            Destroy(iceOnFoot);
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = type.speed;
    }
}
