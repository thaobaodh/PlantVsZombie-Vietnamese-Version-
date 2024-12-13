using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;
public class PlantSlot : MonoBehaviour
{
    public bool isSelected = false;

    public Sprite plantSprite;

    public GameObject plantObject;

    public PlantSlot plantSlot;

    public Image icon;

    public Image cooldownImage;

    public float cooldown;

    public bool isCoolingdown;

    public int price = 100; 

    public TextMeshProUGUI priceText;

    private GameManager gms;

    [Tooltip("X: MaxHeight , Y: Min Height")]

    public Vector2 height;

    private string lastPlantCardName;

    private int count = 0;

    private void Start()
    {
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        GetComponent<Button>().onClick.AddListener(BuyPlant);
    }

    public void BuyPlant()
    {
        if (gms.IsStart)
        {
            if (isCoolingdown)
            {
                return;
            }

            if (gms.suns >= price && !gms.currentPlant)
            {
                selectedPlant();
                gms.BuyPlant(gameObject, plantObject, plantSprite, price);
                isSelected = true;
            }
        }
        else
        {
            if (isSelected)
            {
                if (plantSlot != null)
                {
                    gms.numOfPlantsAiming -= 1;
                    lastPlantCardName = gameObject.GetComponent<PlantSlot>().name;

                    for (int i = 0; i < gms.selectedPlant.Count; i++)
                    {
                        if (gms.selectedPlant[i].name == lastPlantCardName)
                        {
                            gameObject.GetComponent<PlantSlot>().plantSlot.transform.GetChild(3).gameObject.SetActive(false);
                            gameObject.GetComponent<PlantSlot>().plantSlot.isSelected = false;
                            gms.selectedPlant.RemoveAt(i);
                        }
                    }
                    gms.BuyPlant(null, null, null, 0);
                }
                else
                {

                }

            }
            else
            {
                if (isCoolingdown)
                {
                    return;
                }

                if (gms.suns >= price && !gms.currentPlant)
                {
                    gms.BuyPlant(gameObject, plantObject, plantSprite, price);
                }
            }

        }
    }
    private void Update()
    {
        if (gms.IsStart)
        {
            if (isCoolingdown)
            {
                if (count > 1)
                    return;
                count++;
                StartCoroutine(CardCooldown(cooldown));
            }
        }
    }

    private void OnValidate()
    {
        if(plantSprite)
        {
            icon.enabled = true;
            icon.sprite = plantSprite;
            priceText.text = price.ToString();
        }
        else
        {
            icon.enabled = false;
        }
    }
    public void selectedPlant()
    {
        cooldownImage.rectTransform.anchoredPosition = new Vector2(0, 0);
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
    }

    public IEnumerator CardCooldown(float cooldown)
    {
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
        isCoolingdown = true;
        for(float i = height.x; i < height.y; i++)
        {
            cooldownImage.rectTransform.anchoredPosition = new Vector2(0, i);
            yield return new WaitForSeconds(cooldown / height.y);
        }
        isCoolingdown = false;
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

}
