using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FumePuff : MonoBehaviour
{
    public int damage;

    public float speed = 0.8f;

    private bool shooted = false;
    [SerializeField] bool sprayPuff;

    private void Start()
    {
        Destroy(gameObject, 1);
    }
    private void FixedUpdate()
    {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            zombie.Hit(damage, false, false, false);
            if (sprayPuff)
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            tinyZombieBoat.Hit(damage);
            if (sprayPuff)
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }
}
