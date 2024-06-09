using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bee : MonoBehaviour
{
    public Transform HomePosition;
    public int damage;
    public float speed = 0.8f;

    private bool shooted = false;

    private GameObject target;
    private GameObject[] targets;

    public float launchHeight = 1;
    public Vector3 movePosition;
    private float playerX;
    private float playerY;
    //private float targetX;
    //private float targetY;
    //private float nextX;
    //private float dist;
    //private float baseY;
    //private float height;

    [SerializeField] float timeToAlive;

    private float cooldown;

    private void Start()
    {
        playerX = transform.position.x;
        playerY = transform.position.y;
        targets = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject _target in targets)
        {
            if (Mathf.Abs(playerX - _target.transform.position.x) <= 10f)
            {
                target = _target;
                return;
            }
        }
    }
    private void Update()
    {

        cooldown += Time.deltaTime;
        if(cooldown >= timeToAlive)
        {
            cooldown = 0;
            Destroy(gameObject, 0.5f);
        }

        if (!shooted)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                playerX = transform.position.x;
                playerY = transform.position.y;
                targets = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (GameObject _target in targets)
                {
                    if (Mathf.Abs(playerX - _target.transform.position.x) <= 10f)
                    {
                        target = _target;
                        return;
                    }
                }

                if(target == null)
                {
                    if(HomePosition != null)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, HomePosition.position, speed * Time.deltaTime);
                    }
                }
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
            zombie.Hit(damage, false, false, false);
            shooted = true;
            this.transform.GetComponent<Collider2D>().isTrigger = false;
            Invoke("ResetCooldown", 2);
        }
        else if (other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
        {
            balloonZombie.Hit(damage);
            shooted = true;
            this.transform.GetComponent<Collider2D>().isTrigger = false;
            Invoke("ResetCooldown", 2);
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            tinyZombieBoat.Hit(damage);
            shooted = true;
            this.transform.GetComponent<Collider2D>().isTrigger = false;
            Invoke("ResetCooldown", 2);
        }
    }

    private void ResetCooldown()
    {
        shooted = false;
        this.transform.GetComponent<Collider2D>().isTrigger = true;
    }
}
