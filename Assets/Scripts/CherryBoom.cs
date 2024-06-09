using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBoom : MonoBehaviour
{
    public float cooldown;

    public float range;

    public LayerMask shootMask;

    private AudioSource source;

    public AudioClip[] shootClips;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        InvokeRepeating("ResetCooldown", 1, cooldown);
    }

    void ResetCooldown()
    {
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
            {
                zombie.Hit(1000, false, false, false);
            }
            else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
            {
                balloonZombie.Hit(1000);
            }
            else if (collider2D.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
            {
                tinyZombieBoat.Hit(1000);
            }
        }

        GetComponent<Plant>().Hit(100, 1);
    }

}
