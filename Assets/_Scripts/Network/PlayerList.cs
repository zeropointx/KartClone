using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class PlayerList : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SendPlayerInfo(uint[] players)
    {
        if(isServer)
        RpcSendPlayerInfo(players);
        SendPlayerInfoHärpäke(players);

    }
    [ClientRpc]
    public void RpcSendPlayerInfo(uint[] players)
    {
        SendPlayerInfoHärpäke(players);
    }
    public void SendPlayerInfoHärpäke(uint[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            NetworkInstanceId id = new NetworkInstanceId(players[i]);
            GameObject player = ClientScene.FindLocalObject(id);
            Gamemode.Player p = new Gamemode.Player(-1, player);
            GameObject.Find("Gamemode").GetComponent<Gamemode>().AddPlayer(p);
        }
    }


    public void RequestPlayerList()
    {
        GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>().SendPlayerInfo();
    }
}
