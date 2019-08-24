using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	public Camera m_camFollow;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public Transform m_tranMouse;
	public MouseController m_clsMouseController;
	public MeshRenderer selfTargetSign;
	Vector3 m_v3CurMouseHitPoint;
	Health m_Health;

	public bool bBoss = false;

	void Start()
    {
		m_Health = this.GetComponent<Health>();
	}

	// Update is called once per frame
	void Update()
	{

		if(bBoss != true)
		{
			switch ((netId.Value%4)+1)
			{
				case 1:
					GetComponent<MeshRenderer>().material.color = Color.blue;
					break;
				case 2:
					GetComponent<MeshRenderer>().material.color = new Color(160.0f / 255.0f, 32.0f / 255.0f, 240.0f / 255.0f);
					break;
				case 3:
					GetComponent<MeshRenderer>().material.color = Color.green;
					break;
				case 4:
					GetComponent<MeshRenderer>().material.color = Color.white;
					break;
			}
		}

		if (!isLocalPlayer)
			return;

		if(Input.GetKeyDown(KeyCode.R))
		{
			m_Health.currentHealth = 100;
			return;
		}

		if (m_Health.currentHealth <= 0)
			return;

		FollowMouse();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}

		float x = Input.GetAxis("Horizontal") * Time.deltaTime * playerMoveSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * playerMoveSpeed;
		//if (Mathf.Abs(x) < 0.1f && Mathf.Abs(z) < 0.1f)
		//{
		//	playerMoveSpeed = playerStartMoveSpeed;
		//}
		transform.position += new Vector3(x, 0, z);
		//if (playerMoveSpeed < playerMoveSpeedLimit)
		//{
		//	playerMoveSpeed += Time.deltaTime * 20.0f;
		//}

		if(Input.GetKey(KeyCode.KeypadPlus))
		{
			if (playerMoveSpeedLimit < 20)
				playerMoveSpeedLimit += 0.1f;
			else
				playerMoveSpeedLimit = 10;
		}
		if(Input.GetKey(KeyCode.KeypadMinus))
		{
			if (playerMoveSpeedLimit > 0.1f)
				playerMoveSpeedLimit -= 0.1f;
			else
				playerMoveSpeedLimit = 0.1f;
		}

		if(Input.GetKeyDown(KeyCode.C))
		{
			if(bJump == false)
			{
				bJump = true;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.F1))
		{
			PrefabManager.Instance.SpawnMagic(PrefabManager.MAGIC_TYPE.Magic01, m_v3CurMouseHitPoint);
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			PrefabManager.Instance.SpawnMagic(PrefabManager.MAGIC_TYPE.Health01, m_v3CurMouseHitPoint);
		}

		JumpBehavior();

		if(Input.GetKeyDown(KeyCode.F9))
		{
			bBoss = true;
			GameSetting.bBossMode = true;
			GetComponent<MeshRenderer>().material.color = Color.red;
		}
	}

	bool bJump = false;
	float fJumpTime = 0.0f;
	float fJumpLimitTime = 0.3f;

	void JumpBehavior()
	{
		if(bJump == true)
		{
			if (fJumpLimitTime < fJumpTime)
			{
				transform.position -= new Vector3(0, Time.deltaTime * 20.0f, 0);
			}
			else
			{
				fJumpTime += Time.deltaTime;
				transform.position += new Vector3(0, Time.deltaTime * 20.0f, 0);
			}
		}
	}

	float playerStartMoveSpeed = 3.0f;
	float playerMoveSpeed = 10.0f;
	float playerMoveSpeedLimit = 10.0f;
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
			m_v3CurMouseHitPoint = floorHit.point;
			Vector3 playerToMouse = floorHit.point - transform.position;
			
			SetPosToHitPointByMouse(m_v3CurMouseHitPoint);
			//Debug.Log("Hit point: " + floorHit.point + " / transform.position: " + transform.position + " / Result: " + playerToMouse);
			playerToMouse.y = 0;

			Quaternion rot = Quaternion.LookRotation(playerToMouse, Vector3.zero);

			transform.rotation = rot;
		}
	}

	void SetPosToHitPointByMouse(Vector3 v_pos)
	{
		m_tranMouse.position = v_pos;
	}

	public override void OnStartLocalPlayer()
	{
		// self
		this.transform.position += (Vector3.forward * netId.Value);

		selfTargetSign.enabled = true;

		switch ((netId.Value % 4) + 1)
		{
			case 1:
				GetComponent<MeshRenderer>().material.color = Color.blue;
				break;
			case 2:
				GetComponent<MeshRenderer>().material.color = new Color(160.0f / 255.0f, 32.0f / 255.0f, 240.0f / 255.0f);
				break;
			case 3:
				GetComponent<MeshRenderer>().material.color = Color.green;
				break;
			case 4:
				GetComponent<MeshRenderer>().material.color = Color.white;
				break;
		}

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
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20.0f;
		bullet.GetComponent<bullet>().playerID = (int)netId.Value;
		bullet.GetComponent<bullet>().bBoss = bBoss;
		Physics.IgnoreCollision(bullet.GetComponent<Collider>(), this.GetComponent<Collider>());

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter");
		ResetJump();
	}

	void ResetJump()
	{
		if(bJump == true)
		{
			bJump = false;
			fJumpTime = 0.0f;
		}
	}
}
