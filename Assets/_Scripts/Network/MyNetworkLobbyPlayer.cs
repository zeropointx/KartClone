using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkLobbyPlayer : NetworkLobbyPlayer
{
    public bool showLobbyGUI = false;

    [SyncVar]
    public int kartId = -1;

    [SyncVar]
    public bool readyInLobby = false;

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

    /*
    public void ToggleReady()
    {
        Debug.Log("toggle ready network lobby player");
        lobbyplayerScript.ToggleReady();
    }
     * */

    public void KickPlayer()
    {
        if (base.isLocalPlayer)
        {
            ClientScene.RemovePlayer(GetComponent<NetworkIdentity>().playerControllerId);
        }
        else
            Debug.Log("Only host can kick players!");
    }

}
