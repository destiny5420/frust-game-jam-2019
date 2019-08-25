using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrefabManager : NetworkBehaviour
{
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
	public void CmdSpawnMagic(MAGIC_TYPE v_type, Vector3 v_pos, Transform r_transform = null)
    {
		GameObject temp = null;
		switch (v_type)
        {
            case MAGIC_TYPE.Magic01:
                if (m_bMagicCD == true) return;
				temp = (GameObject)Instantiate(m_objMagic01, v_pos + new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity);    
                m_bMagicCD = true;
                m_fMagicCDClick = 0.0f;
				break;
            case MAGIC_TYPE.Magic02:
                break;
            case MAGIC_TYPE.Magic03:
                break;
            case MAGIC_TYPE.Magic01Hit:

                
                temp = (GameObject)Instantiate(m_objMagic01_Hit, v_pos , Quaternion.identity);
                break;
            case MAGIC_TYPE.Health01:
                Vector3 v3Target = v_pos;
                Vector3 v3Start = r_transform.position;
                Vector3 v3Result = (v3Target - v3Start).normalized;
                Debug.Log("v3Target: " + v3Target + " / v3Start: " + v3Start + " / v3Result: " + v3Result);
                temp = (GameObject)Instantiate(m_objHealth01, r_transform.position + r_transform.forward * 1.5f , Quaternion.Euler(v3Result));
                break;
            case MAGIC_TYPE.Health02:
				temp = (GameObject)Instantiate(m_objHealth02, v_pos, Quaternion.identity);
                break;
            case MAGIC_TYPE.Health03:
				temp = (GameObject)Instantiate(m_objHealth03, v_pos, Quaternion.identity);
                break;
            default:
                break;
		}

		if(temp != null)
			NetworkServer.Spawn(temp);
	}
}
