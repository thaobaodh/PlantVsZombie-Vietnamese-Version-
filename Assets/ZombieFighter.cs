using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFighter : MonoBehaviour
{
    public string lane;

    public GameObject bullet;

    public int numOfBullet;

    public Transform shootOrigin;

    [SerializeField] Animator animator;

    public float cooldown;

    public float reloadCooldown;

    private bool canShoot = false;

    public int health;

    public float speed;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    private GameManager gms;

    private int countBullet = 0;

    public bool dead = false;

    private float[] raycastPositionY = { 3.49f, 1.71f, 0.17f, -1.28f, -2.91f };

    private void Start()
    {
        animator.Play("Idle");
        canShoot = true; 
        dead = false;
        countBullet = 0;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
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

    void ResetCooldown()
    {
        canShoot = true;
    }
    void Reload()
    {
        canShoot = true;
        countBullet = 0;
    }

    void Shoot(GameObject zombieTarget)
    {
        if (!canShoot)
            return;
        animator.Play("Shooting");
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canShoot = false;
        countBullet++;
        if(countBullet >= numOfBullet)
        {
            animator.Play("Idle");
            Invoke(nameof(Reload), reloadCooldown);
        }
        else
        {
            GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity); 
            Invoke(nameof(ResetCooldown), cooldown);
        }
    }


    void Hit(int damage)
    {
        if (dead) return;
        if (health > 0)
        {
            health -= damage;
            if(health <= 0)
            {
                dead = true;
                Destroy(gameObject, 1f);
            }
        }
    }
}
