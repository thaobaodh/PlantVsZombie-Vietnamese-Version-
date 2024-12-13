using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 5;
    [SerializeField] private bool canHit = true;
    [SerializeField] private float destroyTme = 10f;
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canHit) return;
        animator.Play("Rotate");
        transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canHit) return;
        if (other.TryGetComponent<Plant>(out Plant plant))
        {
            if(plant.name == "TorchWood(Clone)")
            {
                canHit = false;
                animator.Play("Stop");
                Destroy(gameObject, 1);
            }
            else if (plant.name == "Caltrop(Clone)" || plant.name == "SpikeRock(Clone)" || plant.name == "UnderGroundPotatoMine(Clone)")
            {

            }
            else
            {
                plant.Hit(damage, 1);
                canHit = false;
                animator.Play("Stop");
                Invoke(nameof(Resetcooldown), destroyTme);
            }
        }
        else
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach(GameObject bullet in bullets)
            {
                if(bullet.name == other.name)
                {
                    // Debug.Log("Bullet in Spider Web:" + bullet.name);
                    bullet.transform.position = transform.position;
                    Destroy(bullet.gameObject, 0.5f);
                    // Destroy(gameObject, 1f);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Plant>(out Plant plant))
        {

        }
        else
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets)
            {
                if (bullet.name == other.name)
                {
                    // Debug.Log("Bullet in Spider Web:" + bullet.name);
                    bullet.transform.position = transform.position;
                    Destroy(bullet.gameObject, 0.5f);
                    // Destroy(gameObject, 1f);
                }
            }
        }
    }

    protected virtual void Resetcooldown()
    {
        Destroy(gameObject);
    }
}
