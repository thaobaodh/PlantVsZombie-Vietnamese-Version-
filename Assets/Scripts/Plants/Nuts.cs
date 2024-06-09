using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Nuts : MonoBehaviour
{
    public bool Roll;
    public bool Exploded;
    private int Turn;
    [SerializeField] Animator animator;
    [SerializeField] float range;
    [SerializeField] float speed;
    [SerializeField] int damage;
    [SerializeField] private int health;
    [SerializeField] private int max_health;

    public Sprite Wallnut_body;
    public Sprite Wallnut_cracked1;
    public Sprite Wallnut_cracked2;

    // Start is called before the first frame update
    void Start()
    {
        Turn = 0;
        if(Roll)
        {
            animator.SetBool("Roll", false);
        }
        else
        {

        }

        if(Exploded)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        // Roll = false;
        max_health = GetComponent<Plant>().health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        health = GetComponent<Plant>().health;
        if (health > (2 * max_health) / 3)
        {
            GetComponent<SpriteRenderer>().sprite = Wallnut_body;
            GetComponent<SpriteRenderer>().transform.localScale = Vector3.one;
        }
        else if ((health > (1 * max_health) / 3) && (health <= (2 * max_health) / 3))
        {
            GetComponent<SpriteRenderer>().sprite = Wallnut_cracked1;
            GetComponent<SpriteRenderer>().transform.localScale = Vector3.one;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Wallnut_cracked2;
            GetComponent<SpriteRenderer>().transform.localScale = Vector3.one;
        }

        if (!Roll)
            return;
        if(!animator.GetBool("Roll"))
        {
            animator.SetBool("Roll", true);
        }
        Destroy(gameObject, 5);
        if (Turn == 0)
        {
            gameObject.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else if (Turn == 1)
        {
            gameObject.transform.position += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
        }
        else if(Turn == 2)
        {
            gameObject.transform.position += new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Roll && !Exploded) return;
        if(other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            // Debug.Log("Wallnut damage a zombie");
            if(Exploded)
            {
                Destroy(gameObject);
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<Zombie>(out Zombie zombie2))
                    {
                        zombie2.Hit(1000, false, false, false);
                    }
                    else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                    {
                        balloonZombie.Hit(1000);
                    }
                }
            }
            else if(Roll)
            {
                zombie.Hit(damage, false, false, false);
                if (Random.Range(0, 10) % 2 == 0)
                {
                    Turn = 1;
                }
                else
                {
                    Turn = 2;
                }
            }

        }
    }
}
