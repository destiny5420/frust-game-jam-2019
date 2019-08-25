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
	public MeshRenderer selfTargetSign;
	Vector3 m_v3CurMouseHitPoint;
	Health m_Health;

	public bool bBoss = false;

	public GameObject particlePrefab;
	public Transform particleSpawn;

	// model
	public GameObject Sword;
	public GameObject Staff;
	public GameObject Gun;
	public GameObject Drink;
	public Animator Ani;
	public float weapon;
	public bool running;

	public Material[] m_mat;
	public SkinnedMeshRenderer renderer;

	bool bDie = false;

	public Attritube m_Attritube;

	void Start()
    {
		m_Health = this.GetComponent<Health>();

		// model
		Sword.SetActive(false);
		Staff.SetActive(false);
		Gun.SetActive(false);
		Drink.SetActive(false);

		m_Attritube.m_dead = DeadStart;

		if (isLocalPlayer == false)
			selfTargetSign.enabled = false;
	}

	void Update()
	{
		if(bDie == false)
		{
			if(m_Health.currentHealth <= 0)
			{
				Ani.SetBool("Dead", true);
				bDie = true;
			}
		}

		if(bBoss != true)
		{
			switch ((netId.Value % 4)+1)
			{
				case 1:
					renderer.material = m_mat[0];
					GetComponent<MeshRenderer>().material.color = Color.blue;
					break;
				case 2:
					renderer.material = m_mat[1];
					GetComponent<MeshRenderer>().material.color = new Color(160.0f / 255.0f, 32.0f / 255.0f, 240.0f / 255.0f);
					break;
				case 3:
					renderer.material = m_mat[2];
					GetComponent<MeshRenderer>().material.color = Color.green;
					break;
				case 4:
					renderer.material = m_mat[3];
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

		ModelUpdate();

		FollowMouse();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (bJump == false)
			{
				bJump = true;
			}
		}

		float x = Input.GetAxis("Horizontal") * Time.deltaTime * playerMoveSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * playerMoveSpeed;
		if (Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f)
		{
			running = true;
		}
		else
		{
			running = false;
		}

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

		}
		
		if (Input.GetKeyDown(KeyCode.F1))
		{
			PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
			data.magicType = PrefabManager.MAGIC_TYPE.Magic01;
			data.targetPos = m_v3CurMouseHitPoint;
			PrefabManager.Instance.CmdSpawnMagic(data);
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
			data.magicType = PrefabManager.MAGIC_TYPE.Health01;
			data.targetPos = m_v3CurMouseHitPoint;
			data.forward = transform.forward;
			data.startPos = transform.position;

			PrefabManager.Instance.CmdSpawnMagic(data);
		}

		if (Input.GetKeyDown(KeyCode.F3))
		{
			PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
			data.magicType = PrefabManager.MAGIC_TYPE.Magic03;
			data.targetPos = m_v3CurMouseHitPoint;
			PrefabManager.Instance.CmdSpawnMagic(data);
		}

		JumpBehavior();

		if (Input.GetKeyDown(KeyCode.F9))
		{
			bBoss = true;
			GameSetting.bBossMode = true;
			SyncGameSetting.GetInstance.bBoss = true;
			SyncGameSetting.GetInstance.iCurrentBossId = netId.Value;
			GetComponent<MeshRenderer>().material.color = Color.red;
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (weapon == 1 && m_Attritube.IsAttack == false)
			{
				Ani.SetTrigger("Attack");
			}
			else if (weapon == 2 && m_Attritube.IsAttack == false)
			{
				Ani.SetTrigger("MagicAttack");
				PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
				data.magicType = PrefabManager.MAGIC_TYPE.Magic01;
				data.targetPos = m_v3CurMouseHitPoint;
				PrefabManager.Instance.CmdSpawnMagic(data);
			}
			else if (weapon == 3 && m_Attritube.IsAttack == false)
			{
				Ani.SetTrigger("GunAttack");
				CmdFire();
			}
			else if (weapon == 4 && m_Attritube.IsAttack == false)
			{
				Ani.SetTrigger("Drink");
			}
			else
			{
			}
		}
	}

	void ModelUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			weapon = 1;
			Ani.SetFloat("Weapon", weapon);
			Ani.SetBool("HaveWeapon", true);
			Sword.SetActive(true);
			Staff.SetActive(false);
			Gun.SetActive(false);
			Drink.SetActive(false);

		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			weapon = 2;
			Ani.SetFloat("Weapon", weapon);
			Ani.SetBool("HaveWeapon", true);
			Staff.SetActive(true);
			Sword.SetActive(false);
			Gun.SetActive(false);
			Drink.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			weapon = 3;
			Ani.SetFloat("Weapon", weapon);
			Ani.SetBool("HaveWeapon", true);
			Staff.SetActive(false);
			Sword.SetActive(false);
			Gun.SetActive(true);
			Drink.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			weapon = 4;
			Ani.SetFloat("Weapon", weapon);
			Ani.SetBool("HaveWeapon", true);
			Staff.SetActive(false);
			Sword.SetActive(false);
			Gun.SetActive(false);
			Drink.SetActive(true);
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			weapon = 0;
			Ani.SetFloat("Weapon", weapon);
			Ani.SetBool("HaveWeapon", false);
			Staff.SetActive(false);
			Sword.SetActive(false);
			Gun.SetActive(false);
			Drink.SetActive(false);
		}
		
		//if (Input.GetMouseButtonDown(0) && weapon == 1)
		//{
		//	Ani.SetTrigger("Attack");
		//}
		//if (Input.GetMouseButtonDown(0) && weapon == 2)
		//{
		//	Ani.SetTrigger("MagicAttack");
		//}
		//if (Input.GetMouseButtonDown(0) && weapon == 3)
		//{
		//	Ani.SetTrigger("GunAttack");
		//}
		//if (Input.GetMouseButtonDown(0) && weapon == 4)
		//{
		//	Ani.SetTrigger("Drink");
		//}

		if (running)
		{
			Ani.SetBool("Run", true);
		}
		else
			Ani.SetBool("Run", false);
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
		this.transform.position += (Vector3.forward * netId.Value) + Vector3.up*0.5f;

		selfTargetSign.enabled = true;

		switch ((netId.Value % 4) + 1)
		{
			case 1:
				renderer.material = m_mat[0];
				GetComponent<MeshRenderer>().material.color = Color.blue;
				break;
			case 2:
				renderer.material = m_mat[1];
				GetComponent<MeshRenderer>().material.color = new Color(160.0f / 255.0f, 32.0f / 255.0f, 240.0f / 255.0f);
				break;
			case 3:
				renderer.material = m_mat[2];
				GetComponent<MeshRenderer>().material.color = Color.green;
				break;
			case 4:
				renderer.material = m_mat[3];
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
		if (other.tag != "Floor")
			return;
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
	
	public delegate void deadFunc();

	public void DeadStart()
	{
		Ani.SetBool("Dead", false);
	}

}
