using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Squash : MonoBehaviour
{
    public string lane;

    private bool canJump;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    private int zombieCount = 0;

    [SerializeField] Animator animator;

    private float playerX;
    private float playerY;
    private float targetX;
    private float targetY;
    private float nextX;
    private float dist;
    private float baseY;
    private float height;

    public float speed = 0.8f;

    public float launchHeight = 1;

    public Vector3 movePosition;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        canJump = true;
        playerX = transform.position.x;
        playerY = transform.position.y;
        zombieCount = 0;
    }

    private void FixedUpdate()
    {
        if(!canJump) { return; }
        RaycastHit2D hitsLeft = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
        if (hitsLeft.collider)
        {
            zombieCount++;
            if (zombieCount == 1)
            {
                source.PlayOneShot(shootClips[Random.Range(0, 1)]);
            }
  
            target = hitsLeft.collider.gameObject;
            targetX = target.gameObject.transform.position.x;
            targetY = target.gameObject.transform.position.y - 1.25f;
            canJump = false;
            ResetLeftCooldown();

        }

        RaycastHit2D hitsRight = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);

        if (hitsRight.collider)
        {
            zombieCount++;
            if (zombieCount == 1)
            {
                source.PlayOneShot(shootClips[Random.Range(0, 1)]);
            }
            target = hitsRight.collider.gameObject;
            //targetX = target.gameObject.transform.GetChild(10).transform.position.x;
            targetX = target.gameObject.transform.position.x;
            targetY = target.gameObject.transform.position.y - 1.25f;
            canJump = false;
            ResetRightCooldown();
        }
    }

    private void Update()
    {
        /*
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left, range, shootMask);
        foreach (RaycastHit2D hit in hitsLeft)
        {
            if (hit.collider)
            {
                zombieCount++;
                if (zombieCount == 1)
                {
                    source.PlayOneShot(shootClips[Random.Range(0, 1)]);
                }
                InvokeRepeating("ResetLeftCooldown", 1, 3);
                target = hit.collider.gameObject;
                target.GetComponent<Zombie>().Hit(998, false, false, false);

                JumpFuction();
            }
        }

        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right, range, shootMask);
        foreach(RaycastHit2D hit in hitsRight)
        {
            if (hit.collider)
            {
                zombieCount++;
                if(zombieCount == 1)
                {
                    source.PlayOneShot(shootClips[Random.Range(0, 1)]);
                }
                InvokeRepeating("ResetRightCooldown", 1, 3);
                target = hit.collider.gameObject;
                target.GetComponent<Zombie>().Hit(998, false, false, false);

                JumpFuction();
            }
        }
        */
        if (target == null) return;

        JumpFuction(target);
    }

    private void JumpFuction(GameObject target)
    {
        dist = targetX - playerX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(playerY, target.transform.position.y, (nextX - playerX) / dist);
        height = launchHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);
        movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        // transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        // Debug.Log(targetX + " - " + playerX);
    }

    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }

    void ResetLeftCooldown()
    {
        // animator.SetInteger("Jump", 1);
        Jump();
    }

    void ResetRightCooldown()
    {
        //animator.SetInteger("Jump", 2);
        Jump();
    }

    void DestroySquash()
    {
        GetComponent<Plant>().Hit(100, 1);
    }

    void Jump()
    {
        if (!canJump)
            return;
        canJump = false;
        source.PlayOneShot(shootClips[2]);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            if (zombie.lane == lane)
            {
                Invoke("DestroySquash", 1f);
                zombie.Hit(996, false, false, false);
            }
        }
        else if (other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
        {
            //if (balloonZombie.lane == lane)
            //{
            Invoke("DestroySquash", 1f);
            balloonZombie.Hit(996);
            //}
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            if (tinyZombieBoat.lane == lane)
            {
                Invoke("DestroySquash", 1f);
                tinyZombieBoat.Hit(20);
            }
        }
    }
}
