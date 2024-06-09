using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Wintermelon : MonoBehaviour
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
            if (Mathf.Abs(playerY - _target.transform.position.y) <= 0.5f)
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
                targetX = target.transform.position.x;
                targetY = target.transform.position.y;
                // Debug.Log("Cabbage: " + playerX + " " + playerY + " ---  " + targetX + " " + targetY);
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
                    if (movePosition == target.transform.position)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    // Debug.Log("Not in range: " + Mathf.Abs(playerY - targetY));
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
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            zombie.Hit(damage, true, false, false);
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
