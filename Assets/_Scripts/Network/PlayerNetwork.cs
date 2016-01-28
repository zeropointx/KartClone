using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
    MyNetworkManager networkManager = null;
    public GameObject uiPrefab = null;

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
        networkManager = GameObject.Find("NetworkManager").GetComponent<MyNetworkManager>();
        GameObject.Find("Gamemode").GetComponent<Gamemode>().AddPlayer(new Gamemode.Player(-1, gameObject));


    }
    void Awake()
    {


    }
    // Update is called once per frame
    void Update()
    {

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
