using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeBean : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] shootClips;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
        Invoke("DestroyCoffee", 1);
    }

    private void DestroyCoffee()
    {
        GetComponent<Plant>().Hit(1000, 0);
    }

}
