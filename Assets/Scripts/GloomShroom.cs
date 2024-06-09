using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomShroom : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] GameObject shroomPuff;

    [SerializeField] GameObject shroomPuff1;

    [SerializeField] GameObject shroomPuff2;

    [SerializeField] GameObject shroomPuff3;

    [SerializeField] GameObject shroomPuff4;

    [SerializeField] GameObject shroomPuff5;

    [SerializeField] GameObject shroomPuff6;

    [SerializeField] GameObject shroomPuff7;

    [SerializeField] Transform shootOrigin;

    [SerializeField] Transform shootOrigin1;

    [SerializeField] Transform shootOrigin2;

    [SerializeField] Transform shootOrigin3;

    [SerializeField] Transform shootOrigin4;

    [SerializeField] Transform shootOrigin5;

    [SerializeField] Transform shootOrigin6;

    [SerializeField] Transform shootOrigin7;

    [SerializeField] float cooldown;

    private bool canShoot;

    [SerializeField] float range;

    [SerializeField] LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    private int numOfShoot = 10;

    [SerializeField] AudioClip[] shootClips;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        InvokeRepeating("ResetCooldown", 1, cooldown);
    }

    private void FixedUpdate()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
            {
                Debug.Log("Zombie In Range");
                Shoot();
            }
        }
    }

    void ResetCooldown()
    {
        animator.SetBool("Attack", false); //Attack
        canShoot = true;
    }

    void Shoot()
    {
        if (!canShoot)
            return;
        animator.SetBool("Attack", true); //Normal
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canShoot = false;

        for (int i = 0; i < numOfShoot; i++)
        {
            GameObject myBullet = Instantiate(shroomPuff, shootOrigin.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(0, 1f), 0), Quaternion.identity);
            GameObject myBullet1 = Instantiate(shroomPuff1, shootOrigin1.position + new Vector3(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), 0), Quaternion.identity);
            GameObject myBullet2 = Instantiate(shroomPuff2, shootOrigin2.position + new Vector3(Random.Range(0f, 1f), Random.Range(-0.25f, 0.25f), 0), Quaternion.identity);
            GameObject myBullet3 = Instantiate(shroomPuff3, shootOrigin3.position + new Vector3(Random.Range(0f, 0.5f), Random.Range(-0.5f, 0f), 0), Quaternion.identity);
            GameObject myBullet4 = Instantiate(shroomPuff4, shootOrigin4.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-1f, 0f), 0), Quaternion.identity);
            GameObject myBullet5 = Instantiate(shroomPuff5, shootOrigin5.position + new Vector3(Random.Range(-0.5f, 0f), Random.Range(-0.5f, 0f), 0), Quaternion.identity);
            GameObject myBullet6 = Instantiate(shroomPuff6, shootOrigin6.position + new Vector3(Random.Range(-1f, 0f), Random.Range(-0.25f, 0.25f), 0), Quaternion.identity);
            GameObject myBullet7 = Instantiate(shroomPuff7, shootOrigin7.position + new Vector3(Random.Range(-0.5f, 0f), Random.Range(0f, 0.5f), 0), Quaternion.identity);
        }
    }
}
