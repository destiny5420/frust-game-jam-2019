using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController m_clsPlayerController;
    Vector3 m_v3CameraDisWithPlayer = new Vector3(0.0f, 10.0f, -18.0f);
    float m_fRotX = 30.0f;

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
                m_clsPlayerController.m_camFollow = GetComponent<Camera>();
                break;
            }
        }
    }

    void LateUpdate()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        if (m_clsPlayerController == null)
            return;

        gameObject.transform.position = m_clsPlayerController.transform.position + m_v3CameraDisWithPlayer;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(m_fRotX, 0.0f, 0.0f));
    }
}
