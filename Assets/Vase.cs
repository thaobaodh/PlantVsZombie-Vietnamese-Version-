using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    public GameObject objectInVase; 

    public Texture2D hammer;

    public Texture2D hammerDown;

    public bool isBreak = false;

    public Animator animator;

    private AudioSource source;

    public AudioClip[] audioClips;

    //private void OnMouseOver()
    //{
    //    source = gameObject.AddComponent<AudioSource>();
    //    Cursor.SetCursor(hammer, Vector2.zero, CursorMode.ForceSoftware);
    //}

    //private void OnMouseExit()
    //{
    //    //Resets the cursor to the default  
    //    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    //}

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        animator.Play("Idle");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isBreak) return;
        source.PlayOneShot(audioClips[0]);
        animator.Play("Break");
        Instantiate(objectInVase, transform.position, Quaternion.identity);
        Destroy(gameObject, 1);
        isBreak = false;
    }
}
