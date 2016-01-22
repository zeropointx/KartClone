using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {
    public bool ready = false;
    public int playerCount = 0;
    int minPlayerCountToStart = 2;
    public override void OnServerConnect(NetworkConnection conn)
    {
        //Debug.Log("trololo");
        base.OnServerConnect(conn);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        playerCount++;
        if(playerCount >=minPlayerCountToStart)
        {
            ready = true;
        }
        Debug.Log("Player count " + playerCount);
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        playerCount--;
        Debug.Log("Player count " + playerCount);
    }
}
