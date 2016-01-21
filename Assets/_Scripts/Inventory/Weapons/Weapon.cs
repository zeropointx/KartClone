using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour
{
    InventoryScript inventory;

    //Weapon prefabs
    public GameObject bowlingBall;


    void Start()
    {
        inventory = gameObject.GetComponent<InventoryScript>();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
            if (Input.GetButtonDown("Shoot"))
            {
                CmdshootServer();
            }
        
    }
    [Command]
    void CmdshootServer()
    {
        switch (inventory.currentWeapon)
        {
            case InventoryScript.WEAPON.noWeapon:
                {
                       // Position fixing...
                    GameObject ball = (GameObject)Instantiate(bowlingBall, transform.position + (transform.forward * 7), transform.rotation);
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                    NetworkServer.Spawn(ball);
                    break;
                }
            case InventoryScript.WEAPON.BowlingBall:
                {
                    // Position fixing...
                    GameObject ball = (GameObject)Instantiate(bowlingBall, transform.position + (transform.forward * 7), transform.rotation);
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                    NetworkServer.Spawn(ball);
                    break;
                }
        }
    }
}
