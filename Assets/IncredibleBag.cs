using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IncredibleBag : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    [SerializeField] private Animator animator;

    [SerializeField] private bool reachTarget = false;

    [SerializeField] private LayerMask plantMask;
    // Start is called before the first frame update
    void Start()
    {
        reachTarget = false;
        animator.Play("Idle");
        Invoke(nameof(RunAnimation), 1f);
    }

    private void RunAnimation()
    {
        this.PlayAnimation("Adsorb");
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
        if (!reachTarget)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 2f, plantMask);
            if (hit.collider)
            {
                if (hit.collider.name == "TheBut(Clone)")
                {
                    Destroy(gameObject, 1f);
                }
                else
                {
                    hit.collider.GetComponent<Plant>().transform.position = transform.position;
                    hit.collider.GetComponent<Plant>().Hit(100, 1);
                }
            }

            transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
            if (transform.position.x <= -4.5f)
            {
                reachTarget = true;
            }
        }
        else
        {
            transform.position += new Vector3(speed * 5 * Time.deltaTime, 0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(reachTarget)
        {
            if (other.TryGetComponent<YellowBrowDemonZombie>(out YellowBrowDemonZombie yellowBrowDemonZombie))
            {
                Debug.Log("Comeback to Yellow Brown Demon Zombie");
                Destroy(gameObject, 1f);
            }
        }
    }
}
