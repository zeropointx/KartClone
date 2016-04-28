using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour 
{

    public GameObject listEntryObject = null;

    public MyNetworkLobbyManager lobbyManager = null;
    public MyNetworkLobbyPlayer lobbyPlayer = null;
    public List<GameObject> players = new List<GameObject>();
    public GameObject buttonText = null;
    public GameObject lobbyPlayerList = null;

    private LobbyMenu menu;

	// Use this for initialization
    void Start () 
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();

        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        lobbyManager.showLobbyGUI = true;
        buttonText = GameObject.Find("ReadyText");
        lobbyPlayerList = GameObject.Find("LobbyPlayerList");
        menu = transform.FindChild("LobbyUI").GetComponent<LobbyMenu>();

        lobbyManager.showLobbyUI = true;
        if (ServerInfo.hosting)
            lobbyManager.GetComponent<NetworkLobbyManager>().StartHost();
        else
        {
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
        if (menu != null)
            menu.UpdateList();
    }

    public void DisableUI()
    {
        if (menu != null)
            Destroy(transform.FindChild("LobbyUI").gameObject);
        else
            Debug.Log("Lobby UI has already been disabled!");
    }

    public void KickPlayer()
    {
        //Debug.Log("kicked " + playersInLobby.options[playersInLobby.value].text);
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
            lobbyPlayer.CmdToggleReady(!lobbyPlayer.readyInLobby);
        }
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
        menu.UpdateList();
    }
}
