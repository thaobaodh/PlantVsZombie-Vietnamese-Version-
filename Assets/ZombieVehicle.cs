using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieVehicle : MonoBehaviour
{
    public string lane;

    public int health;

    private GameManager gms;

    public bool dead = false;

    private void Start()
    {
        dead = false;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {

    }

    void Hit(int damage)
    {
        if (dead) return;
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                dead = true;
                Destroy(gameObject, 1f);
            }
        }
    }
}
