using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

	// Use this for initialization
    MyNetworkLobbyManager lobbyManager;
    InputField inputField;
	void Start () {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();
        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        inputField.text = "127.0.0.1";
        lobbyManager.showLobbyGUI = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartHost()
    {
        lobbyManager.GetComponent<NetworkLobbyManager>().StartHost();
        //lobbyManager.showLobbyGUI = true;
        lobbyManager.showEpicUI = true;
    }
    public void Connect()
    {
                if(inputField == null)
        {
            Debug.Log("Add inputfield to UILogic script!");
            return;
        }
        string ip = inputField.text;
        if(ip == null || ip == "" || ip.Length > 20)
        {
            Debug.Log("Invalid ip!");
            return;
        }
        lobbyManager.networkAddress = ip;
        lobbyManager.showEpicUI = true;
        lobbyManager.StartClient();
        
    }
}
