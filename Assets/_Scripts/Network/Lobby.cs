using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.NetworkSystem;

public class Lobby : MonoBehaviour
{
    NetworkClient client = null;
    public GameObject listEntryObject = null;

    public MyNetworkLobbyManager lobbyManager = null;
    public MyNetworkLobbyPlayer lobbyPlayer = null;
    public List<GameObject> players = new List<GameObject>();
    public GameObject lobbyPlayerList = null;
    public GameObject startButton = null;

    private LobbyMenu menu;

	// Use this for initialization
    void Start () 
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();

        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        lobbyManager.showLobbyGUI = true;
        lobbyPlayerList = GameObject.Find("LobbyPlayerList");
        menu = transform.FindChild("LobbyUI").GetComponent<LobbyMenu>();
        startButton = GameObject.Find("StartButton");

        lobbyManager.showLobbyUI = true;
        if (ServerInfo.hosting)
        {
            lobbyManager.StartHost();
        }
        else
        {
            lobbyManager.networkAddress = ServerInfo.ip;
            client = lobbyManager.StartClient();
        }
	}
    /*
    void Test(NetworkMessage netMsg)
    {
        var beginMessage = netMsg.ReadMessage<StringMessage>();
        Debug.Log("RECEIVED:: OnServerReadyToBeginMessageClient " + beginMessage.value);
        uint[] players = new uint[beginMessage.value.Length];
        for(int i = 0; i < beginMessage.value.Length; i++)
        {
            players[i] = (uint)beginMessage.value[i];
        }
        GameObject.Find("PlayerList").GetComponent<PlayerList>().SendPlayerInfoHärpäke(players);
    }*/

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

    public void StartGame()
    {
        if (lobbyPlayer.isLocalPlayer)
        {
            Debug.Log("start button pressed");
            if (menu.everyoneReady)
                lobbyPlayer.StartGame();
            else
                Debug.Log("Wait for clients to get ready!");
        }
        else
            Debug.Log("Only host can start the game!");
    }

    public void ToggleReady()
    {
        Debug.Log("toggle ready lobby pressed");
        LobbyPlayerScript.playerScript.ToggleReady();
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
