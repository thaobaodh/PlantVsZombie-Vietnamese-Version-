using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSunFlower : MonoBehaviour
{
    [SerializeField] GameObject sunObject;

    [SerializeField] Transform leftSunSpawner;

    [SerializeField] Transform rightSunSpawner;

    [SerializeField] float cooldown;

    private void Start()
    {
        InvokeRepeating("SpawnSun", 1, cooldown);
    }
    void SpawnSun()
    {
        GameObject myLeftSun = Instantiate(sunObject, leftSunSpawner.position, Quaternion.identity);
        myLeftSun.GetComponent<Sun>().dropToYPos = transform.position.y - 1;


        GameObject myRightSun = Instantiate(sunObject, rightSunSpawner.position, Quaternion.identity);
        myRightSun.GetComponent<Sun>().dropToYPos = transform.position.y - 1;
    }
}
