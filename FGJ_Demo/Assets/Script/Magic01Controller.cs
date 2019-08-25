using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic01Controller : MonoBehaviour
{
    float m_fSpeedUnit = 15.0f;
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
        if (other.tag == "Floor")
        {
            Debug.Log("Hit floor");

            PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
			data.magicType = PrefabManager.MAGIC_TYPE.Magic01Hit;
			data.targetPos = transform.position;

            PrefabManager.Instance.CmdSpawnMagic(data);

            PrefabManager.udsPrefabData data2 = new PrefabManager.udsPrefabData();
			data2.magicType = PrefabManager.MAGIC_TYPE.Magic02;
			data2.targetPos = transform.position;

            PrefabManager.Instance.CmdSpawnMagic(data2);
            Destroy(gameObject);
        }
    }
}
