﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;


// Handles player inventory
public class InventoryScript : NetworkBehaviour
{

    // List of possible weapons you can aquire from weaponBoxes
    public enum WEAPON 
    { 
        BowlingBall = 0, 
        SpeedBoost = 1, 
        Harpoon = 2,
        noWeapon  = 8
    };

    int weaponAmount;               // Amount of different weapons
    [SyncVar]
    public WEAPON currentWeapon;    // Players current weapon
    

    void Start()
    {
        currentWeapon = WEAPON.noWeapon; // Set to not have weapon at the beginning
        weaponAmount = Enum.GetNames(typeof(WEAPON)).Length - 2;

    }

    void Update()
    {
        bool input = false;
        int index;
        for (index = (int)KeyCode.Keypad0; index < (int)KeyCode.Keypad9+1; index++ )
        {
            if (Input.GetKeyDown((KeyCode)index))
            {
                input = true;
                break;
            }
        }
        if(input)
        {
            CmdDebugChangeWeapon(index - (int)KeyCode.Keypad0);
        }

           
    }
    [Command]
   void CmdDebugChangeWeapon(int index)
    {
        currentWeapon = (WEAPON)index;
    }

    public void pickWeapon(GameObject weaponBox)
    {
        if (currentWeapon == WEAPON.noWeapon)
        {
            pickUpRandomWeapon();
            Destroy(weaponBox);
        }
    }

    // Pick up a random weapon for player
    public void pickUpRandomWeapon()
    {
        currentWeapon = ((WEAPON)UnityEngine.Random.Range(0, weaponAmount));
    }

}
