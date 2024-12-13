using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BlessingLight : MonoBehaviour
{
    private AudioSource source;

    [SerializeField] AudioClip[] explodedClip;

    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
