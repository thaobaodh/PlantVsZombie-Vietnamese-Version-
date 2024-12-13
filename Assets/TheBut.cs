using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TheBut : MonoBehaviour
{
    public bool canBless;
    public int ActiveStep;
    public int cooldown = 20;
    public Vector3 shootDestination;
    [SerializeField] GameObject BlessingLight;
    private AudioSource source;
    [SerializeField] AudioClip[] shootClips;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        canBless = false;
        animator.SetInteger("State", 5);
        ActiveStep = 0;
        Invoke("CannonSet", 1);
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ActiveStep != 3 || !canBless)
        {
            return;
        }

        animator.SetInteger("State", 2);
        Invoke("WaitingForReload", 4);

        Invoke("Sound", 1.2f);

        ActiveStep = 0;
        canBless = false;
        Invoke("Reload", cooldown - 1);
        Invoke("ResetCooldown", cooldown);
    }
    void Sound()
    {
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
    }
    void WaitingForReload()
    {
        Instantiate(BlessingLight, shootDestination, Quaternion.identity); // Generate Cob;

        animator.SetInteger("State", 3);
    }
    void Reload()
    {
        animator.SetInteger("State", 4);
    }

    void CannonSet()
    {
        animator.SetInteger("State", 5);
        canBless = true;
        ActiveStep = 1;
        // Debug.Log("CobCannon Set");
    }
    void ResetCooldown()
    {
        animator.SetInteger("State", 5);
        canBless = true;
        ActiveStep = 1;
        // Debug.Log("CobCannon Ready");
    }
}
