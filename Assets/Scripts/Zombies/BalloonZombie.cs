using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonZombie : MonoBehaviour
{
    private float speed;
    private int health;

    private string zName;

    private int damage;
    private float range;
    public LayerMask plantMask;
    private float eatCooldown;
    private bool canEat = true;
    public Plant targetPlant;

    public ZombieType type;

    private AudioSource source;

    public AudioClip[] groans;

    public AudioClip[] chomps;

    public bool lastZombie;

    public bool dead;

    private int max_health;

    private GameManager gms;
    void Groans()
    {
        source.PlayOneShot(groans[Random.Range(0, groans.Length)]);
    }

    void Chomps()
    {
        source.PlayOneShot(chomps[Random.Range(0, chomps.Length)]);
        source.volume = 0.3f;
    }

    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        zName = type.zName;
        health = type.health;
        max_health = health;
        speed = type.speed;
        damage = type.damage;
        range = type.range;
        source = GetComponent<AudioSource>();
        Invoke("Groans", Random.Range(1, 20));
    }
    private void Update()
    {

        RaycastHit2D zombieHit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);

        if (zombieHit.collider)
        {
            targetPlant = zombieHit.collider.GetComponent<Plant>();
            EatPlant();
        }
    }
    void EatPlant()
    {
        if (!canEat || !targetPlant)
            return;
        canEat = false;
        Chomps();
        InvokeRepeating("ResetEatCooldown", 2, 2);
        targetPlant.Hit(damage,0);
    }

    void ResetEatCooldown()
    {

        canEat = true;
    }

    private void FixedUpdate()
    {
        if (!targetPlant)
        {
            transform.position -= new Vector3(speed, 0, 0);
        }
    }


    public void Hit(int damage)
    {
        //Debug.Log(gameObject.name);
        if(dead) return;

        if (gameObject.name == "FlyBalloonZombie(Clone)")
        {
            if(damage == 1000)
            {

            }
            else
            {
                source.PlayOneShot(type.hitClips[3]);
            }
            Destroy(gameObject);
            GameObject myZombie = Instantiate(type.model, transform.position, Quaternion.identity);
        }
        else
        {
            if (damage == 1000)
            {

            }
            else
            {
                source.PlayOneShot(type.hitClips[Random.Range(0, 2)]);
            }
        }


        health -= damage;
        
        if (health <= 0)
        {
            dead = true;

            Destroy(gameObject, 1);

            if (!gms.StartSpawningZombie) return;
            if (dead)
            {
                GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>().zombiesDie++;
                gameObject.GetComponent<Collider2D>().isTrigger = false;
                return;
            }
        }
    }
}
