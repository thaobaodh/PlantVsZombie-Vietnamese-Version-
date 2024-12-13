using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissleZombie : MonoBehaviour
{
    public string lane;

    public GameObject missleObject;

    public GameObject targetMissleObject;

    public Transform shootOrigin;

    [SerializeField] Animator animator;

    public float cooldown;

    private bool canShoot = false;

    public int health;

    public float range;

    public LayerMask shootMask;

    private AudioSource source;

    public AudioClip[] shootClips;

    private GameManager gms;

    private Vector3 targetPosition;

    public bool dead = false;

    private void Start()
    {
        animator.Play("Idle");
        canShoot = true;
        dead = false;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!canShoot)
            return;
        
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Tile>(out Tile tile))
            {
                if (tile.plantObject != null || tile.backgroundObject != null || tile.pumpkinObject != null)
                {
                    animator.Play("Release");
                    Debug.Log("Missle Release - Tile summon: " + tile.name);
                    GameObject targetMark = Instantiate(targetMissleObject, tile.transform.position, Quaternion.identity);
                    targetPosition = tile.transform.position;
                    Invoke(nameof(Shoot), 2f);
                    canShoot = false;
                    break;
                }
            }
        }
    }


    void Shoot()
    {
        GameObject missle = Instantiate(missleObject, new Vector3(targetPosition.x, targetPosition.y + 9, targetPosition.z), Quaternion.identity);
        missle.GetComponent<Missle>().Destination = targetPosition;
        Destroy(gameObject, 2);
        Invoke(nameof(Reload), cooldown - 2);
        Invoke(nameof(ResetCooldown), cooldown);
    }
    void Reload()
    {
        animator.Play("Reload");
    }

    void ResetCooldown()
    {
        canShoot = true;
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
