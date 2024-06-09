using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEngine.GraphicsBuffer;

public class RunnablePotatoMiner : MonoBehaviour
{
    public bool Roll;
    public bool Exploded;
    [SerializeField] float inRange;
    //[SerializeField] Animator animator;
    [SerializeField] float range;
    [SerializeField] float speed;
    [SerializeField] int damage;

    public LayerMask shootMask;

    private AudioSource source;

    public AudioClip[] shootClips;

    private int count = 0;
    private float realSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        realSpeed = 0;
        Roll = false; 
        Exploded = false;
        source = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(realSpeed * Time.deltaTime, 0, 0);

        if (Roll) return;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, inRange, shootMask);
        if (hit.collider)
        {
            realSpeed = speed;
            Roll = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Exploded) return;
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            count++;
            if (count == 1)
            {
                source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
            }
            Debug.Log("Runnable Potato Miner damage a zombie");

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D collider2D in colliderArray)
            {
                if (collider2D.TryGetComponent<Zombie>(out Zombie zombie2))
                {
                    realSpeed = 0;
                    Exploded = true;
                    zombie2.Hit(1000, false, false, false);
                }
                else if (collider2D.TryGetComponent<BalloonZombie>(out BalloonZombie balloonZombie))
                {
                    realSpeed = 0;
                    Exploded = true;
                    balloonZombie.Hit(1000);
                }
            }

            GetComponent<Plant>().Hit(100, 2);
        }
    }
}
