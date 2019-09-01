using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pla_2 : MonoBehaviour
{
    
    public GameObject 功能二;
    bool aaaaa =false;
    void Start()
    {
        if (!功能二)//抓燈光 或燈光開UI
        {
            GameObject.Find("Directional Light").GetComponent<Pla_2>().aaaaa = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (aaaaa) { aaaaa = false; 功能二.SetActive(true);
            GameObject.Find("slot1").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot2").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot3").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot4").GetComponent<Slot>().OpenGame = true;
        }
    }
}
