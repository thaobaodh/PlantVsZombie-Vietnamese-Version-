using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    public bool isDrop;
    public int durability = 12;
    private int max_durability;
    [SerializeField] Sprite Cone_Damage1;
    [SerializeField] Sprite Cone_Damage2;
    [SerializeField] Sprite Cone_Damage3;
    // Start is called before the first frame update
    void Start()
    {
        isDrop = false;
        max_durability = durability;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (durability >= 2 * max_durability / 3)
        {
            if (GetComponent<SpriteRenderer>().sprite.name != "Zombie_cone1")
            {
                GetComponent<SpriteRenderer>().sprite = Cone_Damage1;
            }
        }
        else if ((durability >= 1 * max_durability / 3) && (durability < 2 * max_durability / 3))
        {
            if (GetComponent<SpriteRenderer>().sprite.name != "Zombie_cone2")
            {
                GetComponent<SpriteRenderer>().sprite = Cone_Damage2;
            }
        }
        else if ((durability > 0) && (durability < 1 * max_durability / 3))
        {
            if (GetComponent<SpriteRenderer>().sprite.name != "Zombie_cone3")
            {
                GetComponent<SpriteRenderer>().sprite = Cone_Damage3;
            }
        }
        else if (durability <= 0)
        {
            if (isDrop)
                return;
            isDrop = true;
            Destroy(gameObject, 1);
            for (int i = 0; i < 30; i++)
            {
                gameObject.transform.position = gameObject.transform.position + new Vector3(0, -0.05f, 0);
            }
        }
    }
}
