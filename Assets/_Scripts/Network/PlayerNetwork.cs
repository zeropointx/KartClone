using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour {
    MyNetworkManager networkManager = null;
    public GameObject uiPrefab = null;
	// Use this for initialization
	void Start () {
        GetComponent<KartInput>().enabled = false;
	    if(!isLocalPlayer)
        {
            GetComponent<KartBehaviour>().enabled = false;
            transform.FindChild("Main Camera").gameObject.active = false;
        }
        else
            GameObject.Find("HUD").GetComponent<HUD>().localPlayer = gameObject;
        networkManager = GameObject.Find("NetworkManager").GetComponent<MyNetworkManager>();
	}
    void Awake()
    {

      
    }
	// Update is called once per frame
	void Update () {

	}

}
