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
    float startDelay = 5.0f;
	// Use this for initialization
	void Start () {
        if(isServer)
            setState(State.WAITING_FOR_PLAYERS);
	}
    GameObject hud = null;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if(hud == null)
            hud = GameObject.Find("HUD");
            Debug.Log(hud.active);
            hud.active = !hud.active;
            PlayerNetwork.localPlayer.transform.FindChild("Kart").gameObject.active = hud.active;
        }

        setState(currentState);
	    switch(currentState)
        {
            case State.WAITING_FOR_PLAYERS:
                {
                    if (MyNetworkLobbyManager.networkLobbyManagerInstance.playerCount >= MyNetworkLobbyManager.networkLobbyManagerInstance.minPlayerCountToStart)
                    {

                        setState(Gamemode.State.STARTING);
                    }
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
        PlayerNetwork.localPlayer.GetComponent<KartInput>().enabled = true;
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
