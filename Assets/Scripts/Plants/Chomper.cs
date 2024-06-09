using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chomper : MonoBehaviour
{
    public float cooldown;

    private bool canEat;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    [SerializeField] Animator animator;

    private void Start()
    {
        canEat = true;
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
        if (hit.collider)
        {
            target = hit.collider.gameObject;
            Swallow(target);
        }
    }

    void Gulp()
    {
        animator.SetInteger("Status", 3);
        Invoke("ResetCooldown", 1);
    }

    void Swallowing()
    {
        animator.SetInteger("Status", 2);
        Invoke("Gulp", cooldown - 2);
    }

    void ResetCooldown()
    {
        canEat = true;
        animator.SetInteger("Status", 4);
        Invoke("Normal", 1);
    }
    void Normal()
    {
        animator.SetInteger("Status", 0);
    }    
    void Swallow(GameObject target)
    {
        if (!canEat)
            return;
        animator.SetInteger("Status", 1);
        Invoke("Swallowing", 1);
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canEat = false;
        target.GetComponent<Zombie>().Hit(997, false, false, false);
    }
}
