using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController m_clsPlayerController;

    void Start()
    {
        
    }

    void Update()
    {
        FindPlayerTarget();
    }

    void FindPlayerTarget()
    {
        if (m_clsPlayerController != null)
            return;

        object[] arrayObj = GameObject.FindObjectsOfType<PlayerController>();
        
        for (int i = 0; i < arrayObj.Length; i++)
        {
            NetworkBehaviour player = arrayObj[i] as NetworkBehaviour;
            if (player.isLocalPlayer == true)
            {
                m_clsPlayerController = (PlayerController)player;
                break;
            }
        }
    }
}
