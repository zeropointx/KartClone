using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Gamemode : NetworkBehaviour {
    public GameObject startTimerText;
    public GameObject statusText;
    private List<Player> players = new List<Player>();
    public int playerCount = 0;
    public enum State
    {
        WAITING_FOR_PLAYERS,
        STARTING,
        RACING,
        DONE_RACING
    }
    public struct Player
    {
        public int placement;
        public GameObject gameObject;
        public Player(int placement, GameObject gameObject)
        {
            this.placement = placement;
            this.gameObject = gameObject;
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
            players.Add(p);
        playerCount++;
    }
    [SyncVar]
    public State currentState;
    float startTimer = 0.0f;
    float startDelay = 5.0f;
	// Use this for initialization
	void Start () {
        if(isServer)
            setState(State.RACING);
	}
	
	// Update is called once per frame
	void Update () {
        setState(currentState);
	    switch(currentState)
        {
            case State.WAITING_FOR_PLAYERS:
                {
                    break;
                }
            case State.STARTING:
                {
                    startTimer += Time.deltaTime;
                    if (startTimer >= startDelay)
                        currentState = State.RACING;
                    startTimerText.GetComponent<Text>().text = ((int)(startDelay - startTimer)).ToString(); 
                    break;
                }
            case State.RACING:
                {
                    List<DebugPlayer> tempPlayerList = new List<DebugPlayer>();
                    for (int i = 0; i < players.Count; i++ )
                    {
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

                       Player p = new Player(players.Count+1,currentBest.gameObject);
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
        GameObject.Find("HUD").GetComponent<HUD>().localPlayer.GetComponent<KartInput>().enabled = true;
    }
    public void setState(State state)
    {
        currentState = state;
        switch(state)
        {
            case State.WAITING_FOR_PLAYERS:
                {
                    break;
                }
            case State.STARTING:
                {
                    statusText.active = false;
                    startTimerText.active = true;
           
                    break;
                }
            case State.RACING:
                {
                    statusText.active = false;
                    if (startTimerText.active == true)
                        startTimerText.active = false;
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
}
