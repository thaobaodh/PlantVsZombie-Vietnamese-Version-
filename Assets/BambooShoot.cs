using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BambooShoot : MonoBehaviour
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

    [SerializeField] bool isLargeView = false;

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
            if(!isLargeView)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
                if (hit.collider)
                {
                    target = hit.collider.gameObject;
                    Shoot(target);
                }
            }
            else
            {
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<ZombieBoat>(out ZombieBoat zombieBoat))
                    {
                        target = zombieBoat.gameObject;
                        Shoot(target);
                    }
                }
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
        if(bullet.name == "PlantCannonBall" || bullet.name == "TinyPlantCannonBall")
        {
            myBullet.GetComponent<CannonBall>().target = new Vector3(zombieTarget.transform.position.x, zombieTarget.transform.position.y - 1.5f, zombieTarget.transform.position.z);
        }
        else
        {

        }

        if (bullet.name == "Watermelon" || bullet.name == "IceMelon")
        {
            myBullet.GetComponent<Watermelon>().lane = lane;
            myBullet.GetComponent<Watermelon>().target = zombieTarget;
        }
        else
        {

        }

        Invoke(nameof(ResetCooldown), cooldown);
        //In Roof Map -> Pea Shooter was not shoot zombie which located on Tile % 9 < 4
    }
}
