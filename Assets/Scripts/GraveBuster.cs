using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class GraveBuster : MonoBehaviour
{
    public int damage;

    public bool canEat;

    public float range;

    public LayerMask ExplodeMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] ExplodeClips;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, ExplodeMask);
        if (hit.collider)
        {
            target = hit.collider.gameObject;
            GraveStoneDestroy();
        }
    }

    void GraveStoneDestroy()
    {
        if (!canEat)
            return;
        source.PlayOneShot(ExplodeClips[0]);

        canEat = false;

        if (target.GetComponent<GraveStone>() != null)
        {
            // Debug.Log("GraveStone Destroy");
            target.GetComponent<GraveStone>().GraveHit(damage);
        }
        Invoke("Cooldown", 1);
        Invoke("CooldownDestroy", 3);
    }
    void Cooldown()
    {
        canEat = true;
    }
    void CooldownDestroy()
    {
        GetComponent<Plant>().Hit(100, 0);
    }
}
