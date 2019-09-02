using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    private Transform playerPos;
    public GameObject item;

    private void Start()
    {
        if (!playerPos  && GameObject.Find("Directional Light").GetComponent<Pla_2>().遊戲狀態 == 1)
            playerPos = GameObject.FindGameObjectWithTag("PlayerPos").GetComponent<Transform>();
    }

    public void SpawnItem() {
        Instantiate(item, playerPos.position, Quaternion.identity);
    }
}
