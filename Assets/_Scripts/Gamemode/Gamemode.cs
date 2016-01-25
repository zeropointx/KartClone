using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Gamemode : NetworkBehaviour {
    public GameObject startTimerText;
    public GameObject statusText;
    public enum State
    {
        WAITING_FOR_PLAYERS,
        STARTING,
        RACING,
        DONE_RACING
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
                  
                    break;
                }
            case State.DONE_RACING:
                {
                    break;
                }
        }
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
