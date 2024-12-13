using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusSpine : MonoBehaviour
{
    public int damage;

    public float speed = 0.8f;

    public Sprite peaPopped;

    private bool shooted = false;

    private void Start()
    {
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        if(!shooted)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            zombie.Hit(damage, false, false, false);
            GetComponent<SpriteRenderer>().sprite = peaPopped;
            shooted = true;
            Destroy(gameObject , 1);
        }
        else if(other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie)) 
        { 
            balloonZombie.Hit(damage);
            GetComponent<SpriteRenderer>().sprite = peaPopped;
            shooted = true;
            Destroy(gameObject, 1);
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            tinyZombieBoat.Hit(damage);
            GetComponent<SpriteRenderer>().sprite = peaPopped;
            shooted = true;
            Destroy(gameObject, 1);
        }
    }
}
