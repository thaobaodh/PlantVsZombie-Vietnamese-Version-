using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoSmash : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip[] explodedSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.PlayOneShot(explodedSound[0]);
        Destroy(gameObject, 1);
    }
}
