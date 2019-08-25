using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrefabManager : NetworkBehaviour
{
    public struct udsPrefabData
    {
        public MAGIC_TYPE magicType;
        public Vector3 targetPos;
        public Vector3 forward;
        public Vector3 startPos;
    }

    public enum MAGIC_TYPE
    {
        Magic01 = 0,
        Magic02,
        Magic03,
        Magic01Hit,
        Health01,
        Health02,
        Health03,
    }

    static PrefabManager instance;
    public static PrefabManager Instance
    {
        get{ return instance; }
    }

    public GameObject m_objMagic01;
    public GameObject m_objMagic02;
    public GameObject m_objMagic03;
    public GameObject m_objMagic01_Hit;
    public GameObject m_objHealth01;
    public GameObject m_objHealth02;
    public GameObject m_objHealth03;

    bool m_bMagicCD = false;
    const float MAGIC_01_TIME = 3.0f;
    float m_fMagicCDClick = 0.0f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (m_bMagicCD == true)
        {
            m_fMagicCDClick += Time.deltaTime;
            if (m_fMagicCDClick >= MAGIC_01_TIME)
            {
                m_bMagicCD = false;
                m_fMagicCDClick = 0.0f;
            }
        }
    }

	[Command]
	public void CmdSpawnMagic(udsPrefabData v_data)
    {
		GameObject temp = null;
		switch (v_data.magicType)
        {
            case MAGIC_TYPE.Magic01:
                if (m_bMagicCD == true) return;
				temp = (GameObject)Instantiate(m_objMagic01, v_data.targetPos + new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity);    
                m_bMagicCD = true;
                m_fMagicCDClick = 0.0f;
				break;
            case MAGIC_TYPE.Magic02:
                break;
            case MAGIC_TYPE.Magic03:
                break;
            case MAGIC_TYPE.Magic01Hit:  
                temp = (GameObject)Instantiate(m_objMagic01_Hit, v_data.targetPos , Quaternion.identity);
                break;
            case MAGIC_TYPE.Health01:
                Vector3 v3Target = v_data.targetPos;
                Vector3 v3Start = v_data.startPos;
                Vector3 v3Result = (v3Target - v3Start).normalized;
                Debug.Log("v3Target: " + v3Target + " / v3Start: " + v3Start + " / v3Result: " + v3Result);
                temp = (GameObject)Instantiate(m_objHealth01, v_data.targetPos + v_data.forward * 1.5f , Quaternion.Euler(v3Result));
                break;
            case MAGIC_TYPE.Health02:
				temp = (GameObject)Instantiate(m_objHealth02, v_data.targetPos, Quaternion.identity);
                break;
            case MAGIC_TYPE.Health03:
				temp = (GameObject)Instantiate(m_objHealth03, v_data.targetPos, Quaternion.identity);
                break;
            default:
                break;
		}

		if(temp != null)
			NetworkServer.Spawn(temp);
	}
}
