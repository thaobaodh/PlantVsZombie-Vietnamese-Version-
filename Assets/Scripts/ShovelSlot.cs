using UnityEngine;
using UnityEngine.UI;

public class ShovelSlot : MonoBehaviour
{
    private GameManager gms;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Shovel Slot init");
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
