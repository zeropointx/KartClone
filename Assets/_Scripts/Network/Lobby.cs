using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Lobby : MonoBehaviour {

	// Use this for initialization
    GameObject lobby;
	void Start () {
        lobby = GameObject.Find("Lobby");
        lobby.GetComponent<NetworkLobbyManager>().StartServer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
