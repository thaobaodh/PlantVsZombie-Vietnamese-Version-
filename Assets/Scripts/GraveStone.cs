using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveStone : MonoBehaviour
{
    [SerializeField] Sprite graveStone0;
    [SerializeField] Sprite graveStone1;
    [SerializeField] Sprite graveStone2;
    [SerializeField] Sprite graveStone3;
    [SerializeField] Sprite graveStone4;
    [SerializeField] Sprite graveStone5;
    [SerializeField] Sprite graveStone6;
    [SerializeField] Sprite graveStone7;
    [SerializeField] Sprite graveStone8;
    [SerializeField] Sprite graveStone9;
    [SerializeField] Sprite graveStone10;
    [SerializeField] Sprite graveStone11;
    [SerializeField] Sprite graveStone12;
    [SerializeField] Sprite graveStone13;
    [SerializeField] Sprite graveStone14;
    [SerializeField] Sprite graveStone15;
    [SerializeField] Sprite graveStone16;
    [SerializeField] Sprite graveStone17;
    [SerializeField] Sprite graveStone18;
    [SerializeField] Sprite graveStone19;
    [SerializeField] int health;
    [SerializeField] Transform zombieSpawnPosition;
    [SerializeField] float time = 10;
    [SerializeField] GameObject zombieObject;
    [SerializeField] GameObject[] zombieTools;

    private bool canRise = false;

    public Tile tile;

    private GameManager gms;
    // Start is called before the first frame update
    void Start()
    {
        canRise = false;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();  
        Invoke("ZombieRiseFromGraveStone", Random.Range(15, time));
        switch (Random.Range(0,19))
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = graveStone0;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = graveStone1;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = graveStone2;
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = graveStone3;
                break;
            case 4:
                GetComponent<SpriteRenderer>().sprite = graveStone4;
                break;
            case 5:
                GetComponent<SpriteRenderer>().sprite = graveStone5;
                break;
            case 6:
                GetComponent<SpriteRenderer>().sprite = graveStone6;
                break;
            case 7:
                GetComponent<SpriteRenderer>().sprite = graveStone7;
                break;
            case 8:
                GetComponent<SpriteRenderer>().sprite = graveStone8;
                break;
            case 9:
                GetComponent<SpriteRenderer>().sprite = graveStone9;
                break;
            case 10:
                GetComponent<SpriteRenderer>().sprite = graveStone10;
                break;
            case 11:
                GetComponent<SpriteRenderer>().sprite = graveStone11;
                break;
            case 12:
                GetComponent<SpriteRenderer>().sprite = graveStone12;
                break;
            case 13:
                GetComponent<SpriteRenderer>().sprite = graveStone13;
                break;
            case 14:
                GetComponent<SpriteRenderer>().sprite = graveStone14;
                break;
            case 15:
                GetComponent<SpriteRenderer>().sprite = graveStone15;
                break;
            case 16:
                GetComponent<SpriteRenderer>().sprite = graveStone16;
                break;
            case 17:
                GetComponent<SpriteRenderer>().sprite = graveStone17;
                break;
            case 18:
                GetComponent<SpriteRenderer>().sprite = graveStone18;
                break;
            case 19:
                GetComponent<SpriteRenderer>().sprite = graveStone19;
                break;
            default:
                GetComponent<SpriteRenderer>().sprite = graveStone0;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gms.IsStart) return;
        if (!canRise) return;
        if(!gms.isZombieSummonInGraveStone) return;
        GameObject myZombie = Instantiate(zombieObject, zombieSpawnPosition.position, Quaternion.identity);
        myZombie.GetComponent<Zombie>().lane = tile.GetComponent<Tile>().lane;
        canRise = false;
        Invoke("ZombieRiseFromGraveStone", Random.Range(15,time));
    }

    void ZombieRiseFromGraveStone()
    {
        canRise = true;
    }

    public void GraveHit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            tile.hasPlant = false;
            Destroy(gameObject, 0.5f);
        }
    }
}
