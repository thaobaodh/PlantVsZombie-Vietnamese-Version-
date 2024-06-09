using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePeater : MonoBehaviour
{
    public GameObject FirstBullet;

    public GameObject bullet;

    public GameObject ThirdBullet;

    public Transform shootOrigin;

    public Transform shootOrigin1;

    public Transform shootOrigin2;

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
                RaycastHit2D firstHit = Physics2D.Raycast(transform.position + new Vector3(0, 1.5f, 0), Vector2.right, 1.5f, shootMask);
                RaycastHit2D secondHit = Physics2D.Raycast(transform.position, Vector2.right, 1.5f, shootMask);
                RaycastHit2D thirdHit = Physics2D.Raycast(transform.position + new Vector3(0, -1.5f, 0), Vector2.right, 1.5f, shootMask);

                if (firstHit.collider)
                {
                    target = firstHit.collider.gameObject;
                    Shoot();
                }

                if (secondHit.collider)
                {
                    target = secondHit.collider.gameObject;
                    Shoot();
                }

                if (thirdHit.collider)
                {
                    target = thirdHit.collider.gameObject;
                    Shoot();
                }
            }
            else
            {
                RaycastHit2D firstHit = Physics2D.Raycast(transform.position + new Vector3(0, 1.5f, 0), Vector2.right, range, shootMask);
                RaycastHit2D secondHit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                RaycastHit2D thirdHit = Physics2D.Raycast(transform.position + new Vector3(0, -1.5f, 0), Vector2.right, range, shootMask);

                if (firstHit.collider)
                {
                    target = firstHit.collider.gameObject;
                    Shoot();
                }

                if (secondHit.collider)
                {
                    target = secondHit.collider.gameObject;
                    Shoot();
                }

                if (thirdHit.collider)
                {
                    target = thirdHit.collider.gameObject;
                    Shoot();
                }
            }
        }
        else
        {
            RaycastHit2D firstHit = Physics2D.Raycast(transform.position + new Vector3(0, 1.5f, 0), Vector2.right, range, shootMask);
            RaycastHit2D secondHit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
            RaycastHit2D thirdHit = Physics2D.Raycast(transform.position + new Vector3(0, -1.5f, 0), Vector2.right, range, shootMask);

            if (firstHit.collider)
            {
                target = firstHit.collider.gameObject;
                Shoot();
            }

            if (secondHit.collider)
            {
                target = secondHit.collider.gameObject;
                Shoot();
            }

            if (thirdHit.collider)
            {
                target = thirdHit.collider.gameObject;
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

        GameObject myFirstBullet = Instantiate(FirstBullet, shootOrigin.position, Quaternion.identity);
        GameObject mySecondBullet = Instantiate(bullet, shootOrigin1.position, Quaternion.identity);
        GameObject myThirdBullet = Instantiate(ThirdBullet, shootOrigin2.position, Quaternion.identity);
    }
}
