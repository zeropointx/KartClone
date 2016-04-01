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
                    NetworkServer.Spawn(ball);
                    break;
                }
            case InventoryScript.WEAPON.SpeedBoost:
                {
                    if (!transform.GetComponent<PlayerNetwork>().GetStatusEffectHandler().HasEffect(StatusEffectHandler.EffectType.HIT))
                    {
                        transform.GetComponent<PlayerNetwork>().GetStatusEffectHandler().AddStatusEffect(StatusEffectHandler.EffectType.BOOST);
                    }
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
                    NetworkServer.Spawn(mine);
                    break;
                }
        }
        inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
    }
}
