using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health01Controller : NetworkTransform
{
    float m_fSpeedUnit = 10.0f;

    void Start()
    {
        Destroy(gameObject, 10.0f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * m_fSpeedUnit, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Well" || other.tag == "Player")
        {
            Debug.Log("Hit " + other.gameObject.name);


			Health hp = other.GetComponent<Health>();
			if (hp != null)
			{
				hp.currentHealth += 20;
				if (hp.currentHealth > 100)
					hp.currentHealth = 100;
			}
				//         PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
				//data.magicType = PrefabManager.MAGIC_TYPE.Magic01Hit;
				//data.targetPos = transform.position;
				//         PrefabManager.Instance.CmdSpawnMagic(data);
				Destroy(gameObject);
        }
    }
}
