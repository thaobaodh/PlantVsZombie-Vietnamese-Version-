using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Plantern : MonoBehaviour
{
    [SerializeField] int range;

    private GameObject[] targets;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targets = GameObject.FindGameObjectsWithTag("Fog");
        if (targets == null)
        {

        }
        else
        {
            foreach (GameObject _target in targets)
            {
                if(Vector3.Distance(_target.transform.position, gameObject.transform.position) <= range * 1.5)
                {
                    Debug.Log("The fog cleared");
                    Destroy(_target.gameObject);
                    //_target.gameObject.SetActive(false);
                }
                else
                { 

                }
            }
        }
    }
}
