using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cob : MonoBehaviour
{
    private bool reachTarget = false;

    [SerializeField] bool isArrow = false;

    [SerializeField] int numOfArrows = 20;

    [SerializeField] GameObject Arrow;

    [SerializeField] int range = 3;

    private float speed = 4;

    public Vector3 Destination;

    private AudioSource source;

    [SerializeField] AudioClip[] explodedClip;

    [SerializeField] Animator animator;

    private float RandomDestinationX = 0;
    private float RandomDestinationY = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        animator.SetBool("Exploded", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (reachTarget)
        {
            return;
        }

        if (isArrow)
        {
            if (transform.position.y <= 6f)
            {
                Debug.Log("Cob Exploded!");
                reachTarget = true;
                source.PlayOneShot(explodedClip[Random.Range(0, explodedClip.Length)]);

                animator.SetBool("Exploded", true);
                for (int i = 0; i < numOfArrows; i++)
                {
                    RandomDestinationX = Destination.x + Random.Range(-2.5f, 2.5f);
                    RandomDestinationY = Destination.y + Random.Range(-3f, 3f);
                    GameObject tinyArrow = Instantiate(Arrow, new Vector3(RandomDestinationX, Random.Range(6f, 10f), 0f), Quaternion.identity);
                    tinyArrow.GetComponent<TinyArrow>().Destination = new Vector3(RandomDestinationX, RandomDestinationY, 0);
                }

                Invoke("DestroyCob", 1f);
            }
            else
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            }
        }
        else
        {
            if (transform.position.y <= Destination.y)
            {
                Debug.Log("Cob Exploded!");
                reachTarget = true;
                source.PlayOneShot(explodedClip[Random.Range(0, explodedClip.Length)]);

                animator.SetBool("Exploded", true);

                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
                    {
                        zombie.Hit(1000, false, false, false);
                    }
                    else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                    {
                        balloonZombie.Hit(1000);
                    }
                    else if (collider2D.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
                    {
                        tinyZombieBoat.Hit(1000);
                    }
                }

                Invoke("DestroyCob", 1f);
            }
            else
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            }
        }
    }
    void DestroyCob()
    {
        Destroy(gameObject);
    }
}
