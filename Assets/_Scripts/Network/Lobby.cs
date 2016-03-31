using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour {

	// Use this for initialization
    MyNetworkLobbyManager lobbyManager;
    
    void Start () 
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();
        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        lobbyManager.showLobbyGUI = true;

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
        //SceneManager.LoadScene("RealLobby");
        lobbyManager.GetComponent<NetworkLobbyManager>().StartHost();
        //lobbyManager.showLobbyGUI = true;
        lobbyManager.showEpicUI = true;
    }

    public void Connect()
    {
        lobbyManager.networkAddress = ServerInfo.ip;
        lobbyManager.showEpicUI = true;
        lobbyManager.StartClient();
        //SceneManager.LoadScene("RealLobby");   
    }

    public void StartGame()
    {
        SceneManager.LoadScene("RealLobby");
    }
}
