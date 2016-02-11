using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour
{
    InventoryScript inventory;

    //Weapon prefabs
    public GameObject bowlingBall;
    public GameObject speedBoost;
    public GameObject homingMissilePrefab;
    public GameObject minePrefab;
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
                    break;
                }
            case InventoryScript.WEAPON.BowlingBall:
                {
                    // TODO: Fix positioning for network
                    GameObject ball = (GameObject)Instantiate(bowlingBall, transform.position + (transform.forward * 7), transform.rotation);
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                    NetworkServer.Spawn(ball);
                    break;
                }
            case InventoryScript.WEAPON.SpeedBoost:
                {
                    GameObject speed = (GameObject)Instantiate(speedBoost, transform.position, Quaternion.LookRotation(-transform.forward));
                    //vähä kovakoodattu shittii
                    speed.transform.parent = gameObject.transform;
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                    NetworkServer.Spawn(speed);
                    break;
                }
            /*case InventoryScript.WEAPON.HomingMissile:
                {
                    GameObject homingMissile = (GameObject)Instantiate(homingMissilePrefab, transform.position - transform.forward * 5, transform.rotation);
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                    NetworkServer.Spawn(homingMissile);
                    break;
                }*/
            case InventoryScript.WEAPON.Mine:
                {
                    GameObject mine = (GameObject)Instantiate(minePrefab, transform.position + transform.forward * -4 , transform.rotation);
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                    NetworkServer.Spawn(mine);
                    break;
                }
        }
    }
}
