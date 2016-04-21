using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
    MyNetworkLobbyManager networkManager = null;

    bool initialized = false;
    public StatusEffectHandler statusEffectHandler;
    public static GameObject localPlayer = null;

    //minimap
    void Start()
    {
        statusEffectHandler= new StatusEffectHandler(gameObject);
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
    public void RequestPlayerList()
    {
        CmdRequestPlayerList();
    }
    [Command]
    public void CmdRequestPlayerList()
    {

        GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>().SendPlayerInfo();
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
                 RequestPlayerList();
              //  NetworkConnection conn = MyNetworkLobbyManager.GetConnectionFromGameObject(gameObject);
              //  gamemode.GetComponent<Gamemode>().AddPlayer(new Gamemode.Player(-1, gameObject));
                initialized = true;
            }
        }
    }
    public StatusEffectHandler GetStatusEffectHandler()
    {
        return statusEffectHandler;
    }
    [ClientRpc]
    public void RpcApplyStatusEffectClient(StatusEffectHandler.EffectType type)
    {
        if (!isServer)
            transform.GetComponent<PlayerNetwork>().GetStatusEffectHandler().AddStatusEffect(type);

    }
}
