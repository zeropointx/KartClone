using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour {

    MyNetworkLobbyManager lobbyManager = null;
    MyNetworkLobbyPlayer lobbyPlayer = null;
    private List<GameObject> players = new List<GameObject>();
    private Dropdown playersInLobby = null;
    private GameObject buttonText = null;

	// Use this for initialization
    void Start () 
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();

        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        lobbyManager.showLobbyGUI = true;
        playersInLobby = GameObject.Find("PlayersInLobby").GetComponent<Dropdown>();
        playersInLobby.ClearOptions();
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
        UpdateList();
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

    public void UpdateList()
    {
        playersInLobby.ClearOptions();
        foreach (var obj in players)
        {
            MyNetworkLobbyPlayer mnlb = obj.GetComponent<MyNetworkLobbyPlayer>();
            string ready = " | ";
            if (mnlb.isLocalPlayer)
                ready += "local player | ";
            ready += mnlb.readyToBegin ? "Ready!" : "Not ready!";
            playersInLobby.options.Add(new Dropdown.OptionData("id " + mnlb.netId.Value + ready));
        }
        playersInLobby.RefreshShownValue();
    }

    public void AddGameObject(GameObject g)
    {
        var id = g.GetComponent<MyNetworkLobbyPlayer>().netId.Value;
        if (players.Count > 0)
        {
            int i = 0;
            foreach (var obj in players)
            {
                if (obj == g)
                {
                    Debug.Log("lobby: netid " + id + " is already on list!");
                    break;
                }
                i++;
            }
            if (i == players.Count)
            {
                Debug.Log("lobby: added netid " + id);
                players.Add(g);
            }
        }
        else
            players.Add(g);
        UpdateList();
    }
}
