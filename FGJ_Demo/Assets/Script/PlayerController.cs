using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	public Camera m_camFollow;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
		FollowMouse();

		switch (netId.Value)
		{
			case 1:
				GetComponent<MeshRenderer>().material.color = Color.blue;
				break;
			case 2:
				GetComponent<MeshRenderer>().material.color = Color.red;
				break;
			case 3:
				GetComponent<MeshRenderer>().material.color = Color.green;
				break;
			case 4:
				GetComponent<MeshRenderer>().material.color = Color.white;
				break;
		}

		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}

		float x = Input.GetAxis("Horizontal") * Time.deltaTime * playerMoveSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * playerMoveSpeed;

		//// transform.Rotate(0, x, 0);
		//transform.Translate(0, 0, z);
		transform.position += new Vector3(x, 0, z);

		if(Input.GetKey(KeyCode.KeypadPlus))
		{
			if (playerMoveSpeed < 10)
				playerMoveSpeed += 0.1f;
			else
				playerMoveSpeed = 10;
		}
		if(Input.GetKey(KeyCode.KeypadMinus))
		{
			if (playerMoveSpeed > 0.1f)
				playerMoveSpeed -= 0.1f;
			else
				playerMoveSpeed = 0.1f;
		}

		if(Input.GetKeyDown(KeyCode.C))
		{
			if(bJump == false)
			{
				bJump = true;
			}
		}
		JumpBehavior();
	}

	bool bJump = false;
	float fJumpTime = 0.0f;
	float fJumpLimitTime = 0.3f;

	void JumpBehavior()
	{
		if(bJump == true)
		{
			fJumpTime += Time.deltaTime;
			transform.position += new Vector3(0, Time.deltaTime * 20.0f, 0);
			if (fJumpLimitTime < fJumpTime)
				bJump = false;
		}
	}

	float playerMoveSpeed = 3.0f;
	Vector3 mousePos;

	void FollowMouse()
	{
		if (m_camFollow == null)
			return;

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;
		
		LayerMask floorMask = 1<< 9;

		if(Physics.Raycast (camRay, out floorHit, 1000, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;

			playerToMouse.y = transform.position.y;
			transform.LookAt(playerToMouse);
		}
	}

	void Fire()
	{
		// Create the Bullet from the Bullet Prefab
		GameObject bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}

	public override void OnStartLocalPlayer()
	{
		// self
		this.transform.position += (Vector3.forward * netId.Value);
	}

	[Command]
	void CmdFire()
	{
		// Create the Bullet from the Bullet Prefab
		GameObject bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
