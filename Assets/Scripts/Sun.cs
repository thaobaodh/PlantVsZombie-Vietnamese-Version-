using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float dropToYPos;
    private float dropSpeed = 0.15f;
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, Random.Range(6f, 12f));
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.y > dropToYPos)
        {
            transform.position -= new Vector3(0, dropSpeed * Time.fixedDeltaTime, 0);
        }
    }
}
