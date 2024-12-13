using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : MonoBehaviour
{
    public GameObject sunObject;

    public float cooldown;
    
    private void Start()
    {
        InvokeRepeating("SpawnSun", 1, cooldown);
    }
    void SpawnSun()
    {
        GameObject mySun = Instantiate(sunObject, new Vector3(transform.position.x + Random.Range(-.5f, .5f), transform.position.y + Random.Range(0f, .5f), -1), Quaternion.identity);
        mySun.GetComponent<Sun>().dropToYPos = transform.position.y - 1;
    }
}
