using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheImmortal : MonoBehaviour
{
    [SerializeField] int damage;

    [SerializeField] int usingTime = 5;

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

    private void Update()
    {
    }

    void ResetEatCooldown()
    {
        canEat = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.layer == 7)
        {
            if (!canEat || !other)
                return;
            source.PlayOneShot(sound[Random.Range(0, sound.Length)]);
            canEat = false;

            other.GetComponent<Zombie>().Hit(damage, false, false, false);

            usingTime--;
            if (usingTime <= 0)
            {
                Destroy(gameObject, 1);
            }
        }
    }

}
