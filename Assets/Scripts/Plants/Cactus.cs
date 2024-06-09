using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    public GameObject bullet;

    public Transform shootOrigin;

    public Transform highShootOrigin;

    public float cooldown;

    private bool canShoot;

    public float range;

    public LayerMask shootMask;

    public LayerMask flyShootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    [SerializeField] Animator animator;

    private bool isGround = true;

    private bool flyZombie = false;

    private GameManager gms;

    private float[] raycastPositionY = { 3.49f, 1.71f, 0.17f, -1.28f, -2.91f };

    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator.SetInteger("Detect", 0);
        source = gameObject.AddComponent<AudioSource>();
        Invoke("ResetCooldown", 1);
    }

    private void Update()
    {
        if (!canShoot)
            return;

        if (gms.Roof)
        {
            if (transform.position.x < 2.7)
            {
                RaycastHit2D FlyHit = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, flyShootMask);
                if (FlyHit.collider)
                {
                    flyZombie = true;
                    if (animator.GetInteger("Detect") == 0 || animator.GetInteger("Detect") == 4)
                    {
                        animator.SetInteger("Detect", 1);
                        Invoke("UpIdle", 1);
                    }
                    else
                    {

                    }

                    target = FlyHit.collider.gameObject;
                    HighShoot();
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, shootMask);
                    if (hit.collider)
                    {
                        flyZombie = false;
                        if (animator.GetInteger("Detect") == 2)
                        {
                            Invoke("Down", 1);
                        }
                        else
                        {

                        }
                        target = hit.collider.gameObject;
                        Shoot();
                    }
                }
            }
            else
            {
                RaycastHit2D FlyHit = Physics2D.Raycast(transform.position, Vector2.right, range, flyShootMask);
                if (FlyHit.collider)
                {
                    flyZombie = true;
                    if (animator.GetInteger("Detect") == 0 || animator.GetInteger("Detect") == 4)
                    {
                        animator.SetInteger("Detect", 1);
                        Invoke("UpIdle", 1);
                    }
                    else
                    {

                    }

                    target = FlyHit.collider.gameObject;
                    HighShoot();
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                    if (hit.collider)
                    {
                        flyZombie = false;
                        if (animator.GetInteger("Detect") == 2)
                        {
                            Invoke("Down", 1);
                        }
                        else
                        {

                        }
                        target = hit.collider.gameObject;
                        Shoot();
                    }
                }
            }
        }
        else
        {
            RaycastHit2D FlyHit = Physics2D.Raycast(transform.position, Vector2.right, range, flyShootMask);
            if (FlyHit.collider)
            {
                flyZombie = true;
                if (animator.GetInteger("Detect") == 0 || animator.GetInteger("Detect") == 4)
                {
                    animator.SetInteger("Detect", 1);
                    Invoke("UpIdle", 1);
                }
                else
                {

                }

                target = FlyHit.collider.gameObject;
                HighShoot();
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                if (hit.collider)
                {
                    flyZombie = false;
                    if (animator.GetInteger("Detect") == 2)
                    {
                        Invoke("Down", 1);
                    }
                    else
                    {

                    }
                    target = hit.collider.gameObject;
                    Shoot();
                }
            }
        }
    }

    void UpIdle()
    {
        isGround = false;
        animator.SetInteger("Detect", 2);
        if(flyZombie)
        {
            return;
        }
        Invoke("Down", 1);
    }
    void Down()
    {
        animator.SetInteger("Detect", 3);
        Invoke("Ground", 1);
    }

    void Ground()
    {
        isGround = true;
        animator.SetInteger("Detect", 4);
    }
    void ResetCooldown()
    {
        canShoot = true;
    }

    void Shoot()
    {
        if (!canShoot)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);

        canShoot = false;

        GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);

        Invoke("ResetCooldown", cooldown);
    }


    void HighShoot()
    {

        if (!canShoot)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);

        canShoot = false;

        GameObject myBullet = Instantiate(bullet, highShootOrigin.position, Quaternion.identity);

        Invoke("ResetCooldown", cooldown);
    }
}
