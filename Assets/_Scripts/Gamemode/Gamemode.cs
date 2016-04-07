﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Gamemode : NetworkBehaviour {
    public GameObject startTimerText;
    public GameObject statusText;
    private List<Player> players = new List<Player>();
    public int playerCount = 0;
    int finishedPlayers = 0;
   public static GameObject hud = null;
    public GameObject HUDPrefab = null;
    void Awake()
    {
       hud =  GameObject.Instantiate(HUDPrefab);
       startTimerText = hud.transform.Find("StartTimerText").gameObject;
       statusText = hud.transform.Find("StatusText").gameObject;

    }
    public enum State
    {
        STARTING,
        RACING,
        DONE_RACING
    }
    public struct Player
    {
        public int placement;
        public GameObject gameObject;
        public NetworkConnection conn;
        public Player(int placement, GameObject gameObject, NetworkConnection conn)
        {
            this.placement = placement;
            this.gameObject = gameObject;
            this.conn = conn;
        }
    }
    private struct DebugPlayer
    {
        
        public GameObject gameObject;
        public int currentLap;
        public int currentCheckpointIndex;
        public float distanceToNextCheckpoint;
        public DebugPlayer(GameObject gameObject,int currentLap,int currentCheckpointIndex, float distanceToNextCheckpoint)
        {
            this.gameObject = gameObject;
            this.currentLap = currentLap;
            this.currentCheckpointIndex = currentCheckpointIndex;
            this.distanceToNextCheckpoint = distanceToNextCheckpoint;
        }
    }

    public void AddPlayer(Player p)
    {
        if (!players.Contains(p))
        {
            players.Add(p);
            playerCount++;
        }
    }
    public Player GetPlayerFromGameObject(GameObject g)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject == g)
                return players[i];
        }
        return new Player(-1,null,null);
    }
    public void RemovePlayer(GameObject g)
    {
        Player p = GetPlayerFromGameObject(g);
        if (p.gameObject != null)
            players.Remove(p);
    }
    [SyncVar]
    public State currentState;
    float startTimer = 0.0f;
    float startDelay = 2.0f;
	// Use this for initialization
	void Start () {
        if(isServer)
            setState(State.STARTING);
	}

	// Update is called once per frame


	void Update () {

	    switch(currentState)
        {
            case State.STARTING:
                {
                    startTimer += Time.deltaTime;
                    if (startTimer >= startDelay)
                        setState(State.RACING);
                    startTimerText.GetComponent<Text>().text = ((int)(startDelay - startTimer)).ToString(); 
                    break;
                }
            case State.RACING:
                {
                    List<DebugPlayer> tempPlayerList = new List<DebugPlayer>();
                    for (int i = 0; i < players.Count; i++ )
                    {
                        if(players[i].conn == null)
                        {
                            players.Remove(players[i]);
                            continue;
                        }
                        DebugPlayer dp = new DebugPlayer();
                        Player p = players[i];
                        Placement placement = p.gameObject.GetComponent<Placement>();
                        TrackInformation track = GameObject.FindGameObjectsWithTag("track")[0].transform.root.gameObject.GetComponent<TrackInformation>();

                        dp.gameObject = p.gameObject;
                        dp.currentLap = placement.currentLap;
                        dp.currentCheckpointIndex = placement.currentCheckPointIndex;
                        dp.distanceToNextCheckpoint = Vector3.Distance( track.checkPoints[dp.currentCheckpointIndex + 1].transform.position,dp.gameObject.transform.position);
                        tempPlayerList.Add(dp);
                    }
                    players.Clear();
                   for(int j = 0; j < tempPlayerList.Count; j++)
                   {
                        DebugPlayer currentBest = new DebugPlayer();
                        currentBest.gameObject = null;
                        for (int i = 0; i < tempPlayerList.Count; i++)
                        {
                            DebugPlayer tempPlayer = tempPlayerList[i];
                            if (containsPlayer(tempPlayer.gameObject))
                                continue;
                            if(currentBest.gameObject != null)
                            {
                                if(currentBest.currentLap == tempPlayer.currentLap)
                                {
                                    
                                        
                                    if(currentBest.currentCheckpointIndex == tempPlayer.currentCheckpointIndex)
                                    {
                                         if(currentBest.distanceToNextCheckpoint > tempPlayer.distanceToNextCheckpoint)
                                        {
                                            currentBest = tempPlayer;
                                        }
                                    }
                                    else if (currentBest.currentCheckpointIndex < tempPlayer.currentCheckpointIndex)
                                        currentBest = tempPlayer;
                                }
                                else if (currentBest.currentLap < tempPlayer.currentLap)
                                    currentBest = tempPlayer;
                            }
                            else
                            {
                                currentBest = tempPlayer;
                            }
                        }

                        Player p = new Player(players.Count + 1, currentBest.gameObject, MyNetworkLobbyManager.GetConnectionFromGameObject(currentBest.gameObject));
                       players.Add(p);

                    }
                        break;
                }
            case State.DONE_RACING:
                {
                    break;
                }
        }
	}

    public int getPlacement(GameObject g)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject == g)
                return players[i].placement;
        }
        return -1;
    }
    bool containsPlayer(GameObject g)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject == g)
                return true;
        }
        return false;
    }
    [ClientRpc]
    void RpcenableInput()
    {
        PlayerNetwork.localPlayer.GetComponent<KartInput>().EnableInput();
    }
    public void setState(State state)
    {
        currentState = state;
        switch(state)
        {
            case State.STARTING:
                {
                    statusText.SetActive(false);
                    startTimerText.SetActive(true);
           
                    break;
                }
            case State.RACING:
                {
                    statusText.SetActive(false);
                    if (startTimerText.activeSelf)
                        startTimerText.SetActive(false);
                    if(isServer)
                    RpcenableInput();
                    break;
                }
            case State.DONE_RACING:
                {
                    break;
                }
          
        }
    }

    // Checks if enough players have finished the track to end it
    public void checkGameFinish()
    {
        if (playerCount > 0)
        {
            for (int i = 0; i < playerCount; i++) // Search through all players
            {
                Placement placement = players[i].gameObject.GetComponent<Placement>();
                if (placement.gameFinished == true) // If the player has finished the game, add it to the index
                {
                    finishedPlayers++;
                }
            }

            //If enough players have finished the track to end it, do the following
            if (playerCount == finishedPlayers)
            {
                Debug.Log("All players have finished the track!");
                // Do something
            }

            finishedPlayers = 0;
        }
    }
}
