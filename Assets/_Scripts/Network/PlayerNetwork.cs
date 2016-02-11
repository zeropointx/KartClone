using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
    MyNetworkLobbyManager networkManager = null;
    public GameObject uiPrefab = null;
    bool initialized = false;
    
    public static GameObject localPlayer = null;
    void Start()
    {
        GetComponent<KartInput>().enabled = false;
        if (!isLocalPlayer)
        {
            GetComponent<KartBehaviour>().enabled = false;
            transform.FindChild("Main Camera").gameObject.active = false;
        }
        else
           localPlayer = gameObject;
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

    public void Spin()
    {
        transform.GetComponent<KartBehaviour>().Spin();
    }

}
