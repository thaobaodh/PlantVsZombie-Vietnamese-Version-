using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BronzeGong : MonoBehaviour
{
    public bool isBone;

    public int damage = 100;

    public float speed = 0.8f;

    private bool shooted = false;

    public Vector3 target;

    public float launchHeight = 1;

    public Vector3 movePosition;

    public GameObject skeletonZombie;

    [SerializeField] bool isPlantSide;

    [SerializeField] Plant targetPlant;

    [SerializeField] Animator animator;

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
        animator.Play("Idle");
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerX = transform.position.x;
        playerY = transform.position.y;
        Invoke(nameof(ReturnToYellowBrownDemon), 8.75f);
        Destroy(gameObject, 10f);
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
                if (isBone)
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (shooted) return;
        if (targetLock)
        {
            if (transform.position == target)
            {
                if (other.TryGetComponent<Plant>(out Plant plant))
                {
                    Debug.Log("Gooooo");
                    targetPlant = plant;
                    shooted = true;
                    Invoke(nameof(TurnPlantToWater), 3f);
                    animator.Play("Feng");
                }
            }
        }
    }

    private void TurnPlantToWater()
    {
        Debug.Log("Turn Plant To Water");
        targetPlant.Hit(damage, 1);
        Invoke(nameof(ReturnToYellowBrownDemon), 1f);
    }
    private void ReturnToYellowBrownDemon()
    {
        Debug.Log("Return To Yellow Brown Demon");
        animator.Play("ReturnToYellowBrown");
        Destroy(gameObject, 1.2f);
    }
}
