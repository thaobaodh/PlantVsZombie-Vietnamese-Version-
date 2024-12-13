using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Peach : MonoBehaviour
{
    [SerializeField] private int health = 50;

    [SerializeField] private float speed = 1f;

    [SerializeField] public Vector3 targetPoint;

    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        this.PlayAnimation("Adsorb");
        this.Movement();
    }

    private void PlayAnimation(string animatorName)
    {
        // animator.Play(animatorName);
    }

    private void Movement()
    {
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<YellowBrowDemonZombie>(out YellowBrowDemonZombie yellowBrowDemonZombie))
        {
            Debug.Log("Comeback to Yellow Brown Demon Zombie");
            if(yellowBrowDemonZombie.health + health >= yellowBrowDemonZombie.maxHealth)
            {
                yellowBrowDemonZombie.health = yellowBrowDemonZombie.maxHealth;
            }
            else
            {
                yellowBrowDemonZombie.health += health;
            }
            Destroy(gameObject, 1f);
        }
    }
}
