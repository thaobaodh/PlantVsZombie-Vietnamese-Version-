using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tiger : MonoBehaviour
{
    public string lane;

    public int damage;

    public float speed = 0.8f;

    private bool shooted = false;

    public GameObject target;

    public float launchHeight = 1;

    public Vector3 movePosition;

    private bool goBack = false;

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
        Debug.Log("Lane: " + lane + " / " + target.name);
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerX = transform.position.x;
        playerY = transform.position.y;
        Destroy(gameObject, 10);
    }

    [System.Obsolete]
    private void Update()
    {
        if (!shooted)
        {
            if (target != null)
            {
                targetX = target.transform.position.x;
                targetY = target.transform.position.y;
                // Debug.Log("Cabbage: " + playerX + " " + playerY + " ---  " + targetX + " " + targetY);
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
                    }
                    else
                    {
                        // Debug.Log("Not in range: " + Mathf.Abs(playerY - targetY));
                    }
                }
                else
                {
                    dist = targetX - playerX;
                    nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
                    baseY = Mathf.Lerp(playerY, target.transform.position.y, (nextX - playerX) / dist);
                    height = launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
                    movePosition = new Vector3(nextX, baseY + height, transform.position.z);
                    transform.rotation = LookAtTarget(movePosition - transform.position);
                    transform.position = movePosition;
                }
            }
            else
            {

                targetX = playerX;
                targetY = playerY;
                playerX = transform.position.x;
                playerY = transform.position.y;
            }
        }
        else
        {

        }

        if (goBack)
        {
            dist = targetX - playerX;
            nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
            baseY = Mathf.Lerp(playerY, targetY, (nextX - playerX) / dist);
            height = launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
            movePosition = new Vector3(nextX, baseY + height, transform.position.z);
            transform.rotation = WayBackHome(movePosition - transform.position);
            if (ContainsNaN(transform.position) && ContainsNaN(movePosition))
            {
                Destroy(gameObject, 0.25f);
            }
            else
            {
                transform.position = movePosition;
            }

            // Debug.Log(transform.position + " / " + targetX + ", " + targetY);
            if (Mathf.Abs(transform.position.x - targetX) <= 0.2f)
            {
                Destroy(gameObject, 0.25f);
            }
        }
    }

    public static bool ContainsNaN(Vector3 vector)
    {
        return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
    }

    public static Quaternion WayBackHome(Vector2 r)
    {
        return Quaternion.Euler(0, 180, 0);
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
                zombie.Hit(damage, false, false, false);
                shooted = true;

                targetX = playerX;
                targetY = playerY;
                playerX = transform.position.x;
                playerY = transform.position.y;
                goBack = true;
            }
            else
            {
                shooted = true;
                targetX = playerX;
                targetY = playerY;
                playerX = transform.position.x;
                playerY = transform.position.y;
                goBack = true;
            }
        }
        else if (other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
        {
            balloonZombie.Hit(damage);
            shooted = true;

            targetX = playerX;
            targetY = playerY;
            playerX = transform.position.x;
            playerY = transform.position.y;
            goBack = true;
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            tinyZombieBoat.Hit(damage);
            shooted = true;

            targetX = playerX;
            targetY = playerY;
            playerX = transform.position.x;
            playerY = transform.position.y;
            goBack = true;
        }
        //else if(other.TryGetComponent<TigerGrass>(out  TigerGrass tigerGrass))
        //{
        //    tigerGrass.isReturn = true;
        //}
    }
}
