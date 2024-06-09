using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    public GameObject sunObject;
    private GameManager gms;
    // Start is called before the first frame update
    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        InvokeRepeating("SpawnSun", 1, 15);
    }
    private void SpawnSun()
    {
        if (!gms.IsStart)
        {
            return;
        }
        GameObject mySun = Instantiate(sunObject, new Vector3(Random.Range(-4.4f, 6.4f), 6, -1), Quaternion.identity);
        mySun.GetComponent<Sun>().dropToYPos = Random.Range(2.5f, -4.6f);
    }
    // Update is called once per frame
    private void Update()
    {

    }
}
