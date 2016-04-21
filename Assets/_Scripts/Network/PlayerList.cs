using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
        GameObject[] playerObj = new GameObject[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            NetworkInstanceId id = new NetworkInstanceId(players[i]);
            GameObject player = ClientScene.FindLocalObject(id);
            Gamemode.Player p = new Gamemode.Player(-1, player);

            GameObject gamemode = GameObject.Find("Gamemode");
            gamemode.GetComponent<Gamemode>().AddPlayer(p);

        }
    }


    public void RequestPlayerList()
    {
        CmdRequestPlayerList();
    }
    [Command]
    public void CmdRequestPlayerList()
    {
        
        GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>().SendPlayerInfo();
    }
}
