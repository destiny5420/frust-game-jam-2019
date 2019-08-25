using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health02Controller : NetworkBehaviour
{
    [SerializeField] Animation m_anim;
    float m_fSpeedUnit = 15.0f;
    
    void Start()
    {
        m_anim.Play("Health02_Drop");
    }

    void SetSpeed(float v_value)
    {
        m_fSpeedUnit = v_value;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.down * Time.deltaTime * m_fSpeedUnit, Space.Self);
    }

    void Play(string v_clipName)
    {
        m_anim.Play(v_clipName);
    }

    void Dest()
    {
        Destroy(gameObject, 5.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Plane")
        {
            Debug.Log("Hit " + other.gameObject.name);
            SetSpeed(0.0f);
            m_anim.Play();
            CameraController.Instance.InduceStress(0.5f);
            Destroy(gameObject, 3.0f);
        }
    }
}
