using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncGameSetting : NetworkBehaviour
{
	public bool bBoss = false;

	[SyncVar(hook = "SetBoss")]
	public uint iCurrentBossId = 99;

	private static SyncGameSetting m_Instance = null;

	public static SyncGameSetting GetInstance
	{
		get
		{
			return m_Instance;
		}
	}

	private void Awake()
	{
		if(m_Instance == null)
			m_Instance = this;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.F9))
		{
			//CmdSetBoss(netId.Value);
			//RpcSetBoss(netId.Value);
			if(isServer)
				SetBoss(netId.Value);
			else
				CmdSetBoss(netId.Value);

			//if (isServer)
			//	CmdSetBoss(netId.Value);
			//else
			//	RpcSetBoss(netId.Value);
		}
	}

	public bool BossMode
	{
		get
		{
			return bBoss;
		}
		set
		{
			bBoss = value;
		}
	}

	[Command]
	void CmdSetBoss(uint NetID)
	{
		Debug.Log("CmdSetBoss " + NetID);
		bBoss = true;
		iCurrentBossId = NetID;
	}

	public void SetBoss(uint NetID)
	{
		Debug.Log("SetBoss " + NetID);
		bBoss = true;
		iCurrentBossId = NetID;
	}

	[ClientRpc]
	public void RpcSetBoss(uint NetID)
	{
		bBoss = true;
		iCurrentBossId = NetID;
	}
}
