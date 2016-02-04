using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Harpoon : NetworkBehaviour {
    public GameObject harpoonHeadPrefab;
	// Use this for initialization
	void Start () {
        if (isServer)
        {
            GameObject harpoonHead = (GameObject)GameObject.Instantiate(harpoonHeadPrefab, transform.position, transform.rotation);
            harpoonHead.GetComponent<HarpoonHead>().harpoon = gameObject;
            NetworkServer.Spawn(harpoonHead);
           // harpoonHead
        }
      }
	
	// Update is called once per frame
	void Update () {

	}
}
