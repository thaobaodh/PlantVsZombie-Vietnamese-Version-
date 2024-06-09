using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : MonoBehaviour
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, shootMask);
                if (hit.collider)
                {
                    // Debug.Log("Detect Zombie in Short range");
                    target = hit.collider.gameObject;
                    Shoot();
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                if (hit.collider)
                {
                    // Debug.Log("Detect Zombie in Long range " + range);
                    target = hit.collider.gameObject;
                    Shoot();
                }
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
            if (hit.collider)
            {
                // Debug.Log("Detect Zombie in Long range " + range);
                target = hit.collider.gameObject;
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
        for (int i = 0; i < 2; i++)
        {
            source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);

            GameObject myBullet = Instantiate(bullet, shootOrigin.position + new Vector3(i, 0, 0), Quaternion.identity);
        }
        canShoot = false;

    }
}
