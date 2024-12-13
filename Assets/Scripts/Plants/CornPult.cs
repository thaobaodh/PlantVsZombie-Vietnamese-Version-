using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornPult : MonoBehaviour
{
    public string lane;

    public GameObject bullet;

    public GameObject butterBullet;

    public Transform shootOrigin;

    public float cooldown;

    private bool canShoot;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    private GameManager gms;

    private float[] raycastPositionY = {3.49f, 1.71f, 0.17f, -1.28f, -2.91f};

    // private int numOfBullet = 0;

    private void Start()
    {

        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
        InvokeRepeating("ResetCooldown", 1, cooldown);
    }

    private void Update()
    {
        //Check shoot trigger in Roof map -> Cabbage Pult, Corn Pult, Winter Melon, Melon Pult
        //There was a zombie animation error when zombies were on the roof map
        //Zombies are summonning in Grave Stones
        if (!canShoot)
            return;

        if (gms.Roof)
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
        /*
        if (numOfBullet == 5)
        {
            GameObject myBullet = Instantiate(butterBullet, shootOrigin.position, Quaternion.identity);
            numOfBullet = 0;
        }
        else
        {
            GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
            numOfBullet++;
        }
        */
        if (Random.Range(0,5) % 5 == 0)
        {
            GameObject myBullet = Instantiate(butterBullet, shootOrigin.position, Quaternion.identity);
            myBullet.GetComponent<Butter>().lane = lane;
            myBullet.GetComponent<Butter>().target = zombieTarget;
        }
        else
        {
            GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
            myBullet.GetComponent<cabbage>().lane = lane;
            myBullet.GetComponent<cabbage>().target = zombieTarget;
        }
    }
}
