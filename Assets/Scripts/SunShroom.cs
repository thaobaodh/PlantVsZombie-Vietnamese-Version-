using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SunShroom : MonoBehaviour
{
    [SerializeField] GameObject tinySunObject;

    [SerializeField] GameObject sunObject;

    [SerializeField] float cooldown;

    [SerializeField] float grownUpCooldown;

    private float cooldownTime;

    [SerializeField] bool BigSunShroom;

    private AudioSource source;

    public AudioClip[] SunClips;

    private void Start()
    {
        InvokeRepeating("SpawnSun", 1, cooldown);

        source = gameObject.AddComponent<AudioSource>();

        cooldownTime = grownUpCooldown;
    }

    private void FixedUpdate()
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
        }
        else if (cooldownTime < 0)
        {
            GrownUpSunShroom();
            cooldownTime = 0;
        }
        else
        {

        }
    }


    void GrownUpSunShroom()
    {
        BigSunShroom = true;
        source.PlayOneShot(SunClips[1]);
        gameObject.transform.localScale += new Vector3(0.5f, 0.5f, 0);
    }

    void SpawnSun()
    {
        source.PlayOneShot(SunClips[0]);
        if(BigSunShroom)
        {
            GameObject mySun = Instantiate(sunObject, new Vector3(transform.position.x + Random.Range(-.5f, .5f), transform.position.y + Random.Range(0f, .5f), -1), Quaternion.identity);
            mySun.GetComponent<Sun>().dropToYPos = transform.position.y - 1;
        }
        else
        {
            GameObject mySun = Instantiate(tinySunObject, new Vector3(transform.position.x + Random.Range(-.5f, .5f), transform.position.y + Random.Range(0f, .5f), -1), Quaternion.identity);
            mySun.GetComponent<Sun>().dropToYPos = transform.position.y - 1;
        }
    }
}
