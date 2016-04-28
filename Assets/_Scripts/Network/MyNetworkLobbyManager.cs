using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.NetworkSystem;

public class MyNetworkLobbyManager : NetworkLobbyManager
{
    public static MyNetworkLobbyManager networkLobbyManagerInstance = null;
    public bool showLobbyUI = true;
    public bool debugMessages;
    private bool playerUIAdded = false;
    public List<NetworkConnection> connections = new List<NetworkConnection>();
    public bool playerListUpdated = false;
    public void Start()
    {
        debugMessages = true;
        

    }

    void DebugMessage(string message)
    {
        if (debugMessages)
            Debug.Log(message);
    }

    public int playerCount
    {
        get
        {
            return GetPlayerCount();
        }
    }

    public List<NetworkConnection> GetConnections()
    {
        return connections;
    }

    public int GetPlayerCount()
    {
        return connections.Count;
    }

    public void AddPlayer(NetworkConnection conn)
    {
        if (!connections.Contains(conn))
        {
            playerListUpdated = true;
            connections.Add(conn);
        }
    }

    void RemovePlayer(NetworkConnection conn)
    {
        connections.Remove(conn);
        GameObject g = GameObject.Find("Gamemode");
        if (g == null)
            return;
        Gamemode gameMode = g.GetComponent<Gamemode>();
        playerListUpdated = true;
        gameMode.RemovePlayer(conn.playerControllers[0].gameObject);
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);



        DebugMessage("Player connected! Current players " + playerCount);
        if (conn.address != "localServer")
        {
            AddPlayer(conn);
        }
        DebugMessage("Player added! Current players " + playerCount);
 
     //   SendPlayerInfo(conn);
    }


    public override void OnServerDisconnect(NetworkConnection conn)
    {
       
        base.OnServerDisconnect(conn);
        DebugMessage("Player disconnected! Current players " + playerCount);
        RemovePlayer(conn);
        DebugMessage("Client disconnected! Current players " + playerCount);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        DebugMessage("Client connected! Current players " + playerCount);

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        DebugMessage("OnServerReady");

    }

    public override void OnServerSceneChanged(string sceneName)
    {
        GameObject.Find("Lobby").GetComponent<Lobby>().DisableUI();
        showLobbyGUI = false;
        base.OnServerSceneChanged(sceneName);
        DebugMessage("SceneChanged: " + sceneName);
    }
   /* public void SendPlayerInfo(NetworkConnection conn)
    {
        string playerString = "";
        for (int i = 0; i < connections.Count; i++)
        {
            playerString += (char)connections[i].playerControllers[0].gameObject.GetComponent<NetworkIdentity>().netId.Value;
        }

        var msg = new StringMessage();
       // conn.Send(messageId, msg);
        DebugMessage("Players updated for clients");
    }*/
    public uint[] GetPlayers()
    {
        uint[] players = new uint[connections.Count];
        for (int i = 0; i < connections.Count; i++)
        {
            players[i] += (char)connections[i].playerControllers[0].gameObject.GetComponent<NetworkIdentity>().netId.Value;
        }
        return players;
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        DebugMessage("Player removed! Current players " + playerCount);
    }

    public void OnGUI()
    {
        if (!showLobbyUI)
            return;

        if (SceneManager.GetActiveScene().name != lobbyScene)
            return;

        if (!playerUIAdded)
        {
            AddPlayerUI();
            playerUIAdded = true;
        }
    }

    public void AddPlayerUI()
    {
        if (NetworkClient.active)
        {
            short controllerId = -1;
            var controllers = NetworkClient.allClients[0].connection.playerControllers;

            if (controllers.Count < maxPlayers)
            {
                controllerId = (short)controllers.Count;
            }
            else
            {
                for (short i = 0; i < maxPlayers; i++)
                {
                    if (!controllers[i].IsValid)
                    {
                        controllerId = i;
                        break;
                    }
                }
            }
            if (LogFilter.logDebug) { DebugMessage("NetworkLobbyManager TryToAddPlayer controllerId " + controllerId + " ready:" + ClientScene.ready); }

            if (controllerId == -1)
            {
                if (LogFilter.logDebug) { DebugMessage("NetworkLobbyManager No Space!"); }
                return;
            }

            if (ClientScene.ready)
            {
                ClientScene.AddPlayer(controllerId);
            }
            else
            {
                ClientScene.AddPlayer(NetworkClient.allClients[0].connection, controllerId);
            }
        }
        else
        {
            if (LogFilter.logDebug) { DebugMessage("NetworkLobbyManager NetworkClient not active!"); }
        }
    }
}
