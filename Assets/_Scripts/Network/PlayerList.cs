﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerList : NetworkBehaviour {
    public List<uint> players = new List<uint>();
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SendPlayerInfo(uint[] players)
    {
       // if(isServer)
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

            GameObject gamemode = GameObject.Find("Gamemode");
            gamemode.GetComponent<Gamemode>().AddPlayer(p);

        }
    }




    public void SendPlayerInfo()
    {
        SendPlayerInfo(GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>().GetPlayers());
    }
}
