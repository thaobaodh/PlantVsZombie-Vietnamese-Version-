using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TangleKelp : MonoBehaviour
{
    private bool canGrab;

    [SerializeField] float range;

    [SerializeField] GameObject grab;

    [SerializeField] LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    [SerializeField] AudioClip[] shootClips;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        canGrab = true;
    }

    private void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, range, shootMask);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                Grab();
            }
        }
    }

    void Grab()
    {
        if (!canGrab)
            return;
        canGrab = false;
        source.PlayOneShot(shootClips[0]);
        Instantiate(grab, target.transform.position, Quaternion.identity);
        target.transform.position += new Vector3(0, -0.5f, 0);
        if(target.GetComponent<Zombie>() != null)
        {
            target.GetComponent<Zombie>().Hit(999, false, false, false);
        }
        else if (target.GetComponent<TinyZombieBoat>() != null)
        {
            target.GetComponent<TinyZombieBoat>().Hit(999);
        }
        GetComponent<Plant>().Hit(100, 1);
    }
}
