using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BambooStack : MonoBehaviour
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

    private bool goBack = false;

    private float targetX;

    private float targetY;

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
        goBack = false;
        targetX = transform.position.x;
        targetY = transform.position.y;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
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
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }

        }
        else
        {
        }

        if(goBack)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0); 
            if (Mathf.Abs(transform.position.x - targetX) <= 0.2f)
            {
                goBack = false;
                Destroy(gameObject, 0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
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

            shooted = true;
            goBack = true;
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            if (flame)
            {
                hitZombieSound();
            }

            if (freeze)
            {
                snowPeaSound();
            }
            tinyZombieBoat.Hit(damage);

            shooted = true;
            goBack = true;
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
                freeze = false;
                flame = true;
                damage = damage * 3;
                GetComponent<SpriteRenderer>().sprite = firePea;
            }
        }
    }
}
