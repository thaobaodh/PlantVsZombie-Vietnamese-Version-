using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class ZombieDeathTrigger : MonoBehaviour
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
        if (other.gameObject.layer == 9)
        {
            if(other.GetComponent<Zombie>() != null)
            {
                other.GetComponent<Zombie>().Hit(100, false, false, false);
            }
            else
            {

            }
        }
        else  if(other.gameObject.name == "RunnablePotatominer(Clone)")
        {
            other.GetComponent<Plant>().Hit(100, 0);
        }
    }
}
