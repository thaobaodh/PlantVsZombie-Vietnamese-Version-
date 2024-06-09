using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A100StacksBamboo : MonoBehaviour
{
    public string lane;

    public GameObject bullet;

    public Transform stickZombieOrigin;

    public Transform shootOrigin;

    public float cooldown;

    private bool canShoot = false;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    private GameManager gms;

    private float[] raycastPositionY = { 3.49f, 1.71f, 0.17f, -1.28f, -2.91f };

    public bool isBlessed = false;

    [SerializeField] Animator animator;

    [SerializeField] float timeout = 30f;

    [SerializeField] float countTime = 0;

    [SerializeField] int damage = 10;
    private void Start()
    {
        isBlessed = false;
        canShoot = true;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {

        if (isBlessed)
        {
            if (!animator.GetBool("Blessed"))
            {
                animator.SetBool("Blessed", true);
            }

            countTime += Time.deltaTime;
            if (countTime >= timeout)
            {
                countTime = 0;
                animator.SetBool("Blessed", false);
                isBlessed = false;
                canShoot = false;
            }


            if (!canShoot)
                return;

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, 3);
            foreach (Collider2D collider2D in colliderArray)
            {
                if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
                {
                    zombie.transform.position = stickZombieOrigin.position;
                    zombie.Hit(damage, false, false, true);
                    Invoke(nameof(ResetCooldown), 1);
                    canShoot = false;
                }
                else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                {
                    balloonZombie.transform.position = stickZombieOrigin.position;
                    balloonZombie.Hit(damage);
                    Invoke(nameof(ResetCooldown), 1);
                    canShoot = false;
                }
            }

        }
        else
        {
            if (!canShoot)
                return;
            if (animator.GetBool("Blessed"))
            {
                animator.SetBool("Blessed", false);
            }

            if (gms.Roof)
            {
                if (gameObject.name == "MelonPult(Clone)" || gameObject.name == "WinterMelon(Clone)")
                {
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, raycastPositionY[int.Parse(lane) - 1]), Vector2.right, range, shootMask);
                    if (hit.collider)
                    {
                        target = hit.collider.gameObject;
                        //Debug.Log("Hit " + hit.collider.gameObject.transform.position);
                        Shoot(target);
                    }
                }
                else
                { // Still error -> Change the range with position
                    if (transform.position.x < 2.7)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, shootMask);
                        if (hit.collider)
                        {
                            Debug.Log("Detect Zombie in Short range");
                            target = hit.collider.gameObject;
                            Shoot(target);
                        }
                    }
                    else
                    {
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                        if (hit.collider)
                        {
                            Debug.Log("Detect Zombie in Long range " + range);
                            target = hit.collider.gameObject;
                            Shoot(target);
                        }
                    }
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                if (hit.collider)
                {
                    target = hit.collider.gameObject;
                    Shoot(target);
                }
            }
        }
    }

    void ResetCooldown()
    {
        canShoot = true;
    }

    void StickZombie(GameObject zombieTarget)
    {
        if (!canShoot)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canShoot = false;
        zombieTarget.transform.position = stickZombieOrigin.position;

        Invoke(nameof(ResetCooldown), cooldown);
    }

    void Shoot(GameObject zombieTarget)
    {
        if (!canShoot)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canShoot = false;

        GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);

        Invoke(nameof(ResetCooldown), cooldown);
    }
}
