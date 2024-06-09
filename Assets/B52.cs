using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class B52 : MonoBehaviour
{
    [SerializeField] List<Transform> positionsList;
    [SerializeField] List<GameObject> zombieParachutesList;
    [SerializeField] List<GameObject> zombieVehiclesList;
    [SerializeField] Transform zombieParachuteSpawner;
    [SerializeField] int skill = 0;
    [SerializeField] int numOfSkills = 5;
    [SerializeField] int zombieParachuteNum = 1;
    [SerializeField] float speed = 5f;
    [SerializeField] float cooldown = 10f;
    [SerializeField] bool isReady = false;
    [SerializeField] Animator animator;
    private int RandomPosition = 0;
    private bool GoUp = false;
    private GameManager gms;
    // Start is called before the first frame update
    void Start()
    {
        isReady = false;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator.Play("Idle");
        Invoke("ResetCooldown", cooldown);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gms.IsStart) return;
        if(!gms.StartSpawningZombie) return;
        if (!isReady) return;

        if (GoUp)
        {
            if (transform.position.y >= positionsList[RandomPosition].position.y)
            {
                isReady = false;
                animator.Play("ReleaseZombie");
                Invoke("KeepRelease", 1.5f);
                Invoke("ResetCooldown", cooldown);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, positionsList[RandomPosition].position, speed * Time.deltaTime);
            }
        }
        else
        {
            if (transform.position.y <= positionsList[RandomPosition].position.y)
            {
                isReady = false;
                animator.Play("ReleaseZombie");
                Invoke("KeepRelease", 1.5f);
                Invoke("ResetCooldown", cooldown);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, positionsList[RandomPosition].position, speed * Time.deltaTime);
            }
        }

    }

    void KeepRelease()
    {
        animator.Play("StillReleaseZombie");
        switch(skill)
        {
            case 0:
                {
                    for (int i = 0; i < zombieParachuteNum; i++)
                    {
                        GameObject zombieParachutes = Instantiate(zombieParachutesList[Random.Range(0, zombieParachutesList.Count)], zombieParachuteSpawner.position, Quaternion.identity);
                        zombieParachutes.GetComponent<Zombie>().lane = RandomPosition.ToString();
                    }
                    break;
                }
            case 1:
                {
                    GameObject zombieFighter = Instantiate(zombieVehiclesList[0], positionsList[RandomPosition].position, Quaternion.identity);
                    zombieFighter.GetComponent<ZombieFighter>().lane = RandomPosition.ToString();
                    break;
                }
            case 2:
                {

                    GameObject missleZombie = Instantiate(zombieVehiclesList[1], positionsList[RandomPosition].position, Quaternion.identity);
                    missleZombie.GetComponent<MissleZombie>().lane = RandomPosition.ToString();
                    break;
                }
            case 3:
                {

                    GameObject doubleCabin = Instantiate(zombieVehiclesList[2], positionsList[RandomPosition].position, Quaternion.identity);
                    doubleCabin.GetComponent<DoubleCabinAircraftZombie>().lane = RandomPosition.ToString();
                    break;
                }
            case 4:
                {

                    GameObject arbiterx = Instantiate(zombieVehiclesList[3], positionsList[RandomPosition].position, Quaternion.identity);
                    arbiterx.GetComponent<ArbiterX>().lane = RandomPosition.ToString(); 
                    break;
                }
        }

        Invoke("Close", 3f);
    }
    void Close()
    {
        animator.Play("Close");
        Invoke("Idle", 1.5f);
    }
    void Idle()
    {
        animator.Play("Idle");
    }
    void ResetCooldown()
    {
        isReady = true;
        RandomPosition = Random.Range(0, positionsList.Count);
        skill = Random.Range(0, numOfSkills);
        // skill = 3;
        if (transform.position.y <= positionsList[RandomPosition].position.y)
        {
            GoUp = true;
            Debug.Log("Random Position: " + RandomPosition + " Up" + " / Skill: " + skill);
        }
        else
        {
            GoUp = false;
            Debug.Log("Random Position: " + RandomPosition + " Down" + " / Skill: " + skill);
        }
    }
}
