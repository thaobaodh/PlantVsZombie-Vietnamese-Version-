using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public GameObject zombie;

    public ZombieType[] sampleZombiePreview;

    public GameObject FogSprites;

    public ZombieTypeProb[] zombieTypes;

    private List<ZombieType> probList = new List<ZombieType>();

    public ZombieWaveProb[] zombieWaves;

    public int zombieMax;

    public int zombieSpawned;

    public int zombiesDie = 0;

    public Slider progressBar;

    public float zombieDelay;
    
    private GameManager gms;

    private int zombieCount;

    private int countWave;

    private GameObject[] targets;
    private void Start()
    {

        // 9 - 15 X
        // Y -7.5 - 2
        zombieSpawned = 0;

        zombiesDie = 0;

        zombieCount = 0;

        countWave = 0;

        gms = GameObject.Find("GameManager").GetComponent<GameManager>();


        if (gms.isVaseBreaker)
        {
            gms.StartSpawningZombie = true;
            gms.IsStart = true;
            return;
        }
        else
        {
            if (gms.isFogAndRain)
            {
                for (int i = 0; i < 40; i++)
                {
                    Instantiate(FogSprites, new Vector3(Random.Range(2f, 12f), Random.Range(-5.5f, 5f), 0), Quaternion.identity);
                }
            }

            if (!gms.IsStart)
            {
                foreach (ZombieType _zombie in sampleZombiePreview)
                {
                    for (int i = 0; i <= Random.Range(0, 2); i++)
                    {
                        GameObject myZombie = Instantiate(_zombie.model, new Vector3(Random.Range(11f, 15f), Random.Range(-3f, 3f), 0), Quaternion.identity);
                        myZombie.GetComponent<Zombie>().type = _zombie;
                    }
                }
            }

            if (zombieWaves.Length > 0)
            {
                zombieDelay = zombieWaves[0].zombieDelay;
                zombieTypes = zombieWaves[0].types;
            }

            InvokeRepeating("SpawnZombie", 15, zombieDelay);
            foreach (ZombieTypeProb zom in zombieTypes)
            {
                for (int i = 0; i < zom.probability; i++)
                {
                    probList.Add(zom.type);
                }
            }
            for (int i = 0; i < zombieWaves.Length; i++)
            {
                zombieMax += zombieWaves[i].numOfZombies;
            }
            progressBar.maxValue = zombieMax;
        }
    }
    private void FixedUpdate()
    {
        if (gms.isVaseBreaker) return;

        progressBar.value = zombieSpawned;
        if (zombiesDie >= zombieSpawned && zombieSpawned == zombieMax)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Win();
        }
    }

    void SpawnZombie()
    {
        if (!gms.IsStart)
        {
            return;
        }

        if (zombieSpawned >= zombieMax)
        {
            return;
        }

        gms.StartSpawningZombie = true;

        if (zombieCount == zombieWaves[countWave].numOfZombies)
        {
            // Debug.Log("Zombie wave " + countWave + " finished with " + zombieCount + " zombies");
            CancelInvoke("SpawnZombie");
            countWave++;
            probList.Clear();
            if (countWave < zombieWaves.Length)
            {
                zombieDelay = zombieWaves[countWave].zombieDelay;
                zombieTypes = zombieWaves[countWave].types;
            }

            foreach (ZombieTypeProb zom in zombieTypes)
            {
                for (int i = 0; i < zom.probability; i++)
                {
                    probList.Add(zom.type);
                }
            }
            InvokeRepeating("SpawnZombie", 15, zombieDelay);
            zombieCount = 0;

            if (gms.isFogAndRain)
            {
                Debug.Log("Plantern is dead -> The Fog will recovering");
                for (int i = 0; i < 40; i++)
                {
                    Instantiate(FogSprites, new Vector3(Random.Range(2f, 12f), Random.Range(-5.5f, 5f), 0), Quaternion.identity);
                }
            }
        }

        zombieCount++;
        zombieSpawned++;
        int r = Random.Range(0, spawnPoints.Length);
        int z = Random.Range(0, probList.Count);

        GameObject myZombie = Instantiate(probList[z].model, spawnPoints[r].position, Quaternion.identity);
        myZombie.GetComponent<Zombie>().type = probList[z];
        myZombie.GetComponent<Zombie>().lane = spawnPoints[r].name;

        if (zombieSpawned >= zombieMax)
        {
            myZombie.GetComponent<Zombie>().lastZombie = true;
        }

        //if (probList[z].zName == "BalloonZombie")
        //{
        //    GameObject myZombie = Instantiate(probList[z].model, spawnPoints[r].position, Quaternion.identity);
        //    myZombie.GetComponent<BalloonZombie>().type = probList[Random.Range(0, probList.Count)];

        //    if (zombieSpawned >= zombieMax)
        //    {
        //        myZombie.GetComponent<BalloonZombie>().lastZombie = true;
        //    }
        //}
        //else
        //{
        //    GameObject myZombie = Instantiate(probList[z].model, spawnPoints[r].position, Quaternion.identity);
        //    myZombie.GetComponent<Zombie>().type = probList[z];
        //    myZombie.GetComponent<Zombie>().lane = spawnPoints[r].name;

        //    if (zombieSpawned >= zombieMax)
        //    {
        //        myZombie.GetComponent<Zombie>().lastZombie = true;
        //    }
        //}
    }

}
[System.Serializable]
public class ZombieTypeProb
{
    public ZombieType type;
    public int probability;

}


[System.Serializable]
public class ZombieWaveProb
{
    public int numOfZombies;
    public float zombieDelay;
    public ZombieTypeProb[] types;

}