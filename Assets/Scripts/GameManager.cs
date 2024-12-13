using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using static UnityEngine.GraphicsBuffer;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Pool;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Data;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public bool isZombieSummonInGraveStone = false;

    [SerializeField] int hammerDamage = 20;

    public bool SmashMouseMode;

    public Texture2D hammer;

    public Texture2D hammerDown;

    public bool notPreviewZombie = false;

    public bool IsStart = false;

    public bool Shovel = false;

    public bool Day;

    public bool Night;

    public bool Roof;

    public bool Pool;

    public bool isFogAndRain;

    public bool isVaseBreaker;

    public int numOfGraveStones;

    public GameObject graveStones;

    public GameObject potObject;

    public List<GameObject> wallObjectList;

    public List<GameObject> vaseObjectList;

    public List<Tile> summonTileList;

    public bool needToBuildCastle;

    public GameObject lilyPadObject;

    public GameObject pileFieldObject;

    public GameObject currentPlant;

    public Sprite currentPlantSprite;

    private int currentPrice;

    private PlantSlot currentPlantSlot;

    public Transform tiles;

    public LayerMask zombieMask;

    public LayerMask tileMask;

    public int suns;

    public TextMeshProUGUI sunText;

    public LayerMask plantMask;

    public LayerMask sunMask;

    public AudioClip plantSFX;

    private AudioSource source;

    public AudioClip sunSFX;

    public AudioSource sunSource;

    public bool MountainGodTarget = false;

    public bool TheButTarget = false;

    public bool CobcannonTarget = false;

    public Vector3 MountainGodTargetLocation;

    public Vector3 TheButTargetLocation;

    public Vector3 cobcannonTargetLocation;

    private GameObject MountainGodSelected;

    private GameObject TheButSelected;

    private GameObject CobCannonSelected;

    private GameObject[] targets;

    private float mouseLimitXmin = 476;

    private float mouseLimitXmax = 1796;

    private float mouseLimitYmin = 10;

    private float mouseLimitYmax = 920;

    private float realLimitXmin = -4.8f;

    private float realLimitXmax = 8.45f;

    private float realLimitYmin = -5.32f;

    private float realLimitYmax = 3.8f;

    public bool StartSpawningZombie = false;

    private Tile unavailableTile;

    [Header("Select Plant Slot in start game UI")]

    public List<PlantSlot> selectedPlant;

    public List<PlantSlot> staticPlant; // remove in selectedPlant and copy staticPlant to selectedPlant

    public PlantSlot samplePlant;

    [SerializeField] int MaxPlantSlots;

    [SerializeField] Button LetsRockButton;

    [SerializeField] Button shovelSlot;

    [SerializeField] Sprite shovelSprite;

    [SerializeField] GameObject progressBarObject;

    public int numOfPlantsAiming;

    private Button btn;

    private Button btnShovel;

    [SerializeField] List<int> graveStoneSpawnListPosition;

    [SerializeField] List<int> graveStoneSpawnPosition;

    [SerializeField] int vaseTotal = 35;
    [SerializeField] int vasePlant = 2;
    [SerializeField] int vaseBoss = 1;

    [SerializeField] int countVasePlant = 0;
    [SerializeField] int countVaseBoss = 0;
    [SerializeField] int countVaseMystic = 0;

    [SerializeField] List<GameObject> bossObjectsList;
    [SerializeField] List<GameObject> zombieObjectsList;
    [SerializeField] List<GameObject> plantObjectsList;

    [SerializeField] List<int> SpawnPositionList;
    [SerializeField] List<GameObject> RandomObjectSpawnList;

    private GameObject objectInVase;

    private float timeOut = 0;
    private void generateRandomPosition()
    {
        int count = 0;
        int Position = 0;
        for (int i = 0; i < 45; i++)
        {
            graveStoneSpawnListPosition.Add(i);
        }

        while (count < numOfGraveStones)
        {
            Position = graveStoneSpawnListPosition[Random.Range(0, graveStoneSpawnListPosition.Count)];
            if (Position % 9 >= 4)
            {
                Debug.Log("count : " + count + " / " + Position);
                count++;
                graveStoneSpawnPosition.Add(Position);
                graveStoneSpawnListPosition.Remove(Position);
            }
            else
            {

            }
        }
    }

    private void generateRandomVasePosition(int numOfVase)
    {
        int count = 0;
        int Position = 0;
        if(numOfVase == 1)
        {
            for (int i = 0; i < 45; i++)
            {
                graveStoneSpawnListPosition.Add(i);
            }
        }
        else
        {

        }

        while (count < numOfVase)
        {
            Position = graveStoneSpawnListPosition[Random.Range(0, graveStoneSpawnListPosition.Count)];
            if (Position % 9 >= 2)
            {
                // Debug.Log("count : " + count + " / " + Position);
                count++;
                SpawnPositionList.Add(Position);
                graveStoneSpawnPosition.Add(Position);
                graveStoneSpawnListPosition.Remove(Position);
            }
            else
            {

            }
        }
    }

    private Vector3 convertMousePos2RealPos()
    {
        Vector3 realVector3 = Vector3.zero;
        realVector3.x = (((cobcannonTargetLocation.x - mouseLimitXmin) * (realLimitXmax - realLimitXmin)) / (mouseLimitXmax - mouseLimitXmin)) + realLimitXmin;
        realVector3.y = (((cobcannonTargetLocation.y - mouseLimitYmin) * (realLimitYmax - realLimitYmin)) / (mouseLimitYmax - mouseLimitYmin)) + realLimitYmin;
        return realVector3;
    }

    private Vector3 convertTheButMousePos2RealPos()
    {
        Vector3 realVector3 = Vector3.zero;
        realVector3.x = (((TheButTargetLocation.x - mouseLimitXmin) * (realLimitXmax - realLimitXmin)) / (mouseLimitXmax - mouseLimitXmin)) + realLimitXmin;
        realVector3.y = (((TheButTargetLocation.y - mouseLimitYmin) * (realLimitYmax - realLimitYmin)) / (mouseLimitYmax - mouseLimitYmin)) + realLimitYmin;
        return realVector3;
    }

    void TaskOnClick()
    {
        Debug.Log("Plant!");
        IsStart = true;
        if (!notPreviewZombie)
        {
            targets = GameObject.FindGameObjectsWithTag("Zombie");
            if (targets == null)
            {

            }
            else
            {
                foreach (GameObject _target in targets)
                {
                    _target.GetComponent<Zombie>().Hit(997, false, false, false);
                }
            }
        }
    }


    private void SummonVase(int num)
    {
        targets = GameObject.FindGameObjectsWithTag("Tile");
        if (targets == null)
        {

        }
        else
        {
            foreach (GameObject _target in targets)
            {
                for (int i = 0; i < graveStoneSpawnPosition.Count; i++)
                {
                    if ((int.Parse(_target.gameObject.name) == graveStoneSpawnPosition[i]))
                    {
                        if(num == 0)
                        {
                            if(Random.Range(0, 5) >= 2)
                            {
                                objectInVase = plantObjectsList[Random.Range(0, plantObjectsList.Count)];
                                RandomObjectSpawnList.Add(objectInVase);
                            }
                            else
                            {
                                objectInVase = zombieObjectsList[Random.Range(0, zombieObjectsList.Count)];
                                RandomObjectSpawnList.Add(objectInVase);
                            }
                        }
                        else if(num == 1)
                        {
                            objectInVase = plantObjectsList[Random.Range(0, plantObjectsList.Count)];
                            RandomObjectSpawnList.Add(objectInVase);
                        }
                        else if(num == 2)
                        {
                            objectInVase = bossObjectsList[Random.Range(0, bossObjectsList.Count)];
                            RandomObjectSpawnList.Add(objectInVase);
                        }

                        GameObject vaseObject = Instantiate(vaseObjectList[num], _target.GetComponent<Tile>().transform.position, Quaternion.identity);
                        vaseObject.GetComponent<Vase>().objectInVase = objectInVase;
                        _target.GetComponent<Tile>().plantObject = vaseObject;
                        _target.GetComponent<Tile>().hasPlant = true;

                        graveStoneSpawnPosition.RemoveAt(i);
                    }
                }
            }
        }
    }

    void TakeShovel()
    {
        Debug.Log("Take a shovel!");
        Shovel = !Shovel;
    }

    private void Start()
    {
        if (Night)
        {
            generateRandomPosition();
        }

        if(isVaseBreaker)
        {
            Cursor.SetCursor(hammer, Vector2.zero, CursorMode.ForceSoftware);
            vasePlant = Random.Range(2, 5);
            int vaseMystic = vaseTotal - (vaseBoss + vasePlant);
            generateRandomVasePosition(vaseBoss);
            SummonVase(2);
            generateRandomVasePosition(vasePlant);
            SummonVase(1);
            generateRandomVasePosition(vaseMystic);
            SummonVase(0);
        }

        if (SmashMouseMode)
        {
            Cursor.SetCursor(hammer, Vector2.zero, CursorMode.ForceSoftware);
        }

        source = GetComponent<AudioSource>();
        btn = LetsRockButton.GetComponent<Button>();
        btnShovel = shovelSlot.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        btnShovel.onClick.AddListener(TakeShovel);

        progressBarObject.SetActive(false);
        if (Night)
        {
            targets = GameObject.FindGameObjectsWithTag("Tile");
            if (targets == null)
            {

            }
            else
            {
                foreach (GameObject _target in targets)
                {
                    for (int i = 0; i < graveStoneSpawnPosition.Count; i++)
                    {
                        if ((int.Parse(_target.gameObject.name) == graveStoneSpawnPosition[i]))
                        {
                            graveStones.GetComponent<GraveStone>().tile = _target.GetComponent<Tile>();
                            Instantiate(graveStones, _target.GetComponent<Tile>().transform.position, Quaternion.identity);
                            _target.GetComponent<Tile>().plantObject = graveStones.gameObject;
                            _target.GetComponent<Tile>().hasPlant = true;
                        }
                    }
                }
            }
        }

        //graveStoneSpawnListPosition.Clear();
        //graveStoneSpawnPosition.Clear();

        if (Roof)
        {
            targets = GameObject.FindGameObjectsWithTag("Tile");
            if (targets == null)
            {

            }
            else
            {
                foreach (GameObject _target in targets)
                {
                    if (int.Parse(_target.GetComponent<Tile>().name) % 9 < 5)
                    {
                        JustSetObject(_target, potObject);
                    }
                }
            }
        }
        else if (Pool)
        {
            targets = GameObject.FindGameObjectsWithTag("Tile");
            if (targets == null)
            {

            }
            else
            {
                foreach (GameObject _target in targets)
                {
                    if (int.Parse(_target.GetComponent<Tile>().name) % 9 < 5)
                    {
                        JustSetObject(_target, lilyPadObject);
                    }
                    else if (int.Parse(_target.GetComponent<Tile>().name) % 9 >= 5 && int.Parse(_target.GetComponent<Tile>().name) % 9 < 8)
                    {
                        JustSetObject(_target, pileFieldObject);
                    }
                }
            }
        }
        else if (needToBuildCastle)
        {
            foreach (Tile currentTile in summonTileList)
            {
                JustSetObject(currentTile.gameObject, wallObjectList[Random.Range(0, wallObjectList.Count)]);
            }
        }
        else if (isVaseBreaker)
        {

        }
    }

    public void Win()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
            return;
        }
        PlayerPrefs.SetInt("levelSave", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BuyPlant(GameObject PlantSlot, GameObject plant, Sprite sprite, int price)
    {
        if (!IsStart)
        {
            if (PlantSlot == null && plant == null && sprite == null && price == 0)
            {
                for (int i = 0; i < staticPlant.Count; i++)
                {
                    btn.interactable = false;

                    if (i < selectedPlant.Count)
                    {
                        staticPlant[i].gameObject.SetActive(true);
                        staticPlant[i].gameObject.name = selectedPlant[i].gameObject.name;
                        staticPlant[i].GetComponent<PlantSlot>().plantSprite = selectedPlant[i].GetComponent<PlantSlot>().plantSprite;
                        staticPlant[i].GetComponent<PlantSlot>().plantObject = selectedPlant[i].GetComponent<PlantSlot>().plantObject;
                        staticPlant[i].GetComponent<PlantSlot>().cooldown = selectedPlant[i].GetComponent<PlantSlot>().cooldown;
                        staticPlant[i].GetComponent<PlantSlot>().price = selectedPlant[i].GetComponent<PlantSlot>().price;
                        staticPlant[i].GetComponent<PlantSlot>().icon.enabled = true;
                        staticPlant[i].GetComponent<PlantSlot>().icon.sprite = selectedPlant[i].GetComponent<PlantSlot>().icon.sprite;
                        staticPlant[i].GetComponent<PlantSlot>().priceText.text = selectedPlant[i].GetComponent<PlantSlot>().priceText.text;
                        staticPlant[i].GetComponent<PlantSlot>().isSelected = true;
                        staticPlant[i].GetComponent<PlantSlot>().plantSlot = selectedPlant[i].GetComponent<PlantSlot>();
                    }
                    else
                    {
                        staticPlant[i].gameObject.SetActive(false);
                        staticPlant[i].gameObject.name = i.ToString();
                        staticPlant[i].GetComponent<PlantSlot>().plantSprite = null;
                        staticPlant[i].GetComponent<PlantSlot>().plantObject = null;
                        staticPlant[i].GetComponent<PlantSlot>().cooldown = 0;
                        staticPlant[i].GetComponent<PlantSlot>().price = 0;
                        staticPlant[i].GetComponent<PlantSlot>().icon.enabled = false;
                        staticPlant[i].GetComponent<PlantSlot>().icon.sprite = null;
                        staticPlant[i].GetComponent<PlantSlot>().priceText.text = "0";
                        staticPlant[i].GetComponent<PlantSlot>().isSelected = false;
                        staticPlant[i].GetComponent<PlantSlot>().plantSlot = null;
                    }
                }
            }
            else
            {
                if (numOfPlantsAiming < MaxPlantSlots)
                {
                    numOfPlantsAiming++;
                    PlantSlot.GetComponent<PlantSlot>().gameObject.transform.GetChild(3).gameObject.SetActive(true);
                    currentPlantSlot = PlantSlot.GetComponent<PlantSlot>();
                    // Debug.Log("Plant Slot selected: " + currentPlantSlot.name);
                    currentPlant = plant;
                    currentPlantSprite = sprite;
                    currentPrice = price;
                    currentPlantSlot.isSelected = true;
                    selectedPlant.Add(currentPlantSlot);

                    for (int i = 0; i < staticPlant.Count; i++)
                    {
                        if (i < selectedPlant.Count)
                        {
                            staticPlant[i].gameObject.SetActive(true);
                            staticPlant[i].gameObject.name = selectedPlant[i].gameObject.name;
                            staticPlant[i].GetComponent<PlantSlot>().plantSprite = selectedPlant[i].GetComponent<PlantSlot>().plantSprite;
                            staticPlant[i].GetComponent<PlantSlot>().plantObject = selectedPlant[i].GetComponent<PlantSlot>().plantObject;
                            staticPlant[i].GetComponent<PlantSlot>().cooldown = selectedPlant[i].GetComponent<PlantSlot>().cooldown;
                            staticPlant[i].GetComponent<PlantSlot>().price = selectedPlant[i].GetComponent<PlantSlot>().price;
                            staticPlant[i].GetComponent<PlantSlot>().icon.enabled = true;
                            staticPlant[i].GetComponent<PlantSlot>().icon.sprite = selectedPlant[i].GetComponent<PlantSlot>().icon.sprite;
                            staticPlant[i].GetComponent<PlantSlot>().priceText.text = selectedPlant[i].GetComponent<PlantSlot>().priceText.text;
                            staticPlant[i].GetComponent<PlantSlot>().isSelected = true;
                            staticPlant[i].GetComponent<PlantSlot>().plantSlot = selectedPlant[i].GetComponent<PlantSlot>();
                        }
                        else
                        {
                            staticPlant[i].gameObject.SetActive(false);
                            staticPlant[i].gameObject.name = i.ToString();
                            staticPlant[i].GetComponent<PlantSlot>().plantSprite = null;
                            staticPlant[i].GetComponent<PlantSlot>().plantObject = null;
                            staticPlant[i].GetComponent<PlantSlot>().cooldown = 0;
                            staticPlant[i].GetComponent<PlantSlot>().price = 0;
                            staticPlant[i].GetComponent<PlantSlot>().icon.enabled = false;
                            staticPlant[i].GetComponent<PlantSlot>().icon.sprite = null;
                            staticPlant[i].GetComponent<PlantSlot>().priceText.text = "0";
                            staticPlant[i].GetComponent<PlantSlot>().isSelected = false;
                            staticPlant[i].GetComponent<PlantSlot>().plantSlot = null;
                        }
                    }
                    currentPlant = null;
                    currentPlantSprite = null;
                    if (numOfPlantsAiming >= MaxPlantSlots)
                    {
                        btn.interactable = true;
                    }
                    else
                    {
                        btn.interactable = false;
                    }
                }
                else
                {
                    //Debug.Log("Maximum Plant Slot to Aim");
                }
            }
        }
        else
        {
            PlantSlot.GetComponent<PlantSlot>().gameObject.transform.GetChild(3).gameObject.SetActive(true);
            currentPlantSlot = PlantSlot.GetComponent<PlantSlot>();
            // Debug.Log("Plant Slot selected: " + currentPlantSlot.name);
            currentPlant = plant;
            currentPlantSprite = sprite;
            currentPrice = price;
        }
    }

    void SelectDestination()
    {
        CobCannonSelected.GetComponent<CobCannon>().ActiveStep = 2; //Wait for select shoot destination;
    }

    void SelectTheButDestination()
    {
        TheButSelected.GetComponent<TheBut>().ActiveStep = 2; //Wait for select shoot destination;
    }

    void SelectMountainGodDestination()
    {
        MountainGodSelected.GetComponent<MoutainGod>().ActiveStep = 2; //Wait for select shoot destination;
    }
    void HammerAgain()
    {
        Cursor.SetCursor(hammer, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void Update()
    {
        if (isVaseBreaker)
        {
            RaycastHit2D vaseActiveHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, plantMask);

            if (vaseActiveHit.collider)
            {
                if (Input.GetMouseButton(0))
                {
                    timeOut += Time.deltaTime;
                    // Debug.Log("Time out: " + timeOut);
                    if (vaseActiveHit.collider.GetComponent<Vase>() != null)
                    {
                        if (vaseActiveHit.collider.GetComponent<Vase>().isBreak) return;
                        if (timeOut > 0.1f)
                        {
                            Debug.Log("Hit Vase");
                            Cursor.SetCursor(hammerDown, Vector2.zero, CursorMode.ForceSoftware);
                            Invoke("HammerAgain", 0.1f);
                            vaseActiveHit.collider.GetComponent<Vase>().isBreak = true;
                            timeOut = 0;
                        }
                    }
                }
            }
        }
        else if (SmashMouseMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(hammerDown, Vector2.zero, CursorMode.ForceSoftware);
                Invoke("HammerAgain", 0.1f);
            }
            else
            {

            }

            RaycastHit2D zombieActiveHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, zombieMask);

            if (zombieActiveHit.collider)
            {
                if (Input.GetMouseButton(0))
                {
                    timeOut++;
                    if (timeOut > 0.1f)
                    {
                        Cursor.SetCursor(hammerDown, Vector2.zero, CursorMode.ForceSoftware);
                        Invoke("HammerAgain", 0.1f);
                        zombieActiveHit.collider.GetComponent<Zombie>().Hit(hammerDamage, false, false, false);
                        timeOut = 0;
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (IsStart)
        {
            if (StartSpawningZombie && !progressBarObject.activeInHierarchy)
            {
                progressBarObject.SetActive(true);
            }

            sunText.text = suns.ToString();

            if (CobcannonTarget)
            {
                if (Input.GetMouseButton(0))
                {
                    Invoke("SelectDestination", 1);
                    if (CobCannonSelected.GetComponent<CobCannon>().ActiveStep == 2) //Wait for select shoot destination;
                    {
                        cobcannonTargetLocation = Input.mousePosition;
                        CobCannonSelected.GetComponent<CobCannon>().ActiveStep = 3; //Shoot
                        CobCannonSelected.GetComponent<CobCannon>().shootDestination = convertMousePos2RealPos(); //Shoot
                        CobcannonTarget = false;
                    }
                }
            }
            else if (TheButTarget)
            {
                RaycastHit2D tileHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

                if (tileHit.collider)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Invoke("SelectTheButDestination", 1);
                        if (TheButSelected.GetComponent<TheBut>().ActiveStep == 2) //Wait for select shoot destination;
                        {
                            // Debug.Log(tileHit.collider.GetComponent<Tile>().name + tileHit.collider.GetComponent<Tile>().transform.position);
                            TheButTargetLocation = Input.mousePosition;
                            TheButSelected.GetComponent<TheBut>().ActiveStep = 3; //Shoot
                            TheButSelected.GetComponent<TheBut>().shootDestination = tileHit.collider.GetComponent<Tile>().transform.position; //Shoot
                            TheButTarget = false;

                        }
                    }
                }
            }
            else if (MountainGodTarget)
            {
                RaycastHit2D tileHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

                if (tileHit.collider)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Invoke("SelectMountainGodDestination", 1);
                        if (MountainGodSelected.GetComponent<MoutainGod>().ActiveStep == 2) //Wait for select shoot destination;
                        {
                            MountainGodTargetLocation = Input.mousePosition;
                            MountainGodSelected.GetComponent<MoutainGod>().ActiveStep = 3; //Shoot
                            MountainGodSelected.GetComponent<MoutainGod>().tileChoiceName = int.Parse(tileHit.collider.GetComponent<Tile>().name);
                            MountainGodSelected.GetComponent<MoutainGod>().shootDestination = tileHit.collider.GetComponent<Tile>().transform.position; //Shoot

                            Debug.Log(tileHit.collider.GetComponent<Tile>().name + MountainGodSelected.GetComponent<MoutainGod>().tileChoiceName);
                            MountainGodTarget = false;

                        }
                    }
                }
            }
            else
            {

            }

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

            foreach (Transform tile in tiles)
            {
                if (tile.GetComponent<Tile>().isGround && tile.GetComponent<Tile>().hasPlant && !tile.GetComponent<Tile>().isPlantable)
                {
                    tile.GetComponent<SpriteRenderer>().enabled = true;
                }
                else if ((tile.GetComponent<Tile>().isPool || tile.GetComponent<Tile>().isRoof) && !tile.GetComponent<Tile>().isUnavailable && tile.GetComponent<Tile>().hasPlant && !tile.GetComponent<Tile>().isPlantable)
                {
                    tile.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    tile.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            if (hit.collider && currentPlant)
            {
                if (!hit.collider.GetComponent<Tile>().isUnavailable)
                {
                    if (hit.collider.GetComponent<Tile>().isGround && hit.collider.GetComponent<Tile>().isPlantable)
                    {
                        hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
                        hit.collider.GetComponent<SpriteRenderer>().enabled = true;
                    }
                    else if ((hit.collider.GetComponent<Tile>().isRoof || hit.collider.GetComponent<Tile>().isPool) && ((!hit.collider.GetComponent<Tile>().isPlantable && !hit.collider.GetComponent<Tile>().hasPlant) || (hit.collider.GetComponent<Tile>().isPlantable && hit.collider.GetComponent<Tile>().hasPlant)))
                    {
                        hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
                        hit.collider.GetComponent<SpriteRenderer>().enabled = true;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        Plant(hit.collider.gameObject);
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        currentPlantSlot.transform.GetChild(3).gameObject.SetActive(false);
                        currentPlant = null;
                        currentPlantSprite = null;
                    }
                }
                else
                {

                }
            }
            else if (hit.collider && Shovel)
            {
                if (!hit.collider.GetComponent<Tile>().isUnavailable)
                {
                    hit.collider.GetComponent<SpriteRenderer>().sprite = shovelSprite;
                    hit.collider.GetComponent<SpriteRenderer>().enabled = true;

                    if (Input.GetMouseButton(0))
                    {
                        if (hit.collider.GetComponent<Tile>().hasPlant)
                        {
                            RemovePlant(hit.collider.gameObject);
                            Shovel = false;
                        }
                        else
                        {
                            Shovel = true;
                        }
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        Shovel = false;
                    }
                }
                else
                {

                }
            }

            RaycastHit2D plantActiveHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, plantMask);

            if (plantActiveHit.collider)
            {
                if (Input.GetMouseButton(0))
                {
                    if ((plantActiveHit.collider.gameObject.name == "Cobcannon(Clone)" || plantActiveHit.collider.gameObject.name == "GoldenTurtleBow(Clone)") && plantActiveHit.collider.GetComponent<CobCannon>().canShoot)
                    {
                        if (plantActiveHit.collider.GetComponent<CobCannon>().ActiveStep == 1)
                        {
                            CobCannonSelected = plantActiveHit.collider.gameObject;
                            CobcannonTarget = true;
                            plantActiveHit.collider.GetComponent<CobCannon>().animator.SetInteger("State", 1);
                            return;
                        }
                    }
                    else if (plantActiveHit.collider.gameObject.name == "TheBut(Clone)" && plantActiveHit.collider.GetComponent<TheBut>().canBless)
                    {
                        if (plantActiveHit.collider.GetComponent<TheBut>().ActiveStep == 1)
                        {
                            TheButSelected = plantActiveHit.collider.gameObject;
                            TheButTarget = true;
                            plantActiveHit.collider.GetComponent<TheBut>().animator.SetInteger("State", 1);
                            return;
                        }
                    }
                    else if (plantActiveHit.collider.gameObject.name == "MoutainGod" && plantActiveHit.collider.GetComponent<MoutainGod>().canRaise)
                    {
                        if (plantActiveHit.collider.GetComponent<MoutainGod>().ActiveStep == 1)
                        {
                            Debug.Log("Mountain God is selected");
                            MountainGodSelected = plantActiveHit.collider.gameObject;
                            MountainGodTarget = true;
                            plantActiveHit.collider.GetComponent<MoutainGod>().animator.Play("BeSelected");
                            return;
                        }
                    }
                }
            }

            RaycastHit2D sunHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, sunMask);

            if (sunHit.collider)
            {
                if (Input.GetMouseButton(0))
                {
                    // sunSource.pitch = Random.Range(.9f, 1.1f);
                    sunSource.PlayOneShot(sunSFX);
                    if (sunHit.collider.gameObject.name == "tinySun(Clone)")
                    {
                        suns += 15;
                    }
                    else if (sunHit.collider.gameObject.name == "sun(Clone)")
                    {
                        suns += 25;
                    }
                    Destroy(sunHit.collider.gameObject);
                }
            }
        }

        //void DestroyPlant(GameObject hit)
        //{
        //    if (hit.GetComponent<Tile>().hasPumpkin && hit.GetComponent<Tile>().pumpkinObject != null)
        //    {
        //        hit.GetComponent<Tile>().hasPumpkin = false;
        //        hit.GetComponent<Tile>().pumpkinObject.GetComponent<Plant>().Hit(100, 0);
        //    }

        //    if (hit.GetComponent<Tile>().hasPlant && hit.GetComponent<Tile>().plantObject != null && hit.GetComponent<Tile>().plantObject != hit.GetComponent<Tile>().backgroundObject)
        //    {
        //        hit.GetComponent<Tile>().plantObject.GetComponent<Plant>().Hit(100, 0);
        //    }

        //    if (hit.GetComponent<Tile>().isPlantable && hit.GetComponent<Tile>().hasPlant && hit.GetComponent<Tile>().backgroundObject != null)
        //    {
        //        hit.GetComponent<Tile>().hasPlant = false;
        //        hit.GetComponent<Tile>().isPlantable = false;
        //        hit.GetComponent<Tile>().backgroundObject.GetComponent<Plant>().Hit(100, 0);
        //    }
        //    hit.GetComponent<Tile>().isUnavailable = true;
        //}

        void RemovePlant(GameObject hit)
        {
            if (!hit.GetComponent<Tile>().isUnavailable)
            {
                if (hit.GetComponent<Tile>().hasPumpkin && hit.GetComponent<Tile>().pumpkinObject != null)
                {
                    hit.GetComponent<Tile>().hasPumpkin = false;
                    hit.GetComponent<Tile>().pumpkinObject.GetComponent<Plant>().Hit(100, 0);
                    return;
                }
                else if (hit.GetComponent<Tile>().hasPlant && hit.GetComponent<Tile>().plantObject != null && hit.GetComponent<Tile>().plantObject != hit.GetComponent<Tile>().backgroundObject)
                {
                    hit.GetComponent<Tile>().plantObject.GetComponent<Plant>().Hit(100, 0);
                    return;
                }
                else if (hit.GetComponent<Tile>().isPlantable && hit.GetComponent<Tile>().hasPlant && hit.GetComponent<Tile>().backgroundObject != null)
                {
                    hit.GetComponent<Tile>().hasPlant = false;
                    hit.GetComponent<Tile>().isPlantable = false;
                    hit.GetComponent<Tile>().backgroundObject.GetComponent<Plant>().Hit(100, 0);
                    return;
                }
            }
        }

        void Plant(GameObject hit)
        {
            if (hit.GetComponent<Tile>().isGround && hit.GetComponent<Tile>().isPlantable && !hit.GetComponent<Tile>().isUnavailable)
            {

                if (currentPlant.name == "Pot" || currentPlant.name == "PileField" || currentPlant.name == "Lilypad" || currentPlant.name == "SeaShroom" || currentPlant.name == "TangleKelp" || currentPlant.name == "Cattail")
                {
                    Debug.Log(currentPlant.name + " cannot be planted in ground");
                }
                else if ((currentPlant.name == "Jalapeno" || currentPlant.name == "IceShroom" || currentPlant.name == "CherryBomb" || currentPlant.name == "DoomShroom" || currentPlant.name == "Squash") && !hit.GetComponent<Tile>().hasPlant)
                {
                    PlantSelfDestruction(hit);
                }
                else if (currentPlant.name == "Pumpkin" && hit.GetComponent<Tile>().pumpkinObject == null && !hit.GetComponent<Tile>().hasPumpkin && hit.GetComponent<Tile>().isPlantable)
                {
                    PlantPumpkin(hit);
                }
                else if (currentPlant.name == "GraveBuster")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "GraveStone")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("GraveBuster must be planted on GraveStone");
                    }
                }
                else if (currentPlant.name == "SpikeRock")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "Caltrop(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("SpikeRock must be planted on Caltrop");
                    }
                }
                else if (currentPlant.name == "TwinSunFlower")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "SunFlower(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("TwinSunFlower must be planted on SunFlower");
                    }
                }
                else if (currentPlant.name == "CoffeeBean")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "Wallnut(Clone)")
                    {
                        RockandRoll(hit);
                    }
                }
                else if (currentPlant.name == "Cobcannon")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "Cornpult(Clone)")
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
                                    if (Convert.ToInt32(hit.GetComponent<Tile>().name) % 9 >= 0 && Convert.ToInt32(hit.GetComponent<Tile>().name) % 9 < 8)
                                    {
                                        if (Convert.ToInt32(_target.GetComponent<Tile>().name) == Convert.ToInt32(hit.GetComponent<Tile>().name) + 1)
                                        {
                                            if (_target.GetComponent<Tile>().plantObject.name == "Cornpult(Clone)")
                                            {
                                                UpgradeRightCobCannon(hit, _target);
                                            }
                                        }

                                    }
                                    else if (Convert.ToInt32(hit.GetComponent<Tile>().name) % 9 == 8)
                                    {
                                        if (Convert.ToInt32(_target.GetComponent<Tile>().name) == Convert.ToInt32(hit.GetComponent<Tile>().name) - 1)
                                        {
                                            if (_target.GetComponent<Tile>().plantObject.name == "Cornpult(Clone)")
                                            {
                                                UpgradeLeftCobCannon(hit, _target);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Cobcannon must be planted on Cornpult");
                    }
                }
                else if (currentPlant.name == "GloomShroom")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "FumeShroom(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("GloomShroom must be planted on FumeShroom");
                    }
                }
                else if (currentPlant.name == "WinterMelon")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "MelonPult(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("WinterMelon must be planted on MelonPult");
                    }
                }
                else if (currentPlant.name == "GatlingPea")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "Repeater(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("GatlingPea must be planted on Repeater");
                    }
                }
                else if (!hit.GetComponent<Tile>().hasPlant && currentPlant.name != "Pumpkin")
                {
                    JustPlant(hit);
                }
            }
            else if (hit.GetComponent<Tile>().isPool && !hit.GetComponent<Tile>().isUnavailable)
            {
                if (currentPlant.name == "Lilypad" || currentPlant.name == "PileField")
                {
                    if (hit.GetComponent<Tile>().backgroundObject == null && !hit.GetComponent<Tile>().hasPlant)
                    {
                        JustPlant(hit);
                    }
                    else
                    {

                    }
                }
                else if ((currentPlant.name == "Jalapeno" || currentPlant.name == "IceShroom" || currentPlant.name == "CherryBomb" || currentPlant.name == "DoomShroom" || currentPlant.name == "Squash") && hit.GetComponent<Tile>().plantObject.name == "Lilypad(Clone)" && hit.GetComponent<Tile>().isPlantable)
                {
                    PlantSelfDestruction(hit);
                }
                else if (currentPlant.name == "Pumpkin" && hit.GetComponent<Tile>().backgroundObject != null && hit.GetComponent<Tile>().pumpkinObject == null && !hit.GetComponent<Tile>().hasPumpkin && hit.GetComponent<Tile>().isPlantable)
                {
                    PlantPumpkin(hit);
                }
                else if (currentPlant.name == "SeaShroom" || currentPlant.name == "TangleKelp")
                {
                    if (hit.GetComponent<Tile>().isPlantable && !hit.GetComponent<Tile>().hasPlant)
                    {

                    }
                    else
                    {
                        JustPlant(hit);
                    }
                }
                else if (currentPlant.name == "Pot" || currentPlant.name == "RunnablePotatoMiner" || currentPlant.name == "UnderGroundPotatoMine" || currentPlant.name == "Caltrop" || currentPlant.name == "SpikeRock")
                {
                    Debug.Log(currentPlant.name + " cannot be planted in swimming pools");
                }
                else if (currentPlant.name == "Cattail")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "Lilypad(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("Cattail must be planted on lilypad");
                    }
                }
                else if (currentPlant.name == "TwinSunFlower")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "SunFlower(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("TwinSunFlower must be planted on SunFlower");
                    }
                }
                else if (currentPlant.name == "GloomShroom")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "FumeShroom(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("GloomShroom must be planted on FumeShroom");
                    }
                }
                else if (currentPlant.name == "WinterMelon")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "MelonPult(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("WinterMelon must be planted on MelonPult");
                    }
                }
                else if (currentPlant.name == "GatlingPea")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "Repeater(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("GatlingPea must be planted on Repeater");
                    }
                }
                else if (hit.GetComponent<Tile>().isPlantable && hit.GetComponent<Tile>().backgroundObject.name == "Lilypad(Clone)" && hit.GetComponent<Tile>().plantObject.name == "Lilypad(Clone)" && currentPlant.name != "Pumpkin")
                {
                    Debug.Log("Plant on Lilypad");
                    JustPlant(hit);
                }
            }
            else if (hit.GetComponent<Tile>().isRoof && !hit.GetComponent<Tile>().isUnavailable)
            {
                if (currentPlant.name == "Pot")
                {
                    if (hit.GetComponent<Tile>().backgroundObject == null && !hit.GetComponent<Tile>().hasPlant)
                    {
                        JustPlant(hit);
                    }
                    else
                    {

                    }
                }
                else if ((currentPlant.name == "Jalapeno" || currentPlant.name == "IceShroom" || currentPlant.name == "CherryBomb" || currentPlant.name == "DoomShroom" || currentPlant.name == "Squash"))
                {
                    PlantSelfDestruction(hit);
                }
                else if (currentPlant.name == "Pumpkin" && hit.GetComponent<Tile>().backgroundObject != null && hit.GetComponent<Tile>().pumpkinObject == null && !hit.GetComponent<Tile>().hasPumpkin && hit.GetComponent<Tile>().isPlantable)
                {
                    PlantPumpkin(hit);
                }
                else if (currentPlant.name == "Cattail" || currentPlant.name == "Lilypad" || currentPlant.name == "SeaShroom" || currentPlant.name == "TangleKelp" || currentPlant.name == "PileField")
                {
                    Debug.Log(currentPlant.name + " cannot be planted in pot");
                }
                else if (currentPlant.name == "Caltrop" || currentPlant.name == "SpikeRock")
                {
                    Debug.Log(currentPlant.name + " cannot be planted in pot");
                }
                else if (currentPlant.name == "TwinSunFlower")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "SunFlower(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("TwinSunFlower must be planted on SunFlower");
                    }
                }
                else if (currentPlant.name == "GloomShroom")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "FumeShroom(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("GloomShroom must be planted on FumeShroom");
                    }
                }
                else if (currentPlant.name == "WinterMelon")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "MelonPult(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("WinterMelon must be planted on MelonPult");
                    }
                }
                else if (currentPlant.name == "GatlingPea")
                {
                    if (hit.GetComponent<Tile>().plantObject.name == "Repeater(Clone)")
                    {
                        UpgradePlant(hit);
                    }
                    else
                    {
                        Debug.Log("GatlingPea must be planted on Repeater");
                    }
                }
                else if (hit.GetComponent<Tile>().isPlantable && hit.GetComponent<Tile>().backgroundObject.name == "Pot(Clone)" && currentPlant.name != "Pumpkin")
                {
                    Debug.Log(currentPlant.name + " has been planted");
                    JustPlant(hit);
                }
            }

        }

    }

    private void UpgradeLeftCobCannon(GameObject hit, GameObject nextHit)
    {
        if (hit.GetComponent<Tile>().isUnavailable) return;
        StartCoroutine(currentPlantSlot.CardCooldown(currentPlantSlot.cooldown));
        Destroy(hit.GetComponent<Tile>().plantObject);
        Destroy(nextHit.GetComponent<Tile>().plantObject);
        source.PlayOneShot(plantSFX);
        GameObject plant = Instantiate(currentPlant, nextHit.transform.position, Quaternion.identity);
        hit.GetComponent<Tile>().hasPlant = true;
        nextHit.GetComponent<Tile>().hasPlant = true;
        plant.GetComponent<Plant>().tile = hit.GetComponent<Tile>();
        plant.GetComponent<Plant>().tile = nextHit.GetComponent<Tile>();
        hit.GetComponent<Tile>().plantObject = plant;
        nextHit.GetComponent<Tile>().plantObject = plant;
        currentPlant = null;
        currentPlantSprite = null;
        suns -= currentPrice;
    }

    private void UpgradeRightCobCannon(GameObject hit, GameObject nextHit)
    {
        if (hit.GetComponent<Tile>().isUnavailable) return;
        StartCoroutine(currentPlantSlot.CardCooldown(currentPlantSlot.cooldown));
        Destroy(hit.GetComponent<Tile>().plantObject);
        Destroy(nextHit.GetComponent<Tile>().plantObject);
        source.PlayOneShot(plantSFX);
        GameObject plant = Instantiate(currentPlant, hit.transform.position, Quaternion.identity);
        hit.GetComponent<Tile>().hasPlant = true;
        nextHit.GetComponent<Tile>().hasPlant = true;
        plant.GetComponent<Plant>().tile = hit.GetComponent<Tile>();
        plant.GetComponent<Plant>().tile = nextHit.GetComponent<Tile>();
        hit.GetComponent<Tile>().plantObject = plant;
        nextHit.GetComponent<Tile>().plantObject = plant;
        currentPlant = null;
        currentPlantSprite = null;
        suns -= currentPrice;
    }
    private void PlantSelfDestruction(GameObject hit)
    {
        if (hit.GetComponent<Tile>().isUnavailable) return;
        if (Roof)
        {
            if (hit.GetComponent<Tile>().backgroundObject == null) return;
        }
        if (hit.GetComponent<Tile>().plantObject != null && hit.GetComponent<Tile>().plantObject != hit.GetComponent<Tile>().backgroundObject) return;

        StartCoroutine(currentPlantSlot.CardCooldown(currentPlantSlot.cooldown));
        source.PlayOneShot(plantSFX);
        GameObject plant = Instantiate(currentPlant, hit.transform.position, Quaternion.identity);

        if (currentPlant.name == "Squash")
        {
            plant.GetComponent<Squash>().lane = hit.GetComponent<Tile>().lane;
            // Debug.Log("Squash is planted " + plant.GetComponent<Squash>().lane);
        }
        else if (currentPlant.name == "Jalapeno")
        {
            plant.GetComponent<Jalapeno>().lane = hit.GetComponent<Tile>().lane;
            // Debug.Log("Jalapeno is planted " + plant.GetComponent<Jalapeno>().lane);
        }
        hit.GetComponent<Tile>().plantObject = plant;
        hit.GetComponent<Tile>().hasPlant = true;
        hit.GetComponent<Tile>().isPlantable = true;
        plant.GetComponent<Plant>().tile = hit.GetComponent<Tile>();
        currentPlant = null;
        currentPlantSprite = null;
        suns -= currentPrice;

    }

    private void PlantPumpkin(GameObject hit)
    {
        if (hit.GetComponent<Tile>().isUnavailable) return;
        if (Roof)
        {
            if (hit.GetComponent<Tile>().backgroundObject == null) return;
        }
        else
        {
            if (hit.GetComponent<Tile>().plantObject != null && hit.GetComponent<Tile>().plantObject.name == "GraveStone") return;
        }
        if (hit.GetComponent<Tile>().pumpkinObject != null) return;
        StartCoroutine(currentPlantSlot.CardCooldown(currentPlantSlot.cooldown));
        source.PlayOneShot(plantSFX);
        GameObject plant = Instantiate(currentPlant, hit.transform.position, Quaternion.identity);
        hit.GetComponent<Tile>().hasPumpkin = true;
        hit.GetComponent<Tile>().pumpkinObject = plant;
        plant.GetComponent<Plant>().tile = hit.GetComponent<Tile>();
        currentPlant = null;
        currentPlantSprite = null;
        suns -= currentPrice;
    }


    private void RockandRoll(GameObject hit)
    {
        if (hit.GetComponent<Tile>().isUnavailable) return;
        StartCoroutine(currentPlantSlot.CardCooldown(currentPlantSlot.cooldown));
        /*
        Debug.Log("Rock and Roll with Nut and Coffee Bean");
        hit.GetComponent<Nuts>().Rool = true;
        GameObject plant = Instantiate(currentPlant, hit.transform.position, Quaternion.identity);
        hit.GetComponent<Tile>().hasPlant = false;
        hit.GetComponent<Tile>().plantObject = null;
        currentPlant = null;
        currentPlantSprite = null;
        suns -= currentPrice;
        */
        // Debug.Log("Rock and Roll with Nut and Coffee Bean");
        // hit.GetComponent<Nuts>().Rool = true;
        hit.GetComponent<Tile>().plantObject.GetComponent<Nuts>().Roll = true;
        GameObject plant = Instantiate(currentPlant, hit.transform.position, Quaternion.identity);
        hit.GetComponent<Tile>().hasPumpkin = true;
        hit.GetComponent<Tile>().isPlantable = true;
        hit.GetComponent<Tile>().pumpkinObject = plant;
        plant.GetComponent<Plant>().tile = hit.GetComponent<Tile>();
        currentPlant = null;
        currentPlantSprite = null;
        suns -= currentPrice;
    }

    private void UpgradePlant(GameObject hit)
    {
        if (hit.GetComponent<Tile>().isUnavailable) return;
        StartCoroutine(currentPlantSlot.CardCooldown(currentPlantSlot.cooldown));
        Destroy(hit.GetComponent<Tile>().plantObject);
        source.PlayOneShot(plantSFX);
        GameObject plant = Instantiate(currentPlant, hit.transform.position, Quaternion.identity);
        hit.GetComponent<Tile>().hasPlant = true;
        plant.GetComponent<Plant>().tile = hit.GetComponent<Tile>();
        hit.GetComponent<Tile>().plantObject = plant;
        if (currentPlant.name == "WinterMelon")
        {
            plant.GetComponent<BasicShooter>().lane = hit.GetComponent<Tile>().lane;
            // Debug.Log("WinterMelon is planted " + plant.GetComponent<BasicShooter>().lane);
        }
        currentPlant = null;
        currentPlantSprite = null;
        suns -= currentPrice;
    }

    private void JustPlant(GameObject hit)
    {
        if (hit.GetComponent<Tile>().isUnavailable) return;
        Debug.Log("Current Plant: " + currentPlant.name + " / " + hit.transform.position);
        StartCoroutine(currentPlantSlot.CardCooldown(currentPlantSlot.cooldown));
        source.PlayOneShot(plantSFX);
        GameObject plant = Instantiate(currentPlant, hit.transform.position, Quaternion.identity);
        hit.GetComponent<Tile>().isPlantable = true;
        hit.GetComponent<Tile>().hasPlant = true;
        plant.GetComponent<Plant>().tile = hit.GetComponent<Tile>();
        if (currentPlant.name == "Lilypad" || currentPlant.name == "Pot" || currentPlant.name == "PileField")
        {
            if (hit.GetComponent<Tile>().backgroundObject == null)
            {
                hit.GetComponent<Tile>().backgroundObject = plant;
            }
            if (hit.GetComponent<Tile>().plantObject == null)
            {
                hit.GetComponent<Tile>().plantObject = plant;
            }
        }
        else
        {
            hit.GetComponent<Tile>().plantObject = plant;
        }

        if (currentPlant.name == "Cornpult")
        {
            plant.GetComponent<CornPult>().lane = hit.GetComponent<Tile>().lane;
            // Debug.Log("CornPult is planted " + plant.GetComponent<CornPult>().lane);
        }
        else if (currentPlant.name == "Cabbage-pult")
        {
            plant.GetComponent<CabbagePult>().lane = hit.GetComponent<Tile>().lane;
            // Debug.Log("CabbagePult is planted " + plant.GetComponent<CabbagePult>().lane);
        }
        else if (currentPlant.name == "Tigergrass")
        {
            plant.GetComponent<TigerGrass>().lane = hit.GetComponent<Tile>().lane;
            // Debug.Log("Tigergrass is planted " + plant.GetComponent<TigerGrass>().lane);
        }
        else if (currentPlant.name == "BambooTrooper")
        {
            plant.GetComponent<BamBooTrooper>().lane = hit.GetComponent<Tile>().lane;
            // Debug.Log("BambooTrooper is planted " + plant.GetComponent<BamBooTrooper>().lane);
        }
        else if (currentPlant.name == "MelonPult")
        {
            plant.GetComponent<BasicShooter>().lane = hit.GetComponent<Tile>().lane;
            // Debug.Log("MelonPult is planted " + plant.GetComponent<BasicShooter>().lane);
        }

        currentPlant = null;
        currentPlantSprite = null;
        suns -= currentPrice;
    }


    private void JustSetObject(GameObject hit, GameObject usedObject)
    {
        if (hit.GetComponent<Tile>().isUnavailable) return;
        GameObject plant = Instantiate(usedObject, hit.transform.position, Quaternion.identity);
        hit.GetComponent<Tile>().isPlantable = true;
        hit.GetComponent<Tile>().hasPlant = true;
        plant.GetComponent<Plant>().tile = hit.GetComponent<Tile>();
        if (usedObject.name == "Lilypad" || usedObject.name == "Pot" || usedObject.name == "PileField")
        {
            if (hit.GetComponent<Tile>().backgroundObject == null)
            {
                hit.GetComponent<Tile>().backgroundObject = plant;
            }
            if (hit.GetComponent<Tile>().plantObject == null)
            {
                hit.GetComponent<Tile>().plantObject = plant;
            }
        }
        else
        {
            hit.GetComponent<Tile>().plantObject = plant;
        }
    }
}
