using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HurricaneSand : MonoBehaviour
{
    [SerializeField] private bool isAttack = true;

    [SerializeField] private int count = 0;

    [SerializeField] private int maxReach = 4;

    [SerializeField] private int damage = 10;

    [SerializeField] private float speed = 1f;

    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Idle");
        Destroy(gameObject, 20f);
        Invoke(nameof(RunAnimation), 1.3f);
    }

    private void RunAnimation()
    {
        this.PlayAnimation("HugeHurricane");
    }
    // Update is called once per frame
    void Update()
    {
        this.Movement();
    }

    private void PlayAnimation(string animatorName)
    {
        animator.Play(animatorName);
    }

    private void Movement()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        if (transform.position.x <= -4.5f)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAttack) return;
        if (other.TryGetComponent<Plant>(out Plant plant))
        {
            if(plant.name == "TheBut(Clone)")
            {
                Destroy(gameObject, 0.5f);
            }
            else if (plant.name == "Tallnut(Clone)")
            {
                speed = 0;
                plant.Hit(damage, 1);
                Destroy(gameObject, 3f);
            }
            else
            {
                if (count > 4)
                {
                    count = 0;
                    Destroy(gameObject, 0.5f);
                }
                else
                {
                    count++;
                    plant.transform.position = transform.position;
                    plant.Hit(damage, 1);
                }
            }
            isAttack = false;
            Invoke(nameof(ResetCooldown), 1f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isAttack) return;
        if (other.TryGetComponent<Plant>(out Plant plant))
        {
            if (plant.name == "TheBut(Clone)")
            {
                Destroy(gameObject, 0.5f);
            }
            else if (plant.name == "Tallnut(Clone)")
            {
                speed = 0;
                plant.Hit(damage, 1);
                Destroy(gameObject, 3f);
            }
            else
            {
                if (count > 4)
                {
                    count = 0;
                    Destroy(gameObject, 0.5f);
                }
                else
                {
                    count++;
                    plant.transform.position = transform.position;
                    plant.Hit(damage, 1);
                }
            }
            isAttack = false;
            Invoke(nameof(ResetCooldown), 1f);
        }
    }

    private void ResetCooldown()
    {
        isAttack = true;
    }
}
