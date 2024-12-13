using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IceShroom : MonoBehaviour
{
    private GameObject[] targets;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Zombie");
        if (targets == null)
        {

        }
        else
        {
            foreach (GameObject _target in targets)
            {
                if(_target.GetComponent<Zombie>() != null)
                {
                    _target.GetComponent<Zombie>().Hit(damage, false, false, true);

                }
                else if (_target.GetComponent<BalloonZombie>() != null)
                {
                    _target.GetComponent<BalloonZombie>().Hit(damage);
                }
            }
        }

        GetComponent<Plant>().Hit(100, 1);
    }
}
