using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Harpoon : NetworkBehaviour {
    public GameObject harpoonHeadPrefab;
    GameObject harpoonHead = null;
	// Use this for initialization
	void Start () {
        if (isServer)
        {
            //Spawn harpoonhead and tell it that this is harpoon
            harpoonHead = (GameObject)GameObject.Instantiate(harpoonHeadPrefab, transform.position + transform.forward*5, transform.rotation);
            harpoonHead.GetComponent<HarpoonHead>().harpoon = gameObject;
            NetworkServer.Spawn(harpoonHead);
           // harpoonHead
        }
      }
	
	// Update is called once per frame
	void Update () {
        if (harpoonHead == null)
        {
            transform.root.GetComponent<InventoryScript>().currentWeapon = InventoryScript.WEAPON.noWeapon;
            Destroy(gameObject);
            return;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdHandleQuitHook();
            return;
        }
        if(harpoonHead.GetComponent<HarpoonHead>().hitObject != null)
        {
            Vector3 force = harpoonHead.transform.position - transform.root.position;
            force *= Time.deltaTime * 100f;
            
            transform.root.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
        }
	}
    [Command]
    void CmdHandleQuitHook()
    {
        Destroy(harpoonHead.gameObject);
        Destroy(gameObject);
        transform.root.GetComponent<InventoryScript>().currentWeapon = InventoryScript.WEAPON.noWeapon;
    }
}
