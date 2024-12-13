using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cattail : MonoBehaviour
{
    public GameObject bullet;

    public Transform shootOrigin;

    public float cooldown;

    private bool canShoot;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    private GameManager gms;

    private float[] raycastPositionY = { 3.49f, 1.71f, 0.17f, -1.28f, -2.91f };
    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
        InvokeRepeating("ResetCooldown", 1, cooldown);
    }

    private void Update()
    {
        if (!canShoot)
            return;
        if (gms.Roof)
        {
            if (transform.position.x < 2.7)
            {
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, 4);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
                    {
                        Shoot();
                    }
                    else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
                    {
                        Shoot();
                    }
                    else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                    {
                        Shoot();
                    }
                }
            }
        }
        else
        {
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D collider2D in colliderArray)
            {
                if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
                {
                    Shoot();
                }
                else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                {
                    Shoot();
                }
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
}
