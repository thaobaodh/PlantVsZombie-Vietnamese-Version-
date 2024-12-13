using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BamBooBullet : MonoBehaviour
{
    public int damage;

    public float explodeRange = 1.5f;

    public float speed = 0.8f;

    public bool freeze;

    public bool flame;

    private bool shooted = false;

    public Sprite[] peaPopped;

    public Sprite[] snowpeaPopped;

    public Sprite firePea;

    public Sprite normalPea;

    private AudioSource source;

    public AudioClip[] peaAudio;

    private GameManager gms;

    [SerializeField] Animator animator;

    void burnPea()
    {
        source.PlayOneShot(peaAudio[0]);
    }

    void snowPeaSound()
    {
        source.PlayOneShot(peaAudio[3]);
    }

    void hitZombieSound()
    {
        source.PlayOneShot(peaAudio[Random.Range(1, 2)]);
    }


    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
        Destroy(gameObject, 10);
        animator.SetInteger("Status", 0);
    }
    private void Update()
    {
        if (!shooted)
        {
            if (gms.Roof)
            {
                if (transform.position.x <= 2.7f)
                {
                    Destroy(gameObject, 1);
                }
            }
            else
            {

            }
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            if (flame)
            {
                hitZombieSound();
                animator.SetInteger("Status", 2);
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, explodeRange);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<Zombie>(out Zombie zombieExploded))
                    {
                        Debug.Log("Zombie In Range");
                        zombieExploded.Hit(damage, freeze, flame, false);
                    }
                    else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombieExploded))
                    {
                        balloonZombieExploded.Hit(damage);
                    }
                    else if (collider2D.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat1))
                    {
                        tinyZombieBoat1.Hit(damage);
                    }
                }
            }
            else
            {
                snowPeaSound();
                zombie.Hit(damage, freeze, flame, false);
            }


            if (freeze)
            {
                gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 0);
                GetComponent<SpriteRenderer>().sprite = snowpeaPopped[Random.Range(0, snowpeaPopped.Length)];
            }
            else
            {
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                GetComponent<SpriteRenderer>().sprite = peaPopped[Random.Range(0, peaPopped.Length)];
            }

            shooted = true;
            Destroy(gameObject, 1);
        }
        else if (other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
        {
            hitZombieSound();

            if (flame)
            {
                hitZombieSound();
                animator.SetInteger("Status", 2);
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, explodeRange);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<Zombie>(out Zombie zombieExploded))
                    {
                        Debug.Log("Zombie In Range");
                        zombieExploded.Hit(damage, freeze, flame, false);
                    }
                    else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombieExploded))
                    {
                        balloonZombieExploded.Hit(damage);
                    }
                    else if (collider2D.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat1))
                    {
                        tinyZombieBoat1.Hit(damage);
                    }
                }
            }
            else
            {
                snowPeaSound();
                zombie.Hit(damage, freeze, flame, false);
            }

            if (freeze)
            {
                gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 0);
                GetComponent<SpriteRenderer>().sprite = snowpeaPopped[Random.Range(0, snowpeaPopped.Length)];
            }
            else
            {
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                GetComponent<SpriteRenderer>().sprite = peaPopped[Random.Range(0, peaPopped.Length)];
            }


            shooted = true;
            Destroy(gameObject, 1);
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            hitZombieSound();

            if (flame)
            {
                hitZombieSound();
                animator.SetInteger("Status", 2);
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, explodeRange);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<Zombie>(out Zombie zombieExploded))
                    {
                        Debug.Log("Zombie In Range");
                        zombieExploded.Hit(damage, freeze, flame, false);
                    }
                    else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombieExploded))
                    {
                        balloonZombieExploded.Hit(damage);
                    }
                    else if (collider2D.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat1))
                    {
                        tinyZombieBoat1.Hit(damage);
                    }
                }
            }
            else
            {
                snowPeaSound();
                zombie.Hit(damage, freeze, flame, false);
            }

            if (freeze)
            {
                gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 0);
                GetComponent<SpriteRenderer>().sprite = snowpeaPopped[Random.Range(0, snowpeaPopped.Length)];
            }
            else
            {
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                GetComponent<SpriteRenderer>().sprite = peaPopped[Random.Range(0, peaPopped.Length)];
            }


            shooted = true;
            Destroy(gameObject, 1);
        }
        else if (other.GetComponent<TorchWood>())
        {
            if (freeze && !flame)
            {
                freeze = false;
                flame = false;
                damage = damage / 2;
                GetComponent<SpriteRenderer>().sprite = normalPea;
            }
            else if (!freeze && flame)
            {

            }
            else
            {
                burnPea();
                animator.SetInteger("Status", 1);
                freeze = false;
                flame = true;
                damage = damage * 3;
                GetComponent<SpriteRenderer>().sprite = firePea;
            }
        }
    }
}
