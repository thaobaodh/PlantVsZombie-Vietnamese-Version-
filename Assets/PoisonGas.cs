using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonGas : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] int damage = 10;
    [SerializeField] float distanceToFade = 5f;
    [SerializeField] float cooldown = 2f;
    private bool canDamage = true;
    private Vector3 firstPosition;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        canDamage = true;
        firstPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, firstPosition) < distanceToFade)
        {
            // Debug.Log("Distance: " + Vector3.Distance(transform.position, firstPosition));
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<Plant>(out Plant plant))
        {
            if (!canDamage) return;
            Debug.Log("Plant in gas: " + plant.name);
            plant.Hit(damage, 1);
            canDamage = false;
            Invoke("ResetCooldown", cooldown);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!canDamage) return; 
        if (other.TryGetComponent<Plant>(out Plant plant))
        {
            Debug.Log("Plant in gas: " + plant.name);
            plant.Hit(damage, 1);
            canDamage = false;
            Invoke("ResetCooldown", cooldown);
        }
    }

    private void ResetCooldown()
    {
        canDamage = true;
    }
}
