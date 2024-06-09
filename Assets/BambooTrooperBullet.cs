using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BambooTrooperBullet : MonoBehaviour
{
    [SerializeField] bool isSuperBullet;
    public string lane;
    public int damage;
    public float speed = 0.8f;
    private bool shooted = false;
    [SerializeField] float range;
    [SerializeField] int numOfZombieAffected;
    [SerializeField] float distanceToDestroy;
    private bool isUpgrade = false;

    [SerializeField] Animator animator;

    public GameObject target;

    public float launchHeight = 1;
    public Vector3 movePosition;
    private float playerX;
    private float playerY;
    private float targetX;
    private float targetY;
    private float nextX;
    private float dist;
    private float baseY;
    private float height;

    private GameManager gms;

    private int count = 0;
    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerX = transform.position.x;
        playerY = transform.position.y;
        if (isSuperBullet) return;
        if(Random.Range(0, 10) % 5 == 0)
        {
            animator.SetBool("Upgrade", true);
            isUpgrade = true;
        }
        else
        {
            animator.SetBool("Upgrade", false);
            isUpgrade = false;
        }
    }
    private void Update()
    {
        if (!shooted)
        {
            if (target != null)
            {
                if (isSuperBullet)
                {
                    targetX = target.transform.position.x;
                    targetY = target.transform.position.y;
                    // Debug.Log("Corn: " + playerX + " " + playerY + " ---  " + targetX + " " + targetY);
                    if (gms.Roof)
                    {
                        if (Mathf.Abs(playerY - targetY) <= 1.58f)
                        {
                            // Debug.Log("In range: " + Mathf.Abs(playerY - targetY));
                            dist = targetX - playerX;
                            nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
                            baseY = Mathf.Lerp(playerY, target.transform.position.y, (nextX - playerX) / dist);
                            height = launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
                            movePosition = new Vector3(nextX, baseY + height, transform.position.z);
                            transform.rotation = LookAtTarget(movePosition - transform.position);
                            transform.position = movePosition;
                            /*
                            if (movePosition == target.transform.position)
                            {
                                Destroy(gameObject);
                            }
                            */
                        }
                        else
                        {
                            // Debug.Log("Not in range: " + Mathf.Abs(playerY - targetY));
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(playerY - targetY) <= 0.5f)
                        {
                            // Debug.Log("In range: " + Mathf.Abs(playerY - targetY));
                            dist = targetX - playerX;
                            nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
                            baseY = Mathf.Lerp(playerY, target.transform.position.y, (nextX - playerX) / dist);
                            height = launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
                            movePosition = new Vector3(nextX, baseY + height, transform.position.z);
                            transform.rotation = LookAtTarget(movePosition - transform.position);
                            transform.position = movePosition;
                            /*
                            if (movePosition == target.transform.position)
                            {
                                Destroy(gameObject);
                            }
                            */
                        }
                        else
                        {
                            // Debug.Log("Not in range: " + Mathf.Abs(playerY - targetY));
                        }
                    }
                }
                else
                {
                    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                }
            }
            else
            {

                if (isSuperBullet)
                {
                    if (Mathf.Abs(transform.position.y - targetY) >= 0.15f)
                    {
                        dist = targetX - playerX;
                        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
                        baseY = Mathf.Lerp(playerY, targetY, (nextX - playerX) / dist);
                        height = launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
                        movePosition = new Vector3(nextX, baseY + height, transform.position.z);
                        transform.rotation = LookAtTarget(movePosition - transform.position);
                        transform.position = movePosition;
                    }
                    else
                    {
                        animator.SetBool("Upgrade", false);
                    }
                }
                else
                {
                    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                }
            }

            if (Vector3.Distance(transform.position, new Vector3(playerX, playerY, 0)) >= distanceToDestroy)
            {
                Destroy(gameObject);
            }
        }
        else
        {

        }
    }

    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            if (zombie.lane == lane)
            {
                if (isSuperBullet)
                {
                    Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                    foreach (Collider2D collider2D in colliderArray)
                    {
                        if (collider2D.TryGetComponent<Zombie>(out Zombie zombie2))
                        {
                            zombie2.Hit(1000, false, false, false);
                        }
                        else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                        {
                            balloonZombie.Hit(1000);
                        }
                        else if (collider2D.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
                        {
                            tinyZombieBoat.Hit(1000);
                        }
                    }
                    shooted = true;
                    Destroy(gameObject);
                }
                else
                {
                    numOfZombieAffected++;
                    zombie.Hit(damage, false, false, false);
                    if (count >= numOfZombieAffected)
                    {
                        shooted = true;
                        count = 0;
                        Destroy(gameObject);
                    }
                }
            }
            else
            {

            }
        }
        else if (other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
        {
            balloonZombie.Hit(damage);
            shooted = true;
            if (balloonZombie.dead)
            {
                Destroy(gameObject, 0.5f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat1))
        {
            tinyZombieBoat1.Hit(damage);
            shooted = true;
            if (tinyZombieBoat1.dead)
            {
                Destroy(gameObject, 0.5f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isUpgrade)
        {
            return;
        }
        else
        {
            if (other.TryGetComponent<Zombie>(out Zombie zombie))
            {
                if (zombie.lane == lane)
                {
                    if (isSuperBullet)
                    {

                    }
                    else
                    {
                        zombie.transform.position = transform.position;
                    }
                }
            }
        }
    }
}
