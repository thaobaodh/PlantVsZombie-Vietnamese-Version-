using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Pumpkin : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int max_health;
    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetInteger("Health", 0);
        max_health = GetComponent<Plant>().health;
    }

    // Update is called once per frame
    void Update()
    {
        health = GetComponent<Plant>().health;
        if (health >= (3 * max_health) / 4)
        {
            animator.SetInteger("Health", 1);
        }
        else if ((health >= (1 * max_health) / 2) && (health < (3 * max_health) / 4))
        {
            animator.SetInteger("Health", 2);
        }
        else if ((health >= (1 * max_health) / 4) && (health < (1 * max_health) / 2))
        {
            animator.SetInteger("Health", 3);
        }
        else
        {
            animator.SetInteger("Health", 4);
        }
    }

}
