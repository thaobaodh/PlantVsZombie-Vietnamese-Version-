using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MagnetShroom : MonoBehaviour
{
    [SerializeField] bool hasMagnetic;
    [SerializeField] float range;
    [SerializeField] float cooldown;
    [SerializeField] float activeTime;
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] targets;
    private int count = 0;
    // Start is called before the first frame update
    private void Start()
    {
        hasMagnetic = true;
        animator.SetInteger("Magnetic", 0);
    }

    private void ResetCooldown()
    {
        if (animator.GetInteger("Magnetic") != 4)
        {
            animator.SetInteger("Magnetic", 4);
        }

        Invoke("Idle", 1);
    }

    private void Idle()
    {
        if (animator.GetInteger("Magnetic") != 0)
        {
            animator.SetInteger("Magnetic", 0);
        }
        count = 0;
        hasMagnetic = true;
    }

    private void Deactived()
    {
        count++;
        if (count == 1)
        {
            hasMagnetic = false;
            if (animator.GetInteger("Magnetic") != 2)
            {
                animator.SetInteger("Magnetic", 2);
            }

            Invoke("Recover", cooldown - 1);

            Invoke("ResetCooldown", cooldown);
        }

    }

    private void Recover()
    {
        if (animator.GetInteger("Magnetic") != 3)
        {
            animator.SetInteger("Magnetic", 3);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!hasMagnetic)
            return;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Zombie>(out Zombie zombie))
            {
                targets = GameObject.FindGameObjectsWithTag("Metal");
                if (targets == null)
                {

                }
                else
                {

                    foreach (GameObject _target in targets)
                    {
                        if (zombie.MetalObject != null && zombie.MetalObject.transform.position == _target.transform.position)
                        {
                            Invoke("Deactived", activeTime);

                            if (animator.GetInteger("Magnetic") != 1)
                            {
                                animator.SetInteger("Magnetic", 1);
                            }

                            Debug.Log("Zombie in Range - Detected Metal");
                            _target.transform.position = gameObject.transform.position;
                            zombie.MetalObject = null;
                            Destroy(_target, cooldown - 1);
                            // _target.tag = "Untagged";
                            // Debug.Log("Zombie Head position: " + _target.transform.position);

                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
