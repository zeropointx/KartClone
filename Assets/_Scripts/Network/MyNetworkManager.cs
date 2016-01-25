using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {
    public bool ready = false;
    public int playerCount = 0;
    int minPlayerCountToStart = 2;
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
       // Debug.Log("Player connected! Current players " + playerCount);
    
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        playerCount--;
        Debug.Log("Player disconnected! Current players " + playerCount);

    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        playerCount++;
        if (playerCount >= minPlayerCountToStart)
        {
            ready = true;
            GameObject gamemode = GameObject.Find("Gamemode");
            if(gamemode == null)
            {
                Debug.Log("Gamemode object doesn´t exist, ERROR!!!");
                return;
            }
            gamemode.GetComponent<Gamemode>().setState(Gamemode.State.STARTING);
        }
        Debug.Log("Player added! Current players " + playerCount);
        base.OnServerAddPlayer(conn, playerControllerId);
    }
}
