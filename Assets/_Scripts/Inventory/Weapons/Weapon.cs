using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
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
        if (inventory.currentWeapon != InventoryScript.WEAPON.noWeapon)
        {
            if (inventory.currentWeapon == InventoryScript.WEAPON.BowlingBall)
            {
                // Update
                if (Input.GetButtonDown("Shoot"))
                {
                    // Position fixing...
                    Instantiate(bowlingBall, transform.position+(transform.forward*4), transform.rotation);
                    inventory.currentWeapon = InventoryScript.WEAPON.noWeapon;
                }

            }

        }
    }
}
