using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSmoke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            if(zombie.getZombieStatus())
            {
                zombie.Recover();
            }
            else
            {
                zombie.BePoisoned();
            }
        }
    }
}
