using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pla_2 : MonoBehaviour ///顯示狀態
{
    
    public GameObject Canvas_UI; 
    bool GameStart =false;
    public int 遊戲狀態=0; // 0 未進入 1Enter
    void Awake()
    {
        if (!Canvas_UI)//抓燈光 
        {
            GameObject.Find("Directional Light").GetComponent<Pla_2>().GameStart = true;
        }
    } 
    void Update()
    {
        if (GameStart) { GameStart = false; Canvas_UI.SetActive(true); //用燈光 開 UI 
            遊戲狀態 = 1;
            GameObject.Find("slot1").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot2").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot3").GetComponent<Slot>().OpenGame = true;
            GameObject.Find("slot4").GetComponent<Slot>().OpenGame = true;
        }
    }
}
