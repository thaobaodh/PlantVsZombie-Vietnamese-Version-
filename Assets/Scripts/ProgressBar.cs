using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private GameManager gms;
 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ProgressBar is disappear");
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.SetActive(false);gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("ProgressBar");
        if (gms.StartSpawningZombie)
        {
            Debug.Log("ProgressBar appear");
            gameObject.SetActive(true);
        }
    }
}
