using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileField : MonoBehaviour
{
    [SerializeField] int damage = 50;
    [SerializeField] int range = 1;
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
        if(other.TryGetComponent<TinyZombieBoat>(out TinyZombieBoat tinyZombieBoat))
        {
            tinyZombieBoat.Hit(damage);
        }
        else if (other.TryGetComponent<ZombieBoat>(out ZombieBoat zomBoat))
        {
            zomBoat.Hit(damage);
        }
    }
}
