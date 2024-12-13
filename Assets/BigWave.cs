using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWave : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] int damage = 5;
    public bool isReverse = false;
    public bool justKillOneZombie = true;
    private bool isReady = false;
    public bool waveType;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if(waveType)
        {
            Destroy(gameObject, 7);
            Invoke("IsReadyToMove", 0.5f);
        }
        else
        {
            Invoke("IsReadyToMove", 2.2f);
        }
        
    }

    private void IsReadyToMove()
    {
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady) return;
        if (isReverse)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Tile>(out Tile tile))
        {
            if (tile.plantObject != null)
            {
                if (tile.plantObject.name == "Tallnut(Clone)")
                {
                    // Debug.Log("Trigger: " + tile.plantObject.name);

                    tile.plantObject.GetComponent<Plant>().Hit(damage, 1);
                    speed = 0;
                    Destroy(gameObject, 1);
                    return;
                }
                else if (tile.plantObject.name == "Wallnut(Clone)")
                {
                    // Debug.Log("Trigger: " + tile.plantObject.name);

                    tile.plantObject.GetComponent<Plant>().Hit(100, 1);
                    speed = 0;
                    Destroy(gameObject, 1);
                    return;
                }
                else
                {
                    tile.plantObject.GetComponent<Plant>().Hit(100, 1);
                }
            }

            if (tile.backgroundObject != null)
            {
                tile.backgroundObject.GetComponent<Plant>().Hit(100, 1);
            }

            if (tile.pumpkinObject != null)
            {
                tile.pumpkinObject.GetComponent<Plant>().Hit(100, 1);
            }


            if (justKillOneZombie)
            {
                Destroy(gameObject, 2.2f);
            }
            else
            {

            }


        }
    }
}
