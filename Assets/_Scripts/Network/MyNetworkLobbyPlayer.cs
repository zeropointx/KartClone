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
        /*
        if (!showPlayerUI)
            return;
        */
        lobbyManager = NetworkManager.singleton as MyNetworkLobbyManager;
        if (lobbyManager)
        {
            if (SceneManager.GetActiveScene().name != lobbyManager.lobbyScene)
                return;
        }
        Rect rec = new Rect(100 + slot * 100, 200, 90, 20);

        if (base.isLocalPlayer)
        {
            //GUI.Label(rec, " [ You ]");
            /*
            if (base.readyToBegin)
            {
                rec.y += 25;
                if (GUI.Button(rec, "Ready"))
                {
                    base.SendNotReadyToBeginMessage();
                }
            }
            else
            {
                rec.y += 25;
                if (GUI.Button(rec, "Not Ready"))
                {
                    base.SendReadyToBeginMessage();
                }

                rec.y += 25;
                if (GUI.Button(rec, "Remove"))
                {
                    ClientScene.RemovePlayer(GetComponent<NetworkIdentity>().playerControllerId);
                }
            }
             */
            /*
            if (lobbyManager.readyTostart)
            {
                base.SendReadyToBeginMessage();
                lobbyManager.readyTostart = false;
            }
             */
        }
        else
        {
            /*
            GUI.Label(rec, "Player [" + netId + "]");
            rec.y += 25;
            GUI.Label(rec, "Ready [" + base.readyToBegin + "]");
            */
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
        /*
        var lobbyManager = NetworkManager.singleton as MyNetworkLobbyManager;
        if (base.isLocalPlayer)
        {
            base.SendReadyToBeginMessage();
            lobbyManager.showLobbyGUI = false;
        }
        else
            Debug.Log("Only host can start the game!");
        */
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
        /*
        if (base.isLocalPlayer)
        {
            if (base.readyToBegin)
                base.SendNotReadyToBeginMessage();
            else
                base.SendReadyToBeginMessage();
            string temp = base.readyToBegin ? "true" : "false";
            Debug.Log("Toggle ready, current value: " + temp);
        }
        Debug.Log("Only local player can send readytobeginmessage!");
         */
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
