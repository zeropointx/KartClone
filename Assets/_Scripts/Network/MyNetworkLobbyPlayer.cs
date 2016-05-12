using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkLobbyPlayer : NetworkLobbyPlayer
{
    public bool showLobbyGUI = false;

    [SyncVar]
    public int kartId = 0;

    [SyncVar]
    public bool readyInLobby = false;
    [SyncVar]
    public uint kartNetId = 0;

    private MyNetworkLobbyManager lobbyManager;
    private LobbyPlayerScript lobbyplayerScript = null;

    void OnGUI()
    {
        lobbyManager = NetworkManager.singleton as MyNetworkLobbyManager;
        if (lobbyManager)
        {
            if (SceneManager.GetActiveScene().name != lobbyManager.lobbyScene)
            {
                if (showLobbyGUI)
                {
                    if (!lobbyManager.showLobbyGUI)
                    {
                        GameObject.Find("Lobby").GetComponent<Lobby>().DisableUI();
                        showLobbyGUI = false;
                    }
                }
                return;
            }
        }
    }

    void Awake()
    {
        GameObject.Find("Lobby").GetComponent<Lobby>().AddGameObject(gameObject);
        lobbyplayerScript = transform.GetComponent<LobbyPlayerScript>();
    }

    public void StartGame()
    {
        base.SendReadyToBeginMessage();
    }

    public void Update()
    {
        if (lobbyManager != null)
        {
            showLobbyGUI = lobbyManager.showLobbyGUI;
        }
    }
    [Command]
    public void CmdChangeKart(int kartid)
    {
        this.kartId = kartid;
    }

    public static MyNetworkLobbyPlayer GetLocalLobbyPlayer()
    {
        var players = GetLobbyPlayers();
        foreach (MyNetworkLobbyPlayer player in players)
        {
            if (player.isLocalPlayer)
            {
                return player;
            }
        }
        return null;
    }
    public static MyNetworkLobbyPlayer GetLobbyPlayer(NetworkConnection conn)
    {
        var players = GetLobbyPlayers(); ;
        foreach (MyNetworkLobbyPlayer player in players)
        {
            if (player.connectionToClient == conn)
            {
                return player;
            }
        }
        return null;
    }
    public static MyNetworkLobbyPlayer[] GetLobbyPlayers()
    {
        return GameObject.FindObjectsOfType<MyNetworkLobbyPlayer>();

    }

}
