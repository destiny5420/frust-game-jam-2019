using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

	}

	void OnCollisionEnter(Collision collision)
	{
		GameObject hit = collision.gameObject;
		health hp = hit.GetComponent<health>();
		if (hp != null)
		{
			hp.TakeDamage(10);
		}

		Destroy(gameObject);
	}
}
