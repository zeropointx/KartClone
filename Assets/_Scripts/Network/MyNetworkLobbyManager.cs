using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkLobbyManager : NetworkLobbyManager
{
    public static MyNetworkLobbyManager networkLobbyManagerInstance = null;
    public bool showLobbyUI = true;
    public bool debugMessages;
    private bool playerUIAdded = false;
    //private bool m_ShowServer;
    public int minPlayerCountToStart = 1;
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
    /*
    public static NetworkConnection GetConnectionFromGameObject(GameObject g)
    {
        for (int i = 0; i < networkLobbyManagerInstance.connections.Count; i++)
        {
            if (networkLobbyManagerInstance.connections[i].playerControllers[0].gameObject == g)
                return networkLobbyManagerInstance.connections[i];
        }
        return null;
    }*/

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
        base.OnClientConnect(conn);
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
        base.OnServerSceneChanged(sceneName);
        DebugMessage("SceneChanged: " + sceneName);

        uint[] players = new uint[connections.Count];
        for (int i = 0; i < connections.Count; i++)
        {
            players[i] = connections[i].playerControllers[0].gameObject.GetComponent<NetworkIdentity>().netId.Value;
        }
        Lobby lobby = transform.GetComponent<Lobby>();
        string playerString = "";

        for (int i = 0; i < players.Length; i++)
        {
            string crd = "" + players[i];
            if (players[i] < 10) 
                crd = "0" + crd; // leading 0 so each uses exactly 2 chars
            playerString += crd;
        }

        //lobby.SendPlayerInfo(playerString);
        GameObject gg = GameObject.Find("Gamemode");
        gg.GetComponent<Gamemode>().RpcSendPlayerInfo(playerString);
        DebugMessage("Players updated for clients");
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

        //Rect backgroundRec = new Rect(90, 180, 500, 150);
        //GUI.Box(backgroundRec, "Players:");

        if (!playerUIAdded)
        {
            AddPlayerUI();
            playerUIAdded = true;
            /*
            Rect addRec = new Rect(100, 300, 120, 20);
            if (GUI.Button(addRec, "Add Player"))
            {
                AddPlayerUI();
            }
             * */
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


        /*
        int xpos = 10 + 0;
        int ypos = 40 + 0;
        const int spacing = 24;

        
        if (!IsClientConnected() && !NetworkServer.active && matchMaker == null)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
            {
                StartHost();
            }
            ypos += spacing;

            if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
            {
                StartClient();
            }
            networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), networkAddress);
            ypos += spacing;

            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
            {
                StartServer();
            }
            ypos += spacing;
        }
        else
        {
            if (NetworkServer.active)
            {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + networkPort);
                ypos += spacing;
            }
            if (IsClientConnected())
            {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + networkAddress + " port=" + networkPort);
                ypos += spacing;
            }
        }

        if (IsClientConnected() && !ClientScene.ready)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
            {
                ClientScene.Ready(client.connection);

                if (ClientScene.localPlayers.Count == 0)
                {
                    ClientScene.AddPlayer(0);
                }
            }
            ypos += spacing;
        }

        if (NetworkServer.active || IsClientConnected())
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
            {
                StopHost();
            }
            ypos += spacing;
        }

        if (!NetworkServer.active && !IsClientConnected())
        {
            ypos += 10;

            if (matchMaker == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
                {
                    StartMatchMaker();
                }
                ypos += spacing;
            }
            else
            {
                if (matchInfo == null)
                {
                    if (matches == null)
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
                        {
                            matchMaker.CreateMatch(matchName, matchSize, true, "", OnMatchCreate);
                        }
                        ypos += spacing;

                        GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
                        matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), matchName);
                        ypos += spacing;

                        ypos += 10;

                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                        {
                            matchMaker.ListMatches(0, 20, "", OnMatchList);
                        }
                        ypos += spacing;
                    }
                    else
                    {
                        foreach (var match in matches)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
                            {
                                matchName = match.name;
                                matchSize = (uint)match.currentSize;
                                matchMaker.JoinMatch(match.networkId, "", OnMatchJoined);
                            }
                            ypos += spacing;
                        }
                    }
                }

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
                {
                    m_ShowServer = !m_ShowServer;
                }
                if (m_ShowServer)
                {
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
                    {
                        SetMatchHost("localhost", 1337, false);
                        m_ShowServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
                    {
                        SetMatchHost("mm.unet.unity3d.com", 443, true);
                        m_ShowServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                    {
                        SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                        m_ShowServer = false;
                    }
                }

                ypos += spacing;

                GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + matchMaker.baseUri);
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
                {
                    StopMatchMaker();
                }
                ypos += spacing;
            }
        }
        */
    }
}
