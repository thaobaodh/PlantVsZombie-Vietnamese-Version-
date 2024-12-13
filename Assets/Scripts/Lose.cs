using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Lose : MonoBehaviour
{
    public Animator ani;
    private bool hasLost;
    public AudioClip loseSFX;
    public AudioClip Scream;

    public AudioSource music;
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            if (hasLost) return;

            if (other.GetComponent<Zombie>().gameObject != null)
            {
                if (other.GetComponent<Zombie>().dead) return;
            }
            else if (other.GetComponent<TinyZombieBoat>().gameObject != null)
            {
                if (other.GetComponent<TinyZombieBoat>().dead) return;
            }

            hasLost = true;
            source.PlayOneShot(Scream);
            source.PlayOneShot(loseSFX);
            music.Stop();
            ani.Play("DeathAni");
            Invoke("RestartScene", 5);
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
