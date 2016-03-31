using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MyNetworkLobbyManager : NetworkLobbyManager {
    public static MyNetworkLobbyManager networkLobbyManagerInstance = null;
    public bool showEpicUI = true;
    public bool debugLobbyManager = false;
    public void DebugMessage(string message)
    {
        if(!debugLobbyManager)
        {
            return;
        }
        Debug.Log(message);
    }
    public int playerCount
    {
        get
        {
            return GetPlayerCount();
        }
    }
    public int minPlayerCountToStart = 1;
    public List<NetworkConnection> connections = new List<NetworkConnection>();
    public static NetworkConnection GetConnectionFromGameObject(GameObject g)
    {
        for(int i = 0; i <networkLobbyManagerInstance.connections.Count; i++ )
        {
            if (networkLobbyManagerInstance.connections[i].playerControllers[0].gameObject == g)
                return networkLobbyManagerInstance.connections[i];
        }
        return null;
    }
    public List<NetworkConnection> GetConnections()
    {
        return connections;
    }
    public int GetPlayerCount()
    {
        return connections.Count;
    }
    void AddPlayer(NetworkConnection conn)
    {
        if(!connections.Contains(conn))
        connections.Add(conn);
    }
    void RemovePlayer(NetworkConnection conn)
    {
        connections.Remove(conn);

        GameObject g = GameObject.Find("Gamemode");
        if (g == null)
            return;
        Gamemode gameMode= g.GetComponent<Gamemode>();
        gameMode.RemovePlayer(conn.playerControllers[0].gameObject);
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
         DebugMessage("Player connected! Current players " + playerCount);
         

    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {

        
        DebugMessage("Player disconnected! Current players " + playerCount);
        
        base.OnServerDisconnect(conn);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        DebugMessage("Client connected! Current players " + playerCount);
        AddPlayer(conn);

        DebugMessage("Player added! Current players " + playerCount);
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        RemovePlayer(conn);
        DebugMessage("Client disconnected! Current players " + playerCount);
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
        if (!showEpicUI)
            return;

        if (Application.loadedLevelName != lobbyScene)
            return;

        Rect backgroundRec = new Rect(90, 180, 500, 150);
        GUI.Box(backgroundRec, "Players:");

        if (NetworkClient.active)
        {
            Rect addRec = new Rect(100, 300, 120, 20);
            if (GUI.Button(addRec, "Add Player"))
            {
                AddPlayerUI();
            }
        }
    }

    bool m_ShowServer;
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


    }
}
