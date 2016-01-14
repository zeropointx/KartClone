﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using UnityEngine.Networking;

public class NetworkScript : MonoBehaviour {
    private int playerCount = 0;
    string currentLevel = "defaultscene";
    public int port;
	// Use this for initialization
	void Start () {
        Object.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player " + playerCount + " connected from " + player.ipAddress + ":" + player.port);
        playerCount++;
        
    }
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Player " + playerCount + " Disconnected from " + player.ipAddress + ":" + player.port);
    
        playerCount--;
        
    }
    [RPC]
    void NetworkLoadLevel(string level)
    {
        SceneManager.LoadScene(currentLevel);
    }
}
