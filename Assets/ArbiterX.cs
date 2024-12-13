using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbiterX : MonoBehaviour
{
    public string lane;

    public GameObject boom;

    public GameObject boomTarget;

    public int numOfBoom;

    public Transform shootOrigin;

    [SerializeField] Animator animator;

    public float cooldown;

    public float reloadCooldown;

    private bool canBoom = false;

    public int health;

    public float scanSpeed;

    public float speed;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    private GameManager gms;

    private int countBoom = 0;

    public bool dead = false;

    private bool scanMode = true;

    private bool returnHome = false;


    private void Start()
    {
        animator.Play("Idle");
        scanMode = true;
        canBoom = true; 
        returnHome = false;
        dead = false;
        countBoom = 0;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!canBoom)
            return;
        if(scanMode)
        {
            if(!returnHome)
            {
                animator.Play("Invisible");
                transform.position -= new Vector3(scanSpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                animator.Play("Idle");
                transform.position += new Vector3(scanSpeed * Time.deltaTime, 0f, 0f);
            }
        }
        else
        {
            if (countBoom >= numOfBoom)
            {
                transform.position -= new Vector3(scanSpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, shootMask);
                if (hit.collider)
                {
                    target = hit.collider.gameObject;
                    Booming(target);
                }
                else
                {
                    animator.Play("Idle");
                    transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
                }
            }
        }
    }

    void ResetCooldown()
    {
        canBoom = true;
    }
    void Reload()
    {
        canBoom = true;
        countBoom = 0;
    }

    void Booming(GameObject zombieTarget)
    {
        if (!canBoom)
            return;
        // animator.Play("Shooting");
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        canBoom = false;
        countBoom++;
        if (countBoom >= numOfBoom)
        {
            animator.Play("Idle");
            Invoke(nameof(Reload), reloadCooldown);
        }
        else
        {
            GameObject myBullet = Instantiate(boom, shootOrigin.position, Quaternion.identity);
            Invoke(nameof(ResetCooldown), cooldown);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "DeathTrigger")
        {
            if(!returnHome)
            {
                if(scanMode)
                {
                    returnHome = true;
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    Destroy(gameObject, 2f);
                }
            }
            else
            {
                Destroy(gameObject, 2f);
            }
        }
        else if (other.name == "ZombieDeathTrigger")
        {
            if (scanMode && !returnHome) return;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            scanMode = false;
            returnHome = false;
            countBoom = 0;
        }
        else if(other.TryGetComponent<Plant>(out Plant plant))
        {

            if (countBoom >= numOfBoom)
            {

            }
            else
            {
                if(Random.Range(0, 5) % 2 == 0)
                {
                    Debug.Log("Marked a Plant: " + plant.name + " / " + plant.transform.position);
                    countBoom++;
                    Instantiate(boomTarget, plant.transform.position, Quaternion.identity);
                }
                else
                {

                }
            }
        }
    }


    void Hit(int damage)
    {
        if (dead) return;
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                dead = true;
                Destroy(gameObject, 1f);
            }
        }
    }
}
