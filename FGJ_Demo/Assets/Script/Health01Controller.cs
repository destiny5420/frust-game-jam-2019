using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health01Controller : MonoBehaviour
{
    float m_fSpeedUnit = 10.0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * m_fSpeedUnit);
    }
}
