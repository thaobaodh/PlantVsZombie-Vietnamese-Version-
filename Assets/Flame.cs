using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Flame : MonoBehaviour
{
    public int damage;

    public bool isHealth = false;

    public float speed = 0.8f;

    public bool shooted = false;

    public Vector3 target;

    public float launchHeight = 1;

    public Vector3 movePosition;

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

    [SerializeField] public bool isNormalFlame;

    [SerializeField] public Animator animator;

    [SerializeField] public Vector3 targetPoint;

    private void Start()
    {
        // Debug.Log("Cannon Target:" + target);
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerX = transform.position.x;
        playerY = transform.position.y;
        animator.Play("Idle");
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        if (!shooted)
        {
            if (target != null)
            {
                if(isHealth)
                {
                    var step = speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
                }
                else
                {
                    if (isNormalFlame)
                    {
                        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                    }
                    else
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
        if (isHealth)
        {
            Destroy(gameObject, 5f);
        }
        else
        {
            if (other.TryGetComponent<Plant>(out Plant plant))
            {
                Debug.Log("Burning flame: " + other.name);
                if (targetLock)
                {
                    if (plant.transform.position == target)
                    {
                        animator.Play("BigFlame");
                        plant.Hit(damage, 1);
                    }
                }
                else
                {
                    if (isNormalFlame)
                    {
                        animator.Play("Idle");
                    }
                    else
                    {
                        animator.Play("BigFlame");
                    }
                    plant.Hit(damage, 1);
                }
            }
        }
    }
}
