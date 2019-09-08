using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagButton : MonoBehaviour
{
    public GameObject SlotPanel;
    public GameObject IconPanel;
    public bool isOpen = true; //slot
    public GameObject NumbIcon;
    int OpenCheck = 0;
    public void BagClick() {
        if (!isOpen){ isOpen = true; SlotPanel.SetActive(true); IconPanel.SetActive(false); NumbIcon.SetActive(false); //開啟背包
        }else
        {
            isOpen = false; SlotPanel.SetActive(false); IconPanel.SetActive(true); NumbIcon.SetActive(true); //開啟圖示
        }
        
    } 
    void Update()
    { 
        if (isOpen && OpenCheck == 0)
        {
            StopAllCoroutines();
            StartCoroutine(CheckClose());
        }
        else if(!isOpen){ OpenCheck = 0; StopAllCoroutines(); }
        if (isOpen) { OpenCheck++; }
    }
    IEnumerator CheckClose() {
        
        yield return new WaitForSeconds(3f);
        if (isOpen&& OpenCheck!=0) { isOpen = false; SlotPanel.SetActive(false); IconPanel.SetActive(true); NumbIcon.SetActive(true); } 
    }
}
