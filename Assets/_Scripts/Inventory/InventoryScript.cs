using UnityEngine;
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
       // HomingMissile = 2,
        Mine = 3,
        noWeapon  = 8
    };

    int weaponAmount;               // Amount of different weapons
    [SyncVar]
    public WEAPON currentWeapon;    // Players current weapon
    

    void Start()
    {
        currentWeapon = WEAPON.noWeapon; // Set to not have weapon at the beginning
        weaponAmount = Enum.GetNames(typeof(WEAPON)).Length; // Initialize the amount of weapons

    }

    void Update()
    {
        int index;
        for (index = (int)KeyCode.Keypad0; index < (int)KeyCode.Keypad9+1; index++ )
        {
            if (Input.GetKeyDown((KeyCode)index))
            {
                CmdDebugChangeWeapon(index - (int)KeyCode.Keypad0);
                break;
            }
        }


           
    }
    [Command]
   void CmdDebugChangeWeapon(int index)
    {
        currentWeapon = (WEAPON)index;
    }

    // Picks up a weapon. This is called when player hits a weaponBox
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
        currentWeapon = ((WEAPON)UnityEngine.Random.Range(0, weaponAmount-1));
    }

}
