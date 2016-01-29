using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
    MyNetworkLobbyManager networkManager = null;
    public GameObject uiPrefab = null;
    bool initialized = false;
    public enum KartHitState
    {
        SPINNING, NORMAL
    };
    [SyncVar]
    public KartHitState hitState = KartHitState.NORMAL;
    // Use this for initialization
    void Start()
    {
        GetComponent<KartInput>().enabled = false;
        if (!isLocalPlayer)
        {
            GetComponent<KartBehaviour>().enabled = false;
            transform.FindChild("Main Camera").gameObject.active = false;
        }
        else
            GameObject.Find("HUD").GetComponent<HUD>().localPlayer = gameObject;
        networkManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();
      


    }
    void Awake()
    {


    }
    // Update is called once per frame
    void Update()
    {
        if(!initialized)
        {
            GameObject gamemode = GameObject.Find("Gamemode");
            if (gamemode != null)
            {
                gamemode.GetComponent<Gamemode>().AddPlayer(new Gamemode.Player(-1, gameObject));
                initialized = true;
            }
        }
    }

    public KartHitState hitUpdate()
    {
        switch (hitState)
        {
            case KartHitState.SPINNING:
                return KartHitState.SPINNING;

        }
        return KartHitState.NORMAL;
        
    }



}
