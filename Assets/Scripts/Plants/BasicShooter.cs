using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : MonoBehaviour
{
    public string lane;

    public GameObject bullet;

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

    private void Start()
    {
        canShoot = true;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!canShoot)
            return;
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
                if(transform.position.x < 2.7)
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

    void ResetCooldown()
    {
        canShoot = true;
    }

    void Shoot(GameObject zombieTarget)
    {
        if (!canShoot)
            return;
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canShoot = false;

        GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);

        if (bullet.name == "Watermelon" || bullet.name == "IceMelon")
        {
            myBullet.GetComponent<Watermelon>().lane = lane;
            myBullet.GetComponent<Watermelon>().target = zombieTarget;
        }
        else
        {

        }

        Invoke(nameof(ResetCooldown), cooldown);
    }
}
