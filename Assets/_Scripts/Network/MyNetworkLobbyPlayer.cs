using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkLobbyPlayer : NetworkLobbyPlayer
{
    public bool showLobbyGUI = false;

    [SyncVar]
    public bool readyInLobby = false;

    private MyNetworkLobbyManager lobbyManager;

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
        readyInLobby = false;
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
    public void CmdToggleReady(bool value)
    {
        SetReadyInLobby(value);
    }

    private void SetReadyInLobby(bool value)
    {
        readyInLobby = value;
    }

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
