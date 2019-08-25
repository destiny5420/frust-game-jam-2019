using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic01Controller : MonoBehaviour
{
    float m_fSpeedUnit = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * m_fSpeedUnit);
    }

    void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Collider>().tag == "Floor")
        {
            Debug.Log("Hit floor");
        }
    }
}
