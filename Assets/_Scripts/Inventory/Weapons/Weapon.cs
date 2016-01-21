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
        if (Input.GetButtonDown("Shoot"))
        {
            shootServer();
        }
    }
    void shootServer()
    {
        switch (inventory.currentWeapon)
        {
            case InventoryScript.WEAPON.noWeapon:
                {
                    break;
                }
            case InventoryScript.WEAPON.BowlingBall:
                {
                    // Position fixing...
                    Instantiate(bowlingBall, transform.position + (transform.forward * 7), transform.rotation);
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;

                    break;
                }
        }
    }
}
