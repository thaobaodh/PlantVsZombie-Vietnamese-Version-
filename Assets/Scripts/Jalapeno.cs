using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Jalapeno : MonoBehaviour
{
    public string lane; 

    public GameObject flame;

    public float cooldown;

    private AudioSource source;

    public AudioClip[] shootClips;

    float leftX = -4.3f;

    float rightX = 8f;
    
    int numSpace = 8;

    float spaceX;

    private GameObject[] targets;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        spaceX = (Mathf.Abs(leftX) + Mathf.Abs(rightX)) / 8;
        InvokeRepeating("ResetCooldown", 1, cooldown);
        source.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
    }

    void ResetCooldown()
    {
        for (int i = 0; i <= numSpace; i++)
        {
            float positionX = leftX + (i * spaceX);
            float positionY = gameObject.transform.position.y;
            targets = GameObject.FindGameObjectsWithTag("Tile");
            if (targets == null)
            {

            }
            else
            {
                foreach (GameObject _target in targets)
                {
                    if(_target.GetComponent<Tile>().lane == gameObject.GetComponent<Plant>().tile.GetComponent<Tile>().lane)
                    {
                        InitFlame(_target.GetComponent<Tile>().transform.position.x, _target.GetComponent<Tile>().transform.position.y);
                    }
                }
            }
        }
        GetComponent<Plant>().Hit(1000, 0);
    }
    void InitFlame(float positionX, float positionY)
    {
        Vector3 flamePosition = new Vector3(positionX, positionY, 0);
        GameObject flameEffect = Instantiate(flame, flamePosition, Quaternion.identity);
    }

}
