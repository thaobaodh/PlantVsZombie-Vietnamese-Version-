using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerGrass : MonoBehaviour
{
    public string lane;

    public GameObject[] bullet;

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

    [SerializeField] Animator animator;

    public bool isReturn = false;

    private void Start()
    {
        canShoot = true;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (isReturn)
        {
            if (!animator.GetBool("Jumpout"))
            {
                animator.SetBool("Jumpout", true);
                isReturn = false;
            }
        }
        else
        {
            if (animator.GetBool("Jumpout"))
            {
                animator.SetBool("Jumpout", false);
            }
        }

        if (!canShoot)
            return;

        if (gms.Roof)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, raycastPositionY[int.Parse(lane) - 1]), Vector2.right, range, shootMask);
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                animator.SetBool("Jumpout", true);
                Shoot(target);
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                animator.SetBool("Jumpout", true);
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

        GameObject myBullet = Instantiate(bullet[Random.Range(0, bullet.Length)], shootOrigin.position, Quaternion.identity);

        myBullet.GetComponent<Tiger>().lane = lane;

        myBullet.GetComponent<Tiger>().target = zombieTarget;

        Invoke("ResetCooldown", cooldown);

        animator.SetBool("Jumpout", false);
    }
}
