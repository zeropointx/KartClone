using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkLobbyManager : NetworkLobbyManager {
    public static MyNetworkLobbyManager networkLobbyManagerInstance = null;
    public int playerCount = 0;
    public int minPlayerCountToStart = 1;
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
         Debug.Log("Player connected! Current players " + playerCount);

         Debug.Log("Player added! Current players " + ++playerCount);

    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Player disconnected! Current players " + --playerCount);

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
