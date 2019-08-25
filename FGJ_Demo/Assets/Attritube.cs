using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attritube : MonoBehaviour
{
	public bool IsAttack;

	public PlayerController.deadFunc m_dead;

	public void AttackStart()
	{
		IsAttack = true;
	}
	public void AttackEnd()
	{

		IsAttack = false;
	}

	public void DeadStart()
	{
		m_dead();
	}
}
