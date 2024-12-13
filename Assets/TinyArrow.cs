using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyArrow : MonoBehaviour
{
    private bool reachTarget = false;
    public Vector3 Destination;
    [SerializeField] Sprite[] tinyArrowSprites;
    [SerializeField] float speed;
    private bool availableDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        availableDamage = false;
        reachTarget = false;
        transform.rotation = WayBackHome();
        transform.GetComponent<SpriteRenderer>().sprite = tinyArrowSprites[Random.Range(0, tinyArrowSprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (reachTarget)
        {
            return;
        }

        if (transform.position.y <= Destination.y + 2f && transform.position.y > Destination.y)
        {
            availableDamage = true;
        }

        if (transform.position.y <= Destination.y)
        {
            availableDamage = false;
            reachTarget = true;
            Invoke("DestroyObjectFunction", 2);
        }
        else if(transform.position.y > Destination.y)
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }

    }
    private static Quaternion WayBackHome()
    {
        return Quaternion.Euler(0, 0, -90);
    }

    private void DestroyObjectFunction()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!availableDamage) return;

        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            if(zombie != null)
            {
                zombie.Hit(10, false, false, false);
            }
        }
        else if (other.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
        {
            if (balloonZombie != null)
            {
                balloonZombie.Hit(10);
            }
        }
        else if (other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            if (tinyZombieBoat != null)
            {
                tinyZombieBoat.Hit(10);
            }
        }
    }
}
