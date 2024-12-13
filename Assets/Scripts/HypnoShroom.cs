using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HypnoShroom : MonoBehaviour
{
    public bool canPoison;

    public float range;

    public LayerMask HypnoMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] HypnoClips;



    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, HypnoMask);
        if (hit.collider)
        {
            target = hit.collider.gameObject;
            PoisonZombie(target);
        }
    }

    void PoisonZombie(GameObject targetObject)
    {
        if (!canPoison)
            return;
        source.PlayOneShot(HypnoClips[0]);

        canPoison = false;

        if (targetObject.GetComponent<Zombie>() != null)
        {
            Debug.Log("Poisoning zombie");
            targetObject.GetComponent<Zombie>().BePoisoned();
        }

        this.transform.GetComponent<Plant>().Hit(100, 1);

    }

}
