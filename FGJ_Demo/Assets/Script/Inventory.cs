using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int[] items;

    public bool[] isFull;
    public GameObject[] slots;
    private void Start()
    {
        StartCoroutine(Example());
        
    }
    IEnumerator Example()
    { 
        yield return new WaitForSeconds(1f);
        slots[0] = GameObject.Find("slot1");
        slots[1] = GameObject.Find("slot2");
        slots[2] = GameObject.Find("slot3");
        slots[3] = GameObject.Find("slot4");
    }
}
