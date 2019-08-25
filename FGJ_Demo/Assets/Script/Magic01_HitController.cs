using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic01_HitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2.0f);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter");
		GameObject hit = other.gameObject;
		Debug.Log("other " + other.gameObject.name);
		Health hp = hit.GetComponent<Health>();
		if (hp != null)
		{
			hp.TakeDamage(45);
		}
	}
}
