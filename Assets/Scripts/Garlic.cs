using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Garlic : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int max_health;

    [SerializeField] Sprite Garlic_body;
    [SerializeField] Sprite Garlic_body1;
    [SerializeField] Sprite Garlic_body2;


    // Start is called before the first frame update
    void Start()
    {
        max_health = GetComponent<Plant>().health;
    }

    // Update is called once per frame
    void Update()
    {
        health = GetComponent<Plant>().health;
        if (health >= (2 * max_health) / 3)
        {
            GetComponent<SpriteRenderer>().sprite = Garlic_body;
            GetComponent<SpriteRenderer>().transform.localScale = Vector3.one;
        }
        else if ((health >= (1 * max_health) / 3) && (health < (2 * max_health) / 3))
        {
            GetComponent<SpriteRenderer>().sprite = Garlic_body1;
            GetComponent<SpriteRenderer>().transform.localScale = Vector3.one;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Garlic_body2;
            GetComponent<SpriteRenderer>().transform.localScale = Vector3.one;
        }
    }

}
