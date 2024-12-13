using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ZombieType", menuName = "Zombie")]
public class ZombieType : ScriptableObject
{
    public string zName;
    
    public int health;
    
    public float speed;
    
    public int damage;
    
    public float range = .5f;
    
    public float eatCooldown = 1f;

    public GameObject model;

    public GameObject tool;

    public AudioClip[] hitClips;

}
