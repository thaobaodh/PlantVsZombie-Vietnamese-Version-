using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRock : MonoBehaviour
{
    [SerializeField] int damage;

    [SerializeField] int eatCooldown;

    [SerializeField] Animator animator;

    private AudioSource source;

    private bool canEat = false;

    [SerializeField] AudioClip[] sound;
    // Start is called before the first frame update

    void Start()
    {
        source = GetComponent<AudioSource>();
        InvokeRepeating("ResetEatCooldown", 1, eatCooldown);
    }

    void ResetEatCooldown()
    {
        animator.SetBool("Attack", false); //Attack
        canEat = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.layer == 7)
        {
            animator.SetBool("Attack", true); //Normal
            if (!canEat || !other)
                return;
            source.PlayOneShot(sound[0]);
            canEat = false;
            other.GetComponent<Zombie>().Hit(damage, false, false, false);
        }
        // Destroy(gameObject, 1);
    }
}
