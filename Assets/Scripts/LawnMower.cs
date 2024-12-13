using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawnMower : MonoBehaviour
{
    [SerializeField] bool isMoving;

    [SerializeField] float speed = 1f;

    [SerializeField] AudioClip sound;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other != null && other.GetComponent<Zombie>() != null)
        {
            other.GetComponent<Zombie>().Hit(1000, false, false, false);

            if (other.gameObject.layer == 7)
            {
                if (!isMoving)
                {
                    source.PlayOneShot(sound);
                }

                isMoving = true;
                Destroy(gameObject, 8);
            }
        }

    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
}
