using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour {

    MyNetworkLobbyManager lobbyManager;
    MyNetworkLobbyPlayer lobbyPlayer;
    private Dropdown PlayersInLobby = null;

	// Use this for initialization
    void Start () 
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();

        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        lobbyManager.showLobbyGUI = true;
        PlayersInLobby = GameObject.Find("PlayersInLobby").GetComponent<Dropdown>();

        if (ServerInfo.ip == "127.0.0.1")
            StartHost();
        else
            Connect();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void StartHost()
    {
        lobbyManager.GetComponent<NetworkLobbyManager>().StartHost();
        lobbyManager.showLobbyUI = true;
    }

    public void Connect()
    {
        lobbyManager.networkAddress = ServerInfo.ip;
        lobbyManager.showLobbyUI = true;
        lobbyManager.StartClient();
    }

    public void KickPlayer()
    {
        Debug.Log("kicked " + PlayersInLobby.options[PlayersInLobby.value].text);
    }

    public void StartGame()
    {
        Debug.Log("start");
    }
}
