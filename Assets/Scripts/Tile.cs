using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string lane;

    public bool isUnavailable;

    public bool hasPlant;

    public bool hasPumpkin;

    public bool isGround;

    public bool isPool;

    public bool isRoof;

    public int cooldown;

    public int cooldownRemaining;

    public bool isPlantable;

    public Sprite DayGroundHole1;

    public Sprite DayGroundHole2;

    public Sprite NightGroundHole1;

    public Sprite NightGroundHole2;

    public Sprite DayPoolHole1;

    public Sprite DayPoolHole2;

    public Sprite NightPoolHole1;

    public Sprite NightPoolHole2;

    public Sprite DayRoofHole1;

    public Sprite DayRoofHole2;

    public Sprite NightRoofHole1;

    public Sprite NightRoofHole2;

    public GameObject plantObject;

    public GameObject pumpkinObject;

    public GameObject backgroundObject;

    private Vector3 originPosition;

    private bool goUporDown = false;
    private void ResetCooldown()
    {
        Debug.Log("Available to plant");
        if (isGround)
        {
            hasPlant = false;
            isPlantable = true;
        }
        else if (isPool)
        {
            hasPlant = false;
            isPlantable = false;
        }
        else if (isRoof)
        {
            hasPlant = false;
            isPlantable = false;
        }
        isUnavailable = false;

        GetComponent<SpriteRenderer>().sprite = null;

    }
    private void Recovering()
    {
        Debug.Log("Recovering");
        if (isPool)
        {
            GetComponent<SpriteRenderer>().sprite = DayPoolHole2;
        }
        else if (isGround)
        {
            GetComponent<SpriteRenderer>().sprite = DayGroundHole2;
        }
        else if (isRoof)
        {
            GetComponent<SpriteRenderer>().sprite = DayRoofHole2;
        }
    }
    private void Start()
    {
        if(Random.Range(0,5) % 2 == 0)
        {
            goUporDown = true;
        }
        else
        {
            goUporDown = false;
        }
        originPosition = transform.position;
        cooldownRemaining = cooldown;
        isUnavailable = false;
    }
    private void FixedUpdate()
    {
        if(isPool)
        {
            if (backgroundObject != null)
            {
                if (goUporDown)
                {
                    backgroundObject.transform.position += new Vector3(0f, 0.1f * Time.fixedDeltaTime, 0f);
                    if (backgroundObject.transform.position.y > originPosition.y + 0.2f)
                    {
                        goUporDown = false;
                    }
                }
                else
                {
                    backgroundObject.transform.position -= new Vector3(0f, 0.1f * Time.fixedDeltaTime, 0f);
                    if (backgroundObject.transform.position.y < originPosition.y - 0.2f)
                    {
                        goUporDown = true;
                    }
                }

                if (plantObject != null)
                {
                    plantObject.transform.position = backgroundObject.transform.position;
                }
                if (pumpkinObject != null)
                {
                    pumpkinObject.transform.position = backgroundObject.transform.position;
                }
            }
        }

        if (!isUnavailable)
            return;
        if (isPool)
        {
            GetComponent<SpriteRenderer>().sprite = DayPoolHole1;
        }
        else if (isGround)
        {
            GetComponent<SpriteRenderer>().sprite = DayGroundHole1;
        }
        else if (isRoof)
        {
            GetComponent<SpriteRenderer>().sprite = DayRoofHole1;
        }
        Invoke("ResetCooldown", cooldown);
        Invoke("Recovering", cooldown / 2);
        Debug.Log("Set cooldown");
        isUnavailable = false;
    }
}
