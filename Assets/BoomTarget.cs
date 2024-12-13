using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Boom(Clone)")
        {
            Destroy(gameObject, 0.25f);
        }
        else if(other.name == "Missle(Clone)")
        {
            Destroy(gameObject, 0.25f);
        }
    }
}
