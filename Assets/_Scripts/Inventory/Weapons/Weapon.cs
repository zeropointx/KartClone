using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour
{
    InventoryScript inventory;

    //Weapon prefabs
    public GameObject bowlingBall;
    public GameObject speedBoost;
    public GameObject harpoonPrefab;

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
                    GameObject speed = (GameObject)Instantiate(speedBoost, transform.position - new Vector3(0,0,1), Quaternion.LookRotation(-transform.forward));
                    //vähä kovakoodattu shittii
                    speed.transform.position = transform.position;
                    speed.transform.parent = gameObject.transform;
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                    NetworkServer.Spawn(speed);
                    break;
                }
            case InventoryScript.WEAPON.Harpoon:
                {
                    

                    GameObject hook = (GameObject)Instantiate(harpoonPrefab, transform.position+transform.forward,transform.rotation);
                    hook.transform.parent = transform;
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                    //vähä kovakoodattu shittii

                    NetworkServer.Spawn(hook);
                    break;
                }
        }
    }
}
