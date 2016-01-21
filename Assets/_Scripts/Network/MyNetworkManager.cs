using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {
    public override void OnServerConnect(NetworkConnection conn)
    {
        //Debug.Log("trololo");
        base.OnServerConnect(conn);
    }
}
