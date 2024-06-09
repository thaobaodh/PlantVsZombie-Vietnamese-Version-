using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JalapenoFlame : MonoBehaviour
{
    private int damage = 1000;

    [SerializeField] int cooldown;

    [SerializeField] Animator animator;

    private void Start()
    {
        animator.SetInteger("Type", Random.Range(1, 4));
        InvokeRepeating("ResetCooldown", 1, cooldown);
    }

    void ResetCooldown()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            Debug.Log("Zombie in range -> Ignite Zombie");
            zombie.Hit(damage, false, false, false);
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            Debug.Log("Tiny Boat Zombie in range -> Ignite Tiny Boat Zombie");
            tinyZombieBoat.Hit(damage);
        }
    }
}
