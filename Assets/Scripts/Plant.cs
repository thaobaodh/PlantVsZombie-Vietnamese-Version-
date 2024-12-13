using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Plant : MonoBehaviour
{
    public int health;

    public Tile tile;

    private GameObject[] targets;

    [SerializeField] float range;

    [SerializeField] LayerMask shootMask;


    private void Start()
    {
        gameObject.layer = 9;
    }


    public void Hit(int damage, int timeDisappear)
    {
        health -= damage;
        if (health <= 0)
        {
            if(tile.isPool)
            {
                if (gameObject.name == "Lilypad(Clone)" || gameObject.name == "SeaShroom(Clone)" || gameObject.name == "TangleKelp(Clone)" || gameObject.name == "Cattail(Clone)")
                {
                    tile.hasPlant = false;
                    tile.isPlantable = false;
                    tile.hasPumpkin = false;
                    tile.backgroundObject = null;
                    tile.pumpkinObject = null;
                    tile.plantObject = null;
                }
                else if(gameObject.name == "DoomShroom(Clone)")
                {
                    Debug.Log("DoomShroom actived" + " - " + tile.name);
                    tile.hasPlant = true;
                    tile.isPlantable = false;
                    //Destroy(tile.plantObject);
                    Destroy(tile.pumpkinObject);
                    Destroy(tile.backgroundObject);
                    tile.plantObject = null;
                    tile.pumpkinObject = null;
                    tile.backgroundObject = null;
                    tile.hasPumpkin = false;
                    tile.isUnavailable = true;
                }
                else if (gameObject.name == "Pumpkin(Clone)")
                {
                    tile.hasPlant = true;
                    tile.isPlantable = true;
                    tile.hasPumpkin = false;
                    tile.isUnavailable = false;
                    tile.pumpkinObject = null;
                }
                else if (gameObject.name == "Jalapeno(Clone)" || gameObject.name == "Squash(Clone)" || gameObject.name == "CherryBomb(Clone)")
                {
                    tile.hasPlant = true;
                    tile.isPlantable = true;
                    tile.isUnavailable = false;
                }
                else 
                {
                    if(damage == 100)
                    {
                        tile.hasPlant = false;
                        tile.isPlantable = false;
                        tile.plantObject = null;
                        tile.backgroundObject = null;
                        tile.pumpkinObject = null;
                        tile.isUnavailable = false;
                    }
                    else
                    {
                        if(tile.plantObject != null && tile.backgroundObject != null)
                        {
                            if (tile.plantObject.name == tile.backgroundObject.name)
                            {
                                tile.hasPlant = false;
                                tile.isPlantable = false;
                                tile.backgroundObject = null;
                            }
                            else
                            {
                                tile.hasPlant = true;
                                tile.isPlantable = true;
                            }
                            tile.plantObject = null;
                            tile.isUnavailable = false;
                        }
                        else
                        {
                            tile.hasPlant = true;
                            tile.isPlantable = true;
                            tile.plantObject = null;
                            tile.isUnavailable = false;
                        }
                    }
                }
            }
            else if (tile.isGround)
            {
                if(gameObject.name == "Cobcannon(Clone)")
                {
                    targets = GameObject.FindGameObjectsWithTag("Tile");
                    if (targets == null)
                    {

                    }
                    else
                    {
                        foreach (GameObject _target in targets)
                        {
                            if (_target.GetComponent<Tile>() != null)
                            {
                                // Debug.Log(Convert.ToInt32(tile.name) + "---" + Convert.ToInt32(_target.GetComponent<Tile>().name));
                                tile.hasPlant = false;
                                tile.isPlantable = true;
                                tile.plantObject = null;
                                if (Convert.ToInt32(_target.GetComponent<Tile>().name) == Convert.ToInt32(tile.name) - 1)
                                {
                                    _target.GetComponent<Tile>().hasPlant = false;
                                    _target.GetComponent<Tile>().isPlantable = true;
                                    _target.GetComponent<Tile>().plantObject = null;
                                }
                            }
                        }
                    }
                }
                else if (gameObject.name == "DoomShroom(Clone)")
                {
                    Debug.Log("DoomShroom actived" + " - " + tile.name);
                    tile.hasPlant = true;
                    tile.isPlantable = false;
                    //Destroy(tile.plantObject);
                    Destroy(tile.pumpkinObject);
                    Destroy(tile.backgroundObject);
                    tile.plantObject = null;
                    tile.pumpkinObject = null;
                    tile.backgroundObject = null;
                    tile.hasPumpkin = false;
                    tile.isUnavailable = true;
                }
                else if(gameObject.name == "Pumpkin(Clone)")
                {
                    tile.hasPlant = false;
                    tile.isPlantable = true;
                    tile.hasPumpkin = false;
                }
                else if(gameObject.name == "Plantern(Clone)")
                {
                    tile.hasPlant = false;
                    tile.isPlantable = true;
                    tile.hasPumpkin = false;
                }
                else
                {
                    //tile.hasPlant = false;
                    //tile.isPlantable = true;
                    //tile.plantObject = null;
                    tile.hasPlant = false;
                    tile.isPlantable = true;
                    tile.plantObject = null;
                    tile.backgroundObject = null;
                    tile.pumpkinObject = null;
                    tile.isUnavailable = false;
                    // Debug.Log("Plant dead's name: " + gameObject.name + " / Tile :" + tile.name + " / " + tile.hasPlant);
                }
            }
            else if(tile.isRoof)
            {
                if (gameObject.name == "Pot(Clone)" || gameObject.name == "Lilypad(Clone)" || gameObject.name == "SeaShroom(Clone)" || gameObject.name == "TangleKelp(Clone)" || gameObject.name == "Cattail(Clone)")
                {
                    tile.hasPlant = false;
                    tile.isPlantable = false;
                    tile.hasPumpkin = false;
                    tile.backgroundObject = null;
                    tile.pumpkinObject = null;
                    tile.plantObject = null;
                }
                else if (gameObject.name == "DoomShroom(Clone)")
                {
                    Debug.Log("DoomShroom actived" + " - " + tile.name);
                    tile.hasPlant = true;
                    tile.isPlantable = false;
                    //Destroy(tile.plantObject);
                    Destroy(tile.pumpkinObject);
                    Destroy(tile.backgroundObject);
                    tile.plantObject = null;
                    tile.pumpkinObject = null;
                    tile.backgroundObject = null;
                    tile.hasPumpkin = false;
                    tile.isUnavailable = true;
                }
                else if (gameObject.name == "Pumpkin(Clone)")
                {
                    tile.hasPlant = true;
                    tile.isPlantable = true;
                    tile.hasPumpkin = false;
                    tile.isUnavailable = false;
                    tile.pumpkinObject = null;
                }
                else if (gameObject.name == "Jalapeno(Clone)" || gameObject.name == "Squash(Clone)" || gameObject.name == "CherryBomb(Clone)")
                {
                    tile.hasPlant = true;
                    tile.isPlantable = true;
                    tile.isUnavailable = false;
                }
                else
                {
                    tile.hasPlant = true;
                    tile.isPlantable = true;
                    tile.plantObject = null;
                    tile.isUnavailable = false;
                }
            }

            Destroy(gameObject, timeDisappear);
        }
    }
}
