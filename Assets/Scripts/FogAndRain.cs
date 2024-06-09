using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogAndRain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0,2) == 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (Random.Range(0, 2) == 1)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(2).gameObject.SetActive(false);

        }
        else if (Random.Range(0, 2) == 2)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
