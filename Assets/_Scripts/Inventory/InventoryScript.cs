using UnityEngine;
using System.Collections;
using UnityEngine.Networking;



public class InventoryScript : NetworkBehaviour
{

    //TODO: Better names once we know them
    public enum WEAPON 
    { 
        BowlingBall = 0, 
        SpeedBoost = 1, 
        Harpoon = 2,
        noWeapon  = 8
    };

    [SyncVar]
    public WEAPON currentWeapon;


    void Start()
    {
        currentWeapon = WEAPON.noWeapon;

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
    public void pickUpRandomWeapon()
    {
        currentWeapon = ((WEAPON)Random.Range(0, (int)WEAPON.Harpoon+1));
    }

}
