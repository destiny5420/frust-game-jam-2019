using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController m_clsPlayerController;
    Vector3 m_v3CameraDisWithPlayer = new Vector3(0.0f, 10.0f, -18.0f);
    float m_fRotX = 30.0f;

    [SerializeField] float frequency = 25;
    bool m_bEnablShakeing;
    [SerializeField] Vector3 maximumTranslationShake = Vector3.one * 0.5f;
    [SerializeField] Vector3 maximumAngularShake = Vector3.one * 2;
    [SerializeField] float recoverySpeed = 1.5f;
    private float trauma = 0;
    private float seed;
    [SerializeField] float traumaExponent = 2;

    static CameraController instance;
    public static CameraController Instance
    {
        get{ return instance; }
    }

    private void Awake()
    {
        instance = this;
        seed = Random.value;
    }

    void Start()
    {
        
    }

    void Update()
    {
        FindPlayerTarget();
    }

    void CamerShakeUpdate()
    {
        if (m_bEnablShakeing == false)
            return;

        if (trauma == 0)
            m_bEnablShakeing = false;

        float shake = Mathf.Pow(trauma, traumaExponent);

        // Modify the existing code.
        Vector3 v3ExtraPos = new Vector3(
            maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1),
            maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1),
            maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
        ) * shake;

        transform.position += v3ExtraPos;
        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
        // transform.localRotation = Quaternion.Euler(new Vector3(
        //     maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
        //     maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
        //     maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
        // ) * shake);
    }

    public void InduceStress(float stress)
    {
        trauma = Mathf.Clamp01(trauma + stress);
        m_bEnablShakeing = true;
    }

    void FindPlayerTarget()
    {
        if (m_clsPlayerController != null)
            return;

        object[] arrayObj = GameObject.FindObjectsOfType<PlayerController>();
        
        for (int i = 0; i < arrayObj.Length; i++)
        {
            NetworkBehaviour player = arrayObj[i] as NetworkBehaviour;
            if (player.isLocalPlayer == true)
            {
                m_clsPlayerController = (PlayerController)player;
                m_clsPlayerController.m_camFollow = GetComponent<Camera>();
                break;
            }
        }
    }

    void LateUpdate()
    {
        CameraFollow();
        CamerShakeUpdate();
    }

    void CameraFollow()
    {
        if (m_clsPlayerController == null)
            return;

        gameObject.transform.position = m_clsPlayerController.transform.position + m_v3CameraDisWithPlayer;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(m_fRotX, 0.0f, 0.0f));
    }
}
