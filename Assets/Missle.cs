using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Missle : MonoBehaviour
{
    private bool reachTarget = false;

    [SerializeField] bool isArrow = false;

    [SerializeField] int numOfArrows = 20;

    [SerializeField] GameObject Arrow;

    [SerializeField] float range = 3;

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
        animator.Play("Idle");
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
                Debug.Log("Missle Exploded!");
                reachTarget = true;
                source.PlayOneShot(explodedClip[Random.Range(0, explodedClip.Length)]);

                animator.Play("Exploded");
                for (int i = 0; i < numOfArrows; i++)
                {
                    RandomDestinationX = Destination.x + Random.Range(-2.5f, 2.5f);
                    RandomDestinationY = Destination.y + Random.Range(-3f, 3f);
                    GameObject tinyArrow = Instantiate(Arrow, new Vector3(RandomDestinationX, Random.Range(6f, 10f), 0f), Quaternion.identity);
                    tinyArrow.GetComponent<TinyArrow>().Destination = new Vector3(RandomDestinationX, RandomDestinationY, 0);
                }

                Invoke("DestroyMissle", 1f);
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
                Debug.Log("Missle Exploded!");
                reachTarget = true;
                source.PlayOneShot(explodedClip[Random.Range(0, explodedClip.Length)]);

                animator.Play("Exploded");

                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
                foreach (Collider2D collider2D in colliderArray)
                {
                    if(collider2D.TryGetComponent<Tile>(out Tile tile))
                    {
                        tile.hasPlant = true;
                        tile.isPlantable = false;
                        if (tile.plantObject != null)
                        {
                            Destroy(tile.plantObject);
                        }

                        if (tile.pumpkinObject != null)
                        {
                            Destroy(tile.pumpkinObject);
                        }

                        if (tile.backgroundObject != null)
                        {
                            Destroy(tile.backgroundObject);
                        }

                        tile.plantObject = null;
                        tile.pumpkinObject = null;
                        tile.backgroundObject = null;
                        tile.hasPumpkin = false;
                        tile.isUnavailable = true;
                    }

                    //if (collider2D.TryGetComponent<Plant>(out Plant plant))
                    //{
                    //    plant.Hit(1000, 1);
                    //}
                }

                Invoke("DestroyMissle", 2f);
            }
            else
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            }
        }
    }

    void DestroyMissle()
    {
        Destroy(gameObject);
    }
}
