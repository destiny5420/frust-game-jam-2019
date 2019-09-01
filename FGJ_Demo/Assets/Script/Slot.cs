using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {


    private Inventory inventory;
    public int index;
    public bool OpenGame=false;
    private void Start()//需要開始後更新
    {
        
        //inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void Update()//需要開始後更新
    {
        //if (inventory = null&& GameObject.FindGameObjectWithTag("Player")!=null) ;
        if (OpenGame) inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); ;

        if (OpenGame&&transform.childCount <= 0) {
            inventory.items[index] = 0;
        }
    }

    public void Cross() {

        foreach (Transform child in transform) {
            child.GetComponent<Spawn>().SpawnItem();
            GameObject.Destroy(child.gameObject);
        }
    }

}
