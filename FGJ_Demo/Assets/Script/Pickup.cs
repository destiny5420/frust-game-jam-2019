using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemButton;
    public int aa =0; //不能隱藏
    void Update()
    {
        //aa = ;
        if (aa==0 &&GameObject.Find("Directional Light").GetComponent<Pla_2>().遊戲狀態 == 1)
        {
            aa = 2;
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }

    }
     
    void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < inventory.slots.Length; i++)
                if (inventory.items[i] == 0)
                { 
                    inventory.items[i] = 1;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break; 
                }
        }
    }
}
