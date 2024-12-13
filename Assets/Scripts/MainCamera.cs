using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Animator animator;
    private GameManager gms;
    // Start is called before the first frame update
    void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gms.isVaseBreaker)
        {
            animator.Play("Fix");
        }
        else
        {
            animator.SetBool("Status", false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gms.isVaseBreaker) return;

        if(gms.IsStart)
        {
            if(animator.GetBool("Status"))
            {
                return;
            }
            else
            {
                animator.SetBool("Status", true);
            }
        }
    }
}
