using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour {
    public GameObject uiPrefab = null;
	// Use this for initialization
	void Start () {
	    if(!isLocalPlayer)
        {
            GetComponent<KartInput>().enabled = false;
            GetComponent<KartBehaviour>().enabled = false;
            transform.FindChild("Main Camera").gameObject.active = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
