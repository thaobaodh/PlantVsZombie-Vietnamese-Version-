using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotatoMine : MonoBehaviour
{
    public GameObject bullet;

    public GameObject _PotatoMine;

    public GameObject _UnderGroundPotatoMine;

    public Transform ExplodeOrigin;

    public float cooldown;

    private float cooldownTime;

    public bool canExplode;

    public float range;

    public LayerMask ExplodeMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] ExplodeClips;

    [SerializeField] Animator animator;



    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        Debug.Log("Potato Plant");
        animator.SetInteger("Time", 0);
        cooldownTime = cooldown;
    }

    private void Update()
    {
        if (cooldownTime > 0.2) {
            cooldownTime -= Time.deltaTime;
        }
        else if (cooldownTime > 0 && cooldownTime <= 0.2)
        {   
            if(animator.GetInteger("Time") !=  1)
            {
                animator.SetInteger("Time", 1);
            }
            cooldownTime -= Time.deltaTime;
        }
        else if(cooldownTime < 0)
        {
            ResetCooldown();
            cooldownTime = 0;
        }
        else
        {

        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, ExplodeMask);
        if (hit.collider)
        {
            target = hit.collider.gameObject;
            Explode();
        }
    }

    void ResetCooldown()
    {
        Debug.Log("Ready to explode");
        if (animator.GetInteger("Time") != 2)
        {
            animator.SetInteger("Time", 2);
        }
        canExplode = true;
    }

    void Explode()
    {
        if (!canExplode)
            return;
        if (animator.GetInteger("Time") != 3)
        {
            animator.SetInteger("Time", 3);
        }
        Debug.Log("BOOOM!");

        GetComponent<Plant>().Hit(100, 1);

        source.PlayOneShot(ExplodeClips[0]);

        if (target.GetComponent<Zombie>() != null)
        {
            target.GetComponent<Zombie>().Hit(1000, false, false, false);
        }
        else if(target.GetComponent<BalloonZombie>() != null)
        {
            target.GetComponent<BalloonZombie>().Hit(1000);
        }
        canExplode = false;

    }

}
