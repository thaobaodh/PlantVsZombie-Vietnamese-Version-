using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaredShroom : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] GameObject bullet;

    [SerializeField] Transform shootOrigin;

    [SerializeField] float cooldown;

    private bool canShoot;

    private float rangeToScared = 3f;

    private bool scared = false;

    private float braveCooldown = 0;

    [SerializeField] float range;

    [SerializeField] LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    [SerializeField] AudioClip[] shootClips;

    private GameManager gms;

    private float[] raycastPositionY = { 3.49f, 1.71f, 0.17f, -1.28f, -2.91f };

    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
        InvokeRepeating(nameof(ResetCooldown), 1, cooldown);
    }

    private void FixedUpdate()
    {
        if (!canShoot)
            return;
        CheckPosition();
    }


    void CheckPosition()
    {
        if (gms.Roof)
        {
            if (transform.position.x < 2.7)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                if (hit.collider)
                {
                    target = hit.collider.gameObject;
                    float dist = Vector3.Distance(target.transform.position, transform.position);
                    // Debug.Log("Distance to other: " + dist);
                    if (dist > rangeToScared)
                    {
                        // Debug.Log("Safety: " + dist);
                        if (animator.GetInteger("ZombieInsideRange") != 4)
                        {
                            animator.SetInteger("ZombieInsideRange", 4); //Normal
                        }
                        Shoot();
                    }
                    else if (dist >= rangeToScared - 0.2f && dist <= rangeToScared)
                    {
                        // Debug.Log("Turn to scare: " + dist);
                        if (animator.GetInteger("ZombieInsideRange") != 0)
                        {
                            animator.SetInteger("ZombieInsideRange", 0); //Turn Scare
                        }
                        scared = true;
                    }
                    else if (dist >= 0 && dist < rangeToScared - 0.2f)
                    {
                        // Debug.Log("Scared: " + dist);
                        if (animator.GetInteger("ZombieInsideRange") != 1)
                        {
                            animator.SetInteger("ZombieInsideRange", 1); //Scared
                        }
                        scared = true;
                        Scared();
                    }
                }
                else
                {

                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                if (hit.collider)
                {
                    target = hit.collider.gameObject;
                    float dist = Vector3.Distance(target.transform.position, transform.position);
                    // Debug.Log("Distance to other: " + dist);
                    if (dist > rangeToScared)
                    {
                        // Debug.Log("Safety: " + dist);
                        if (animator.GetInteger("ZombieInsideRange") != 4)
                        {
                            animator.SetInteger("ZombieInsideRange", 4); //Normal
                        }
                        Shoot();
                    }
                    else if (dist >= rangeToScared - 0.2f && dist <= rangeToScared)
                    {
                        // Debug.Log("Turn to scare: " + dist);
                        if (animator.GetInteger("ZombieInsideRange") != 0)
                        {
                            animator.SetInteger("ZombieInsideRange", 0); //Turn Scare
                        }
                        scared = true;
                    }
                    else if (dist >= 0 && dist < rangeToScared - 0.2f)
                    {
                        // Debug.Log("Scared: " + dist);
                        if (animator.GetInteger("ZombieInsideRange") != 1)
                        {
                            animator.SetInteger("ZombieInsideRange", 1); //Scared
                        }
                        scared = true;
                        Scared();
                    }
                }
                else
                {

                }
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                float dist = Vector3.Distance(target.transform.position, transform.position);
                // Debug.Log("Distance to other: " + dist);
                if (dist > rangeToScared)
                {
                    // Debug.Log("Safety: " + dist);
                    if(animator.GetInteger("ZombieInsideRange") != 4)
                    {
                        animator.SetInteger("ZombieInsideRange", 4); //Normal
                    }
                    Shoot();
                }
                else if (dist >= rangeToScared - 0.2f && dist <= rangeToScared)
                {
                    // Debug.Log("Turn to scare: " + dist);
                    if (animator.GetInteger("ZombieInsideRange") != 0)
                    {
                        animator.SetInteger("ZombieInsideRange", 0); //Turn Scare
                    }
                    scared = true;
                }
                else if (dist >= 0 && dist < rangeToScared - 0.2f)
                {
                    // Debug.Log("Scared: " + dist);
                    if (animator.GetInteger("ZombieInsideRange") != 1)
                    {
                        animator.SetInteger("ZombieInsideRange", 1); //Scared
                    }
                    scared = true;
                    Scared();
                }
            }
            else
            {

            }
        }
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
    }

    void Scared()
    {
        CancelInvoke("Unscared");
        Invoke("Unscared", 2);
    }

    void Unscared()
    {
        if (scared)
        {
                Debug.Log("Be brave: " + braveCooldown);
                animator.SetInteger("ZombieInsideRange", 2); //BeBrave

                scared = false;

            Invoke("NormalState", 0.5f);
        }
    }

    void NormalState()
    {
        Debug.Log("Safety Again: " + braveCooldown);
        animator.SetInteger("ZombieInsideRange", 3); //Normal
    }
}
