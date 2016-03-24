using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// Script used when a player hits the weapon box
public class WeaponBoxScript : NetworkBehaviour {
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // When players collider hits weapon box collider...
        {
            Debug.Log("Collided with Player!");
            other.transform.root.GetComponent<InventoryScript>().pickWeapon(gameObject);    // Call pickWeapon script from InventoryScript which picks up a random weapon.

        }
    }
}
