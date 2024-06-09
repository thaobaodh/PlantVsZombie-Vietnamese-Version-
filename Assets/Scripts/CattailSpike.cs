using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CattailSpike : MonoBehaviour
{
    public int damage;
    public float speed = 0.8f;

    private bool shooted = false;

    private GameObject target;
    private GameObject[] targets;

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
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        if (!shooted)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
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
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            tinyZombieBoat.Hit(damage);
            shooted = true;
            Destroy(gameObject, 0.5f);
        }
    }
}
