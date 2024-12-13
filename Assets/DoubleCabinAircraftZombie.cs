using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCabinAircraftZombie : MonoBehaviour
{
    public string lane;

    public GameObject bullet;

    public GameObject poisonGas;

    public int numOfBullet;

    public int numOfPoisonGas;

    public Transform gasPosition;

    public Transform[] shootOrigins;

    [SerializeField] Animator animator;

    public float cooldown;

    public float reloadCooldown;

    private bool canReleasePoisonGas = false;

    private bool canShoot = false;

    public bool breakAnHalf = false;

    public int health;

    public int max_health;

    public float speed;

    public float gasSpeed;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    private GameManager gms;

    private int countBullet = 0;

    public bool dead = false;

    private float[] raycastPositionY = { 3.49f, 1.71f, 0.17f, -1.28f, -2.91f };

    [SerializeField] bool releaseAPoisonGas = false;

    private void Start()
    {
        breakAnHalf = false;
        max_health = health;
        animator.Play("Idle");
        canShoot = true;
        canReleasePoisonGas = true;
        dead = false;
        countBullet = 0;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();;

        if (Random.Range(0, 5) % 2 == 0)
        {
            releaseAPoisonGas = true;
        }
        else
        {
            releaseAPoisonGas = false;
        }
    }

    private void Update()
    {
        if(releaseAPoisonGas)
        {
            if(!canReleasePoisonGas)
            {
                transform.position -= new Vector3(gasSpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
                if (hit.collider)
                {
                    ReleaseAPoisonGasFunction();
                }
                else
                {
                    transform.position -= new Vector3(gasSpeed * Time.deltaTime, 0f, 0f);
                }
            }
        }
        else
        {
            if (!canShoot)
                return;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                Shoot(target);
            }
            else
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
            }
        }

    }

    void ResetCooldown()
    {
        canShoot = true;
    }
    void Reload()
    {
        canShoot = true;
        countBullet = 0;
    }

    void ReleaseAPoisonGasFunction()
    {
        if (!canReleasePoisonGas)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canReleasePoisonGas = false;

        GameObject myPoisonGas = Instantiate(poisonGas, gasPosition.position, Quaternion.identity);
        Invoke(nameof(ResetCooldown), reloadCooldown);
    }

    void Shoot(GameObject zombieTarget)
    {
        if (!canShoot)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canShoot = false;
        countBullet++;
        if (countBullet >= numOfBullet)
        {
            if (!breakAnHalf)
            {
                animator.Play("Idle");
            }
            else
            {
                animator.Play("IdleBreakAnHalf");
            }
            Invoke(nameof(Reload), reloadCooldown);
        }
        else
        {
            if(!breakAnHalf)
            {
                animator.Play("Shooting");
                GameObject myBullet1 = Instantiate(bullet, shootOrigins[0].position, Quaternion.identity);
                GameObject myBullet2 = Instantiate(bullet, shootOrigins[1].position, Quaternion.identity);
            }
            else
            {
                animator.Play("IdleBreakAnHalf");
                GameObject myBullet1 = Instantiate(bullet, shootOrigins[0].position, Quaternion.identity);
            }
            Invoke(nameof(ResetCooldown), cooldown);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "DeathTrigger")
        {
            Destroy(gameObject, 2f);
        }
    }

    void Hit(int damage)
    {
        if (dead) return;
        if (health > 0)
        {
            health -= damage;

            if(health > 0 && health <= max_health)
            {
                animator.Play("BreakAnHalf");
                breakAnHalf = true;
            }
            else
            {
                breakAnHalf = false;
            }

            if (health <= 0)
            {
                dead = true;
                Destroy(gameObject, 1f);
            }
        }
    }
}
