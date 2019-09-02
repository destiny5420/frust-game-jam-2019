using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pla_2 : MonoBehaviour ///顯示狀態
{
    
    public GameObject 功能二;
    bool aaaaa =false;
    public int 遊戲狀態=0; // 0未進入 1Enter
    void Awake()
    {
        if (!功能二)//抓燈光 
        {
            GameObject.Find("Directional Light").GetComponent<Pla_2>().aaaaa = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (aaaaa) { aaaaa = false; 功能二.SetActive(true); //燈光開Canvas
            遊戲狀態 = 1;
            GameObject.Find("slot1").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot2").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot3").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot4").GetComponent<Slot>().OpenGame = true;
        }
    }
}
