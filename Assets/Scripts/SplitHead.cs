using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitHead : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    [SerializeField] GameObject BackBullet;

    [SerializeField] Transform shootOrigin;

    [SerializeField] Transform backShootOrigin;

    [SerializeField] float cooldown;

    [SerializeField] float BackCooldown;

    private bool canShoot;

    private bool canBackShoot;

    [SerializeField] float range;

    [SerializeField] LayerMask shootMask;

    private GameObject target;

    private GameObject backTarget;

    private AudioSource source;

    [SerializeField] AudioClip[] shootClips;

    private GameManager gms;

    private float[] raycastPositionY = { 3.49f, 1.71f, 0.17f, -1.28f, -2.91f };

    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
        InvokeRepeating("ResetCooldown", 1, cooldown);
        InvokeRepeating("ResetBackCooldown", 1, BackCooldown);
    }

    private void FixedUpdate()
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
                    target = hit.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hitBack = Physics2D.Raycast(transform.position, Vector2.left, 1.5f, shootMask);
                if (hitBack.collider)
                {
                    Debug.Log("Back Enemy detected!");
                    backTarget = hitBack.collider.gameObject;
                    ShootBack();
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                if (hit.collider)
                {
                    target = hit.collider.gameObject;
                    Shoot();
                }

                RaycastHit2D hitBack = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
                if (hitBack.collider)
                {
                    Debug.Log("Back Enemy detected!");
                    backTarget = hitBack.collider.gameObject;
                    ShootBack();
                }
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                Shoot();
            }

            RaycastHit2D hitBack = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
            if (hitBack.collider)
            {
                Debug.Log("Back Enemy detected!");
                backTarget = hitBack.collider.gameObject;
                ShootBack();
            }
        }
    }

    void ResetCooldown()
    {
        canShoot = true;
    }

    void ResetBackCooldown()
    {
        canBackShoot = true; 
    }

    void Shoot()
    {
        if (!canShoot)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canShoot = false;

        GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
    }


    void ShootBack()
    {
        if (!canBackShoot)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canBackShoot = false;
        for(int i = 0; i < 2; i++)
        {
            GameObject myBullet = Instantiate(BackBullet, backShootOrigin.position + new Vector3(i * 0.5f, 0, 0), Quaternion.identity);
        }
    }
}
