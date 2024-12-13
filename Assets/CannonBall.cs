using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CannonBall : MonoBehaviour
{
    public bool isBone;

    public int damage;

    public float speed = 0.8f;

    private bool shooted = false;

    public Vector3 target;

    public float launchHeight = 1;

    public Vector3 movePosition;

    public GameObject skeletonZombie;

    [SerializeField] bool isPlantSide;

    public bool targetLock;

    private float playerX;
    private float playerY;
    private float targetX;
    private float targetY;
    private float nextX;
    private float dist;
    private float baseY;
    private float height;

    private GameManager gms;


    private void Start()
    {
        // Debug.Log("Cannon Target:" + target);
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerX = transform.position.x;
        playerY = transform.position.y;
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        if (!shooted)
        {
            if (target != null)
            {
                targetX = target.x;
                targetY = target.y;

                dist = targetX - playerX;
                nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
                baseY = Mathf.Lerp(playerY, target.y, (nextX - playerX) / dist);
                height = launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
                movePosition = new Vector3(nextX, baseY + height, transform.position.z);
                transform.rotation = LookAtTarget(movePosition - transform.position);
                transform.position = movePosition;
                if(isBone)
                {
                    if (transform.position == target)
                    {
                        Instantiate(skeletonZombie, transform.position, Quaternion.identity);
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                Destroy(gameObject, 0.5f);
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
        if (isBone)
        {

        }
        else
        {
            if (targetLock)
            {
                if (other.TryGetComponent<Plant>(out Plant plant))
                {
                    if (plant.transform.position == target)
                    {
                        plant.Hit(damage, 1);
                        shooted = true;
                        Destroy(gameObject, 0.1f);
                    }
                }
            }
            else
            {
                if (isPlantSide)
                {
                    if (other.TryGetComponent<ZombieBoat>(out ZombieBoat zombieBoat))
                    {
                        Debug.Log("TinyCannonBall hit ZombieBoat");
                        zombieBoat.Hit(damage);
                        // shooted = true;
                        Destroy(gameObject, 0.1f);
                    }
                }
                else
                {
                    if (other.TryGetComponent<Plant>(out Plant plant))
                    {
                        plant.Hit(damage, 1);
                        // shooted = true;
                        Destroy(gameObject, 0.1f);
                    }
                }
            }
        }
    }
}
