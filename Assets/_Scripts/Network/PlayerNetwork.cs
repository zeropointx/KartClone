using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
    MyNetworkLobbyManager networkManager = null;

    bool initialized = false;
    public StatusEffectHandler statusEffectHandler = new StatusEffectHandler();
    public static GameObject localPlayer = null;

    //minimap
    void Start()
    {
       
        if (!isLocalPlayer)
        {
            GetComponent<KartBehaviour>().enabled = false;
            transform.FindChild("Main Camera").gameObject.SetActive(false);
            GetComponent<KartInput>().enabled = false;
        }
        else
        {
            localPlayer = gameObject;
         
        }
        networkManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();
      


    }
    // Update is called once per frame
    void Update()
    {
        statusEffectHandler.Update();
        if(!initialized)
        {
            GameObject gamemode = GameObject.Find("Gamemode");
            if (gamemode != null)
            {
                NetworkConnection conn = MyNetworkLobbyManager.GetConnectionFromGameObject(gameObject);
                gamemode.GetComponent<Gamemode>().AddPlayer(new Gamemode.Player(-1, gameObject,conn));
                initialized = true;
            }
        }
    }
    public StatusEffectHandler GetStatusEffectHandler()
    {
        return statusEffectHandler;
    }

}
