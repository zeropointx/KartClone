using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public GameObject player;
    InventoryScript inventory;
	// Use this for initialization
	void Start () 
    {
        inventory = player.GetComponent<InventoryScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (inventory.currentWeapon != InventoryScript.WEAPON.noWeapon)
        {
            // Update
            if (Input.GetButtonDown("Shoot"))
            {


            }

        }
	}
}
