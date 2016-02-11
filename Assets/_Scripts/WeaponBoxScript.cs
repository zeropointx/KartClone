using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeaponBoxScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collision)
    {
  
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collided with Player!");
            other.transform.root.GetComponent<InventoryScript>().pickWeapon(gameObject);

        }
    }
}
