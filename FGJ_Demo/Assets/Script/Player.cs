using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator Ani;
    public float weapon;
    public bool running;
    public GameObject Sword;
    public GameObject Staff;
    public GameObject Gun;
    public GameObject Drink;
    public float hp =10;
    public bool IsAttack;


    void Awake()
    {
        Sword.SetActive(false);
        Staff.SetActive(false);
        Gun.SetActive(false);
        Drink.SetActive(false);
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1) && IsAttack==false)
        {
            weapon = 1;
            Ani.SetBool("HaveWeapon", true);
            Ani.SetBool("HaveGun", false);
            Sword.SetActive(true);
            Staff.SetActive(false);
            Gun.SetActive(false);
            Drink.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && IsAttack ==false)
        {
            weapon = 2;
            Ani.SetBool("HaveWeapon", true);
            Ani.SetBool("HaveGun", false);
            Staff.SetActive(true);
            Sword.SetActive(false);
            Gun.SetActive(false);
            Drink.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && IsAttack == false)
        {
            weapon = 3;
            Ani.SetBool("HaveWeapon", false);
            Ani.SetBool("HaveGun", true);
            Staff.SetActive(false);
            Sword.SetActive(false);
            Gun.SetActive(true);
            Drink.SetActive(false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha4) && IsAttack == false)
		{
			weapon = 4;
			Ani.SetBool("HaveWeapon", false);
			Ani.SetBool("HaveGun", true);
			Staff.SetActive(false);
			Sword.SetActive(false);
			Gun.SetActive(false);
			Drink.SetActive(true);
		}

		if (Input.GetKeyDown(KeyCode.Alpha5) && IsAttack == false)
		{
			weapon = 5;
			Ani.SetBool("HaveWeapon", false);
			Ani.SetBool("HaveGun", true);
			Staff.SetActive(false);
			Sword.SetActive(false);
			Gun.SetActive(false);
			Drink.SetActive(false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha0) && IsAttack == false)
        {
            weapon = 0;
            Ani.SetBool("HaveWeapon", false);
            Ani.SetBool("HaveGun", false);
            Staff.SetActive(false);
            Sword.SetActive(false);
            Gun.SetActive(false);
            Drink.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            running = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            running = false;
        }
        if (Input.GetMouseButtonDown(0) && weapon == 1 && IsAttack == false)
        {
            Ani.SetTrigger("Attack");
        }
        if (Input.GetMouseButtonDown(0) && weapon == 2 && IsAttack == false)
        {
            Ani.SetTrigger("MagicAttack");
        }
        if (Input.GetMouseButtonDown(0) && weapon == 3 && IsAttack == false)
        {
            Ani.SetTrigger("GunAttack");
        }
        if (Input.GetMouseButtonDown(0) && weapon == 4   && IsAttack == false)
        {
            Ani.SetTrigger("Drink");
        }

        if (running)
        {
            Ani.SetBool("Run", true);
        }
        else
            Ani.SetBool("Run", false);
        if (hp <= 0)
        {
            Ani.SetBool("Dead", true);
        }


    }
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

        Ani.SetBool("Dead", false); 
    }
}



