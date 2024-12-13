using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

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
        source.PlayOneShot(peaAudio[Random.Range(1,2)]);
    }


    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        if(!shooted)
        {
            if (gms.Roof)
            {
                if(transform.position.x <= 2.7f)
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

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            if (flame)
            {
                hitZombieSound();
            }

            if (freeze)
            {
                snowPeaSound();
            }
            zombie.Hit(damage, freeze, flame, false);

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
            Destroy(gameObject , 1);
        }
        else if(other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
        {
            if (flame)
            {
                hitZombieSound();
            }

            if (freeze)
            {
                snowPeaSound();
            }

            balloonZombie.Hit(damage);
            
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
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat TinyZombieBoat))
        {
            if (flame)
            {
                hitZombieSound();
            }

            if (freeze)
            {
                snowPeaSound();
            }

            TinyZombieBoat.Hit(damage);

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
        else if(other.GetComponent<TorchWood>())
        {
            if(freeze && !flame) {
                freeze = false;
                flame = false;
                damage = damage / 2;
                GetComponent<SpriteRenderer>().sprite = normalPea;
            }
            else if(!freeze &&  flame)
            {

            }
            else
            {
                burnPea();
                freeze = false;
                flame = true;
                damage = damage * 3;
                GetComponent<SpriteRenderer>().sprite = firePea;
            }
        }
    }
}
