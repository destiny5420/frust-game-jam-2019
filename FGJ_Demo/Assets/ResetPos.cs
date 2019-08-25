using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
	Transform m_transform;
    // Start is called before the first frame update
    void Start()
    {
		m_transform = this.transform;
	}

    // Update is called once per frame
    void Update()
    {
		if (m_transform.localPosition != Vector3.zero)
			m_transform.localPosition = Vector3.zero;
    }
}
