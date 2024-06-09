using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class StarFruit : MonoBehaviour
{
    [SerializeField]  GameObject starBullet;

    [SerializeField]  GameObject starBullet1;

    [SerializeField]  GameObject starBullet2;

    [SerializeField]  GameObject starBullet3;

    [SerializeField]  GameObject starBullet4;

    [SerializeField]  Transform shootOrigin;

    [SerializeField]  Transform shootOrigin1;

    [SerializeField]  Transform shootOrigin2;

    [SerializeField]  Transform shootOrigin3;

    [SerializeField]  Transform shootOrigin4;

    [SerializeField]  float cooldown;

    private bool canShoot;

    [SerializeField]  float range;

    [SerializeField]  LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    [SerializeField]  AudioClip[] shootClips;

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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1.5f, shootMask);
                if (hit.collider)
                {
                    target = hit.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hit1 = Physics2D.Raycast(transform.position, new Vector2(1, 1 / 3), 1.5f, shootMask);
                if (hit1.collider)
                {
                    target = hit1.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(1, -6 / 5), 1.5f, shootMask);
                if (hit2.collider)
                {
                    target = hit2.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, shootMask);
                if (hit3.collider)
                {
                    target = hit3.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hit4 = Physics2D.Raycast(transform.position, Vector2.left, 1.5f, shootMask);
                if (hit4.collider)
                {
                    target = hit4.collider.gameObject;
                    Shoot();
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, range, shootMask);
                if (hit.collider)
                {
                    target = hit.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hit1 = Physics2D.Raycast(transform.position, new Vector2(1, 1 / 3), range, shootMask);
                if (hit1.collider)
                {
                    target = hit1.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(1, -6 / 5), range, shootMask);
                if (hit2.collider)
                {
                    target = hit2.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, range, shootMask);
                if (hit3.collider)
                {
                    target = hit3.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hit4 = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
                if (hit4.collider)
                {
                    target = hit4.collider.gameObject;
                    Shoot();
                }
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, range, shootMask);
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                Shoot();
            }

            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, new Vector2(1, 1 / 3), range, shootMask);
            if (hit1.collider)
            {
                target = hit1.collider.gameObject;
                Shoot();
            }

            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(1, -6 / 5), range, shootMask);
            if (hit2.collider)
            {
                target = hit2.collider.gameObject;
                Shoot();
            }

            RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, range, shootMask);
            if (hit3.collider)
            {
                target = hit3.collider.gameObject;
                Shoot();
            }

            RaycastHit2D hit4 = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
            if (hit4.collider)
            {
                target = hit4.collider.gameObject;
                Shoot();
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
        GameObject myBullet = Instantiate(starBullet, shootOrigin.position, Quaternion.identity);
        GameObject myBullet1 = Instantiate(starBullet1, shootOrigin1.position, Quaternion.identity);
        GameObject myBullet2 = Instantiate(starBullet2, shootOrigin2.position, Quaternion.identity);
        GameObject myBullet3 = Instantiate(starBullet3, shootOrigin3.position, Quaternion.identity);
        GameObject myBullet4 = Instantiate(starBullet4, shootOrigin4.position, Quaternion.identity);
    }
}
