using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public enum MAGIC_TYPE
    {
        Magic01 = 0,
        Magic02,
        Magic03
    }

    static PrefabManager instance;
    public static PrefabManager Instance
    {
        get{ return instance; }
    }

    public GameObject m_objMagic01;
    public GameObject m_objMagic02;
    public GameObject m_objMagic03;

    void Awake()
    {
        instance = this;
    }

    public void SpawnMagic(MAGIC_TYPE v_type, Vector3 v_pos)
    {
        switch (v_type)
        {
            case MAGIC_TYPE.Magic01:
                GameObject.Instantiate(m_objMagic01, v_pos, Quaternion.identity);
                break;
            case MAGIC_TYPE.Magic02:
                break;
            case MAGIC_TYPE.Magic03:
                break;
            default:
                break;
        }
    }
}
