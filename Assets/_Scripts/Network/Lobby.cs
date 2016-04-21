using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour {

    MyNetworkLobbyManager lobbyManager = null;
    MyNetworkLobbyPlayer lobbyPlayer = null;
    private Dropdown playersInLobby = null;
    private GameObject buttonText = null;

	// Use this for initialization
    void Start () 
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();

        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        lobbyManager.showLobbyGUI = true;
        playersInLobby = GameObject.Find("PlayersInLobby").GetComponent<Dropdown>();
        buttonText = GameObject.Find("ReadyText");

        lobbyManager.showLobbyUI = true;
        if (ServerInfo.hosting)
        {
            //toggleReadyText.GetComponent<Button>().interactable = false;
            //toggleReadyText.SetActive(false);
            lobbyManager.GetComponent<NetworkLobbyManager>().StartHost();
        }
        else
        {
            //GameObject.Find("StartGameButton").GetComponent<Button>().interactable = false;
            //GameObject.Find("StartGameButton").SetActive(false);
            lobbyManager.networkAddress = ServerInfo.ip;
            lobbyManager.StartClient();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void OnGUI()
    {
        if (!lobbyManager.showLobbyGUI)
            return;

        if (lobbyPlayer == null)
        {
            var temp = GameObject.Find("LobbyPlayer(Clone)");
            if (temp != null)
                lobbyPlayer = temp.GetComponent<MyNetworkLobbyPlayer>();
        }
        else
        {
            if (lobbyManager.playerListUpdated)
            {
                var connections = lobbyManager.connections;
                playersInLobby.ClearOptions();
                foreach (NetworkConnection client in connections)
                {
                    string ready = client.isReady ? " | Ready!" : " | Not ready!";
                    playersInLobby.options.Add(new Dropdown.OptionData(client.address + ready));
                    playersInLobby.RefreshShownValue();
                }
                lobbyManager.playerListUpdated = false;
            }
            if (!lobbyPlayer.isLocalPlayer)
            {
                if (lobbyPlayer.readyToBegin)
                    buttonText.GetComponent<Text>().text = "Ready!";
                else
                    buttonText.GetComponent<Text>().text = "Not ready yet!";
            }
            else
            {
                buttonText.GetComponent<Text>().text = "Start game";
            }
        }
    }

    public void KickPlayer()
    {
        Debug.Log("kicked " + playersInLobby.options[playersInLobby.value].text);
    }

    public void ToggleReady()
    {
        if (lobbyPlayer.isLocalPlayer)
        {
            Debug.Log("start");
            lobbyPlayer.StartGame();
        }
        else
        {
            Debug.Log("toggle ready");
            lobbyPlayer.ToggleReady();
        }
    }

    public void UpdatePlayerList(int selection)
    {
        //playersInLobby.
    }
    public void AddGameObject(GameObject g)
    {
        Debug.Log("HEHHEH");
    }
}
