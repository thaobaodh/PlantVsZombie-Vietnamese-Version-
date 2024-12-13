using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyZombieBoat : MonoBehaviour
{
    public string lane;
    public int health;
    [SerializeField] float speed = 5;
    [SerializeField] float range = 1f;
    [SerializeField] int damage = 5;
    [SerializeField] float cooldown = 1f;
    [SerializeField] LayerMask plantMask;

    public bool dead = false;

    private Plant targetPlant;

    private bool canAttack = true;

    private GameManager gms;

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        canAttack = true;
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gms.IsStart)
        {
            RaycastHit2D plantHit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);

            if (plantHit.collider)
            {
                if (plantHit.collider.GetComponent<Plant>() != null)
                {
                    targetPlant = plantHit.collider.GetComponent<Plant>();
                    AttackPlant(targetPlant);
                }
            }
            else
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }

        }
    }

    public void Hit(int damage)
    {
        if (dead) return;
        if(health > 0)
        {
            health -= damage;
            if(health <= 0)
            {
                dead = true;
                speed = 0;
                Destroy(gameObject, 0.5f);
            }
        }
    }

    void AttackPlant(Plant plant)
    {
        if (!canAttack || !plant)
            return;

        canAttack = false;
        plant.Hit(damage, 0);
        Invoke(nameof(ResetEatCooldown), cooldown);
    }

    private void ResetEatCooldown()
    {
        canAttack = true;
    }
}
