using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Magic03Controller : NetworkTransform
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

    public void SetSpeed(float v_value)
    {
        m_fSpeedUnit = v_value;
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

            CameraController.Instance.InduceStress(4.0f);

            PrefabManager.Instance.CmdSpawnMagic(data2);
            Destroy(gameObject);
        }
    }
}
