using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.NetworkSystem;

public class Lobby : MonoBehaviour {
    NetworkClient client = null;
    public GameObject listEntryObject = null;

    public MyNetworkLobbyManager lobbyManager = null;
    public MyNetworkLobbyPlayer lobbyPlayer = null;
    public List<GameObject> players = new List<GameObject>();
    public GameObject buttonText = null;
    public GameObject lobbyPlayerList = null;
    public const int listEntrySpacing = 48;

    private LobbyMenu menu;
   // const short messageId = 1337;
	// Use this for initialization
    void Start () 
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();

        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        lobbyManager.showLobbyGUI = true;
        buttonText = GameObject.Find("ReadyText");
        lobbyPlayerList = GameObject.Find("LobbyPlayerList");
        menu = transform.FindChild("LobbyUI").GetComponent<LobbyMenu>();

        lobbyManager.showLobbyUI = true;
        if (ServerInfo.hosting)
        {
            //toggleReadyText.GetComponent<Button>().interactable = false;
            //toggleReadyText.SetActive(false);
            lobbyManager.GetComponent<NetworkLobbyManager>().StartHost();
        }
        else
        {
            //GameObject.Find("StartGameButton").GetComponent<Button>().interactable = false;
            //GameObject.Find("StartGameButton").SetActive(false);
            lobbyManager.networkAddress = ServerInfo.ip;
            client = lobbyManager.StartClient();
 //           client.RegisterHandler(messageId, Test);
        }
	}
    /*
    void Test(NetworkMessage netMsg)
    {
        var beginMessage = netMsg.ReadMessage<StringMessage>();
        Debug.Log("RECEIVED:: OnServerReadyToBeginMessageClient " + beginMessage.value);
        uint[] players = new uint[beginMessage.value.Length];
        for(int i = 0; i < beginMessage.value.Length; i++)
        {
            players[i] = (uint)beginMessage.value[i];
        }
        GameObject.Find("PlayerList").GetComponent<PlayerList>().SendPlayerInfoHärpäke(players);
    }*/

	// Update is called once per frame
	void Update () 
    {
        
	}

    public void OnGUI()
    {
        if (menu != null)
            menu.UpdateList();
        /*
        if (lobbyManager != null)
        {
            if (!lobbyManager.showLobbyGUI)
                return;
        }
        else
            return;

        if (lobbyPlayer == null)
        {
            var temp = GameObject.Find("LobbyPlayer(Clone)");
            if (temp != null)
            {
                lobbyPlayer = temp.GetComponent<MyNetworkLobbyPlayer>();
            }
        }
        else
        {
            if (!lobbyPlayer.showLobbyGUI)
                return;
            if (!lobbyPlayer.isLocalPlayer)
            {
                if (lobbyPlayer.readyToBegin)
                    buttonText.GetComponent<Text>().text = "Ready!";
                else
                    buttonText.GetComponent<Text>().text = "Not ready yet!";
            }
            else
            {
                buttonText.GetComponent<Text>().text = "Start game";
            }
        }
        */
    }

    public void DisableUI()
    {
        Destroy(transform.FindChild("LobbyUI").gameObject);
    }

    public void KickPlayer()
    {
        //Debug.Log("kicked " + playersInLobby.options[playersInLobby.value].text);
    }

    public void ToggleReady()
    {
        /*
        lobbyPlayer.ToggleReady();
        if (lobbyPlayer.readyToBegin)
            lobbyManager.showLobbyGUI = false;
        */
        if (lobbyPlayer.isLocalPlayer)
        {
            Debug.Log("start");
            lobbyPlayer.StartGame();
        }
        else
        {
            Debug.Log("toggle ready");
            lobbyPlayer.CmdToggleReady(!lobbyPlayer.readyInLobby);
        }
    }

    public void UpdateList()
    {
        /*
        for(int i = 0; i < lobbyPlayerList.transform.childCount; i++)
        {
            Destroy(lobbyPlayerList.transform.GetChild(i).gameObject);
        }

        int j = -1;
        foreach (var obj in players)
        {
            MyNetworkLobbyPlayer mnlb = obj.GetComponent<MyNetworkLobbyPlayer>();
            string label = "id " + mnlb.netId.Value + " | ";
            if (mnlb.isLocalPlayer)
                label += "local player | ";
            label += mnlb.readyInLobby ? "Ready!" : "Not ready!";
            
            GameObject listEntry = (GameObject)Instantiate(listEntryObject, listEntryObject.transform.position, listEntryObject.transform.rotation);
            listEntry.transform.SetParent(lobbyPlayerList.transform, false);
            listEntry.GetComponent<Text>().text = label;
            listEntry.transform.localPosition += new Vector3(0, j * listEntrySpacing, 0);
            j--;
        }
         */
    }

    public void AddGameObject(GameObject g)
    {
        var id = g.GetComponent<MyNetworkLobbyPlayer>().netId.Value;
        if (players.Count > 0)
        {
            int i = 0;
            foreach (var obj in players)
            {
                if (obj == g)
                {
                    Debug.Log("lobby: netid " + id + " is already on list!");
                    break;
                }
                i++;
            }
            if (i == players.Count)
            {
                Debug.Log("lobby: added netid " + id);
                players.Add(g);
            }
        }
        else
            players.Add(g);
        UpdateList();
    }
}
