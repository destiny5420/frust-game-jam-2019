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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Well" || other.tag == "Player")
        {
            Debug.Log("Hit " + other.gameObject.name);
            //PrefabManager.Instance.CmdSpawnMagic(PrefabManager.MAGIC_TYPE.Magic01Hit, transform.position);
            Destroy(gameObject);
        }
    }
}
