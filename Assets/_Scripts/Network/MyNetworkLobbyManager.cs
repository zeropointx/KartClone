using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MyNetworkLobbyManager : NetworkLobbyManager {
    public static MyNetworkLobbyManager networkLobbyManagerInstance = null;
    public int playerCount
    {
        get
        {
            return GetPlayerCount();
        }
    }
    public int minPlayerCountToStart = 1;
    List<NetworkConnection> connections = new List<NetworkConnection>();
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
         Debug.Log("Player connected! Current players " + playerCount);
         AddPlayer(conn);
         Debug.Log("Player added! Current players " + playerCount);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {

        RemovePlayer(conn);
        Debug.Log("Player disconnected! Current players " + playerCount);
        
        base.OnServerDisconnect(conn);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("Client connected! Current players " + playerCount);
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Debug.Log("Client disconnected! Current players " + playerCount);
    }
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        Debug.Log("OnServerReady");
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        Debug.Log("SceneChanged: " + sceneName);
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
    }
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        Debug.Log("Player removed! Current players " + playerCount);
    }


}
