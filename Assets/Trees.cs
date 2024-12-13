using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Trees : MonoBehaviour
{
    [SerializeField] Sprite[] listOfThings;

    public int damage;

    public float speed = 0.8f;

    private bool shooted = false;

    public float launchHeight = 1;

    public Vector3 movePosition;

    public Vector3 destinationPosition;

    public GameObject target;

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
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        GetComponent<SpriteRenderer>().sprite = listOfThings[Random.Range(0, listOfThings.Length)];
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
                targetX = target.transform.position.x;
                targetY = target.transform.position.y;

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
                targetX = destinationPosition.x;
                targetY = destinationPosition.y;
                
                dist = targetX - playerX;
                nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
                baseY = Mathf.Lerp(playerY, targetY, (nextX - playerX) / dist);
                height = launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
                movePosition = new Vector3(nextX, baseY + height, transform.position.z);
                transform.rotation = LookAtTarget(movePosition - transform.position);
                transform.position = movePosition;
            }
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
            zombie.Hit(damage, false, false, false);
            shooted = true;
            Destroy(gameObject, 0.5f);
        }
        else if (other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
        {
            balloonZombie.Hit(damage);
            shooted = true;
            Destroy(gameObject, 0.5f);
        }
    }
}
