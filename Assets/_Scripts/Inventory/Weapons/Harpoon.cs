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
        }
      }
	
	void Update () {
        if (harpoonHead == null)
        {
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
            force.Normalize();
            force *= Time.deltaTime * 10000f;
            
            transform.root.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
        }
	}
    //Player presses mouse again and sends to server that it has to remove hook
    [Command]
    void CmdHandleQuitHook()
    {
        Destroy(harpoonHead.gameObject);
        Destroy(gameObject);
        transform.root.GetComponent<InventoryScript>().currentWeapon = InventoryScript.WEAPON.noWeapon;
    }
}
