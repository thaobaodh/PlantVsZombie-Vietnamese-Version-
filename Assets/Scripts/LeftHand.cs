using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < 50; i++)
        {
            transform.position += new Vector3(0, -0.0005f, 0);
        }
    }
}
