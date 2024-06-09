using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Star : MonoBehaviour
{
    [SerializeField] Sprite[] starSplash;

    [SerializeField] int bulletPosition; 

    [SerializeField]  int damage;

    [SerializeField]  float speed = 0.8f;

    private bool shooted = false;

    private void Start()
    {
        Destroy(gameObject, 3);
    }
    private void FixedUpdate()
    {
        if (!shooted)
        {
            switch (bulletPosition)
            {
                case 0:

                    transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                    break;

                case 1:
                    transform.position += new Vector3(speed * Time.deltaTime, (speed * Time.deltaTime) / 3, 0);
                    break;

                case 2:
                    transform.position += new Vector3(speed * Time.deltaTime, -2 * (speed * Time.deltaTime) / 5, 0);
                    break;

                case 3:
                    transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
                    break;

                case 4:
                    transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                    break;
                default:
                    break;
            }

        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            zombie.Hit(damage, false, false, false);
            shooted = true;
            gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 0);
            GetComponent<SpriteRenderer>().sprite = starSplash[Random.Range(0, starSplash.Length)];
            Destroy(gameObject, 1);
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            tinyZombieBoat.Hit(damage);
            shooted = true;
            gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 0);
            GetComponent<SpriteRenderer>().sprite = starSplash[Random.Range(0, starSplash.Length)];
            Destroy(gameObject, 1);
        }
    }
}
