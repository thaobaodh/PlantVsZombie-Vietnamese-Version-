using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomShroomPuff : MonoBehaviour
{
    [SerializeField] int bulletPosition;

    [SerializeField] int damage;

    [SerializeField] float speed = 0.8f;

    private bool shooted = true;

    private void Start()
    {
        Destroy(gameObject, 1);
    }
    private void FixedUpdate()
    {
        if (shooted)
        {
            switch (bulletPosition)
            {
                case 0:

                    transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                    break;

                case 1:
                    transform.position += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
                    break;

                case 2:
                    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                    break;

                case 3:
                    transform.position += new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0);
                    break;

                case 4:
                    transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
                    break;


                case 5:
                    transform.position += new Vector3(-speed * Time.deltaTime, -speed * Time.deltaTime, 0);
                    break;


                case 6:
                    transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
                    break;


                case 7:
                    transform.position += new Vector3(-speed * Time.deltaTime, speed * Time.deltaTime, 0);
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
            Destroy(gameObject, 1);
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            tinyZombieBoat.Hit(damage);
            shooted = true;
            Destroy(gameObject, 1);
        }
    }
}
