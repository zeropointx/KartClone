using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkLobbyManager : NetworkLobbyManager {

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }
}
