using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoom : MonoBehaviour
{
    public int damage;

    public float speed = 0.8f;

    private bool shooted = false;

    private AudioSource source;

    public AudioClip[] bulletAudio;

    private GameManager gms;

    [SerializeField] Animator animator;

    private void Start()
    {
        animator.Play("Idle");
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
        Destroy(gameObject, 10);
    }
    private void Update()
    {
        if (!shooted)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);

        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "BoomTarget(Clone)")
        {
            Debug.Log("Reach Target -> Exploded");
            source.PlayOneShot(bulletAudio[0]);
            animator.Play("Exploded"); 
            shooted = true;
            Destroy(gameObject, 2);
            if (other.TryGetComponent<Plant>(out Plant plant))
            {
                plant.Hit(damage, 1);
            }
        }
    }

}
