using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkBehaviour
{
    public static LobbyPlayerScript playerScript = null;
    MyNetworkLobbyPlayer lobbyPlayer = null;

    // Use this for initialization
    void Start()
    {
        if (hasAuthority)
        {
            playerScript = this;
            CmdTestCommand();
        }
    }

    [Command]
    void CmdTestCommand()
    {
        Debug.Log("HEHHEH");
    }

    // Update is called once per frame
    void Update()
    {
        if (lobbyPlayer == null)
        {
            lobbyPlayer = transform.GetComponent<MyNetworkLobbyPlayer>();
        }
    }

    public void ToggleReady()
    {
        if (hasAuthority)
        {
            if (lobbyPlayer != null)
                CmdToggleReady();
            else
                Debug.Log("lobbyPlayer is NULL");
        }
        else
            Debug.Log("No authority send network CMD!");

        /*
        if (lobbyPlayer == null)
        {
            foreach (var obj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (obj.name == "LobbyPlayer(Clone)")
                {
                    var temp = obj.GetComponent<MyNetworkLobbyPlayer>();
                    if (temp != null)
                        CmdToggleReady(obj);
                }
            }
        }
         */
    }

    [Command]
    void CmdToggleReady()
    {
        Debug.Log("toggle ready lobby player script");
        lobbyPlayer.readyInLobby = !lobbyPlayer.readyInLobby;
    }
}
