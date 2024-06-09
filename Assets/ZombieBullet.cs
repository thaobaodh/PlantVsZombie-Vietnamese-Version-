using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBullet : MonoBehaviour
{
    public int damage;

    public float speed = 0.8f;

    private bool shooted = false;

    private AudioSource source;

    public AudioClip[] bulletAudio;

    private GameManager gms;

    void hitZombieSound()
    {
        source.PlayOneShot(bulletAudio[Random.Range(1, 2)]);
    }


    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
        Destroy(gameObject, 10);
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
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);

        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Plant>(out Plant plant))
        {
            plant.Hit(damage, 1);

            shooted = true;
            Destroy(gameObject, 1);
        }
    }

}
