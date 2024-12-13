using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPea : MonoBehaviour
{
    public int damage;
    public float speed = 0.8f;
    public bool freeze;

    public bool flame;

    private bool shooted = false;

    public Sprite[] peaPopped;

    public Sprite firePea;

    public Sprite normalPea;

    private AudioSource source;

    public AudioClip[] peaAudio;

    private float DestX;

    private float DestY;
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
        source = GetComponent<AudioSource>();
        DestX = transform.position.x;
        DestY = transform.position.y;
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        if(!shooted)
        {
            if (transform.position.y >= (DestY - 1.6f))
            {
                transform.position += new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0);
            }
            else
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
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
            gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
            GetComponent<SpriteRenderer>().sprite = peaPopped[Random.Range(0, peaPopped.Length)];
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
            gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
            GetComponent<SpriteRenderer>().sprite = peaPopped[Random.Range(0, peaPopped.Length)];
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
