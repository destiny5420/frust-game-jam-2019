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

	[SyncVar]
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

		WeaponChange_Update();
		AttackUpdate();
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
			PrefabManager.Instance.SpawnMagic(data);
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
			data.magicType = PrefabManager.MAGIC_TYPE.Health01;
			data.targetPos = m_v3CurMouseHitPoint;
			data.forward = transform.forward;
			data.startPos = transform.position;

			PrefabManager.Instance.SpawnMagic(data);
		}

		if (Input.GetKeyDown(KeyCode.F3))
		{
			PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
			data.magicType = PrefabManager.MAGIC_TYPE.Magic03;
			data.targetPos = m_v3CurMouseHitPoint;
			PrefabManager.Instance.SpawnMagic(data);
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

	}

	void WeaponChange_Update()
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

			Sync_Change_Weapon1();

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

			Sync_Change_Weapon2();
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

			Sync_Change_Weapon3();
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

			Sync_Change_Weapon4();
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			weapon = 5;
			Ani.SetFloat("Weapon", weapon);
			Ani.SetBool("HaveWeapon", true);
			Staff.SetActive(false);
			Sword.SetActive(false);
			Gun.SetActive(false);
			Drink.SetActive(false);

			Sync_Change_Weapon5();
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

			Sync_Change_Weapon0();
		}

		if (running)
		{
			// Do Self
			Ani.SetBool("Run", true);
			// Send To Server
			Sync_StartMove();
		}
		else
		{
			// Do Self
			Ani.SetBool("Run", false);
			// Send To Server
			Sync_StopMove();
		}
	}

	void AttackUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (weapon == 1 && m_Attritube.IsAttack == false)
			{
				Ani.SetTrigger("Attack");
				Sync_SwordAttack();
			}
			else if (weapon == 2 && m_Attritube.IsAttack == false)
			{
				Ani.SetTrigger("MagicAttack");
				Sync_MagicAttack(m_v3CurMouseHitPoint);
			}
			else if (weapon == 3 && m_Attritube.IsAttack == false)
			{
				Ani.SetTrigger("GunAttack");
				Sync_BulletFire();
			}
			else if (weapon == 4 && m_Attritube.IsAttack == false)
			{
				Debug.LogWarning("Drink");
				// Do Self
				Ani.SetTrigger("Drink");
				// Send To Server
				Sync_DrinkMedical();
				Debug.LogWarning("Drink End");
			}
			else if (weapon == 5 && m_Attritube.IsAttack == false)
			{
				Sync_HealthFire();
			}
			else
			{
			}
		}
	}
	#region Net Sync List

	#region ChangeWeapon
	void Sync_Change_Weapon0()
	{
		if(isServer)
			Rpc_Change_Weapon0();
		else
			Cmd_Change_Weapon0();
	}
	[ClientRpc]
	void Rpc_Change_Weapon0()
	{
		Change_Weapon0();
	}
	[Command]
	void Cmd_Change_Weapon0()
	{
		Change_Weapon0();
	}
	void Change_Weapon0()
	{
		weapon = 0;
		Ani.SetFloat("Weapon", weapon);
		Ani.SetBool("HaveWeapon", false);
		Staff.SetActive(false);
		Sword.SetActive(false);
		Gun.SetActive(false);
		Drink.SetActive(false);
	}

	void Sync_Change_Weapon1()
	{
		if (isServer)
			Rpc_Change_Weapon1();
		else
			Cmd_Change_Weapon0();
	}
	[ClientRpc]
	void Rpc_Change_Weapon1()
	{
		Change_Weapon1();
	}
	[Command]
	void Cmd_Change_Weapon1()
	{
		Change_Weapon1();
	}
	void Change_Weapon1()
	{
		weapon = 1;
		Ani.SetFloat("Weapon", weapon);
		Ani.SetBool("HaveWeapon", true);
		Sword.SetActive(true);
		Staff.SetActive(false);
		Gun.SetActive(false);
		Drink.SetActive(false);
	}

	void Sync_Change_Weapon2()
	{
		if (isServer)
			Rpc_Change_Weapon2();
		else
			Cmd_Change_Weapon2();
	}
	[ClientRpc]
	void Rpc_Change_Weapon2()
	{
		Change_Weapon2();
	}
	[Command]
	void Cmd_Change_Weapon2()
	{
		Change_Weapon2();
	}
	void Change_Weapon2()
	{
		weapon = 2;
		Ani.SetFloat("Weapon", weapon);
		Ani.SetBool("HaveWeapon", true);
		Staff.SetActive(true);
		Sword.SetActive(false);
		Gun.SetActive(false);
		Drink.SetActive(false);
	}

	void Sync_Change_Weapon3()
	{
		if (isServer)
			Rpc_Change_Weapon3();
		else
			Cmd_Change_Weapon3();
	}
	[ClientRpc]
	void Rpc_Change_Weapon3()
	{
		Change_Weapon3();
	}
	[Command]
	void Cmd_Change_Weapon3()
	{
		Change_Weapon3();
	}
	void Change_Weapon3()
	{
		weapon = 3;
		Ani.SetFloat("Weapon", weapon);
		Ani.SetBool("HaveWeapon", true);
		Staff.SetActive(false);
		Sword.SetActive(false);
		Gun.SetActive(true);
		Drink.SetActive(false);
	}

	void Sync_Change_Weapon4()
	{
		if (isServer)
			Rpc_Change_Weapon4();
		else
			Cmd_Change_Weapon4();
	}
	[ClientRpc]
	void Rpc_Change_Weapon4()
	{
		Change_Weapon4();
	}
	[Command]
	void Cmd_Change_Weapon4()
	{
		Change_Weapon4();
	}
	void Change_Weapon4()
	{
		weapon = 4;
		Ani.SetFloat("Weapon", weapon);
		Ani.SetBool("HaveWeapon", true);
		Staff.SetActive(false);
		Sword.SetActive(false);
		Gun.SetActive(false);
		Drink.SetActive(true);
	}

	void Sync_Change_Weapon5()
	{
		if (isServer)
			Rpc_Change_Weapon5();
		else
			Cmd_Change_Weapon5();
	}
	[ClientRpc]
	void Rpc_Change_Weapon5()
	{
		Change_Weapon5();
	}
	[Command]
	void Cmd_Change_Weapon5()
	{
		Change_Weapon5();
	}
	void Change_Weapon5()
	{
		weapon = 5;
		Ani.SetFloat("Weapon", weapon);
		Ani.SetBool("HaveWeapon", true);
		Staff.SetActive(false);
		Sword.SetActive(false);
		Gun.SetActive(false);
		Drink.SetActive(false);
	}
	#endregion


	void Sync_StartMove()
	{
		if (isServer)
			Rpc_StartMove();
		else
			Cmd_StartMove();
	}
	[ClientRpc]
	void Rpc_StartMove()
	{
		StartMove();
	}
	[Command]
	void Cmd_StartMove()
	{
		StartMove();
	}
	void StartMove()
	{
		Ani.SetBool("Run", true);
	}


	void Sync_StopMove()
	{
		if (isServer)
			Rpc_StopMove();
		else
			Cmd_StopMove();
	}
	[ClientRpc]
	void Rpc_StopMove()
	{
		StopMove();
	}
	[Command]
	void Cmd_StopMove()
	{
		StopMove();
	}
	void StopMove()
	{
		Ani.SetBool("Run", false);
	}

	void Sync_SwordAttack()
	{
		if (isServer)
			Rpc_SwordAttack();
		else
			Cmd_SwordAttack();
	}
	[ClientRpc]
	void Rpc_SwordAttack()
	{
		SwordAttack();
	}
	[Command]
	void Cmd_SwordAttack()
	{
		SwordAttack();
	}
	void SwordAttack()
	{
		Ani.SetTrigger("Attack");
	}

	void Sync_MagicAttack(Vector3 targetPos)
	{
		if (isServer)
			Rpc_MagicAttack(targetPos);
		else
			Cmd_MagicAttack(targetPos);
	}
	[ClientRpc]
	void Rpc_MagicAttack(Vector3 targetPos)
	{
		MagicAttack(targetPos);
	}
	[Command]
	void Cmd_MagicAttack(Vector3 targetPos)
	{
		MagicAttack(targetPos);
	}
	void MagicAttack(Vector3 targetPos)
	{
		Ani.SetTrigger("MagicAttack");
		PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
		data.magicType = PrefabManager.MAGIC_TYPE.Magic01;
		data.targetPos = targetPos;
		GameObject obj = PrefabManager.Instance.SpawnMagic(data);

		// Spawn the bullet on the Clients
		if (isServer)
			NetworkServer.Spawn(obj);
	}

	void Sync_DrinkMedical()
	{
		if (isServer)
			Rpc_DrinkMedical();
		else
			Cmd_DrinkMedical();
	}
	[ClientRpc]
	void Rpc_DrinkMedical()
	{
		DrinkMedical();
	}
	[Command]
	void Cmd_DrinkMedical()
	{
		DrinkMedical();
	}
	void DrinkMedical()
	{
		Debug.LogError("Drink");
		Ani.SetTrigger("Drink");
		m_Health.currentHealth += 50;
		if (m_Health.currentHealth > 100)
			m_Health.currentHealth = 100;
		Debug.LogError("Drink End");
	}

	void Sync_BulletFire()
	{
		if (isServer)
			Rpc_BulletFire();
		else
			Cmd_BulletFire();
	}
	[ClientRpc]
	void Rpc_BulletFire()
	{
		BulletFire();
	}
	[Command]
	void Cmd_BulletFire()
	{
		BulletFire();
	}
	void BulletFire()
	{
		Ani.SetTrigger("GunAttack");
		// Create the Bullet from the Bullet Prefab
		GameObject obj = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * 20.0f;
		obj.GetComponent<bullet>().playerID = (int)netId.Value;
		obj.GetComponent<bullet>().bBoss = bBoss;
		Physics.IgnoreCollision(obj.GetComponent<Collider>(), this.GetComponent<Collider>());

		if (isServer)
			NetworkServer.Spawn(obj);
		// Destroy the bullet after 2 seconds
		Destroy(obj, 2.0f);
	}

	void Sync_HealthFire()
	{
		if (isServer)
			Rpc_HealthFire();
		else
			Cmd_HealthFire();
	}
	[ClientRpc]
	void Rpc_HealthFire()
	{
		HealthFire();
	}
	[Command]
	void Cmd_HealthFire()
	{
		HealthFire();
	}
	void HealthFire()
	{
		PrefabManager.udsPrefabData data = new PrefabManager.udsPrefabData();
		data.magicType = PrefabManager.MAGIC_TYPE.Health01;
		data.targetPos = m_v3CurMouseHitPoint;
		data.forward = transform.forward;
		data.startPos = transform.position;

		GameObject obj = PrefabManager.Instance.SpawnMagic(data);

		if(isServer)
			NetworkServer.Spawn(obj);
		Destroy(obj, 2.0f);
	}
	#endregion

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

	private void OnTriggerEnter(Collider other)
	{
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
