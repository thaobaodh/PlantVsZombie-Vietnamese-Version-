using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusSage : MonoBehaviour
{
    public string lane;

    public GameObject flowerSmoke;

    public Transform shootOrigin;

    public float cooldown;

    private bool canFlowering = false;

    public float range;

    public LayerMask shootMask;

    private GameObject target;

    private AudioSource source;

    public AudioClip[] shootClips;

    private GameManager gms;

    private void Start()
    {
        canFlowering = true;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!canFlowering)
            return;
        Invoke("Flowering", 1);
        canFlowering = false; 
    }

    //void ResetCooldown()
    //{
    //    canFlowering = true;
    //}

    void Flowering()
    {
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);

        GameObject myFlowering = Instantiate(flowerSmoke, shootOrigin.position, Quaternion.identity);

        gameObject.GetComponent<Plant>().Hit(100, 2);

        //Invoke(nameof(ResetCooldown), cooldown);
    }
}
