using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour {

    public GameObject listEntryObject = null;

    MyNetworkLobbyManager lobbyManager = null;
    MyNetworkLobbyPlayer lobbyPlayer = null;
    private List<GameObject> players = new List<GameObject>();
    private GameObject buttonText = null;
    private GameObject lobbyPlayerList = null;
    private const int listEntrySpacing = 48;

	// Use this for initialization
    void Start () 
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();

        MyNetworkLobbyManager.networkLobbyManagerInstance = lobbyManager;
        lobbyManager.showLobbyGUI = true;
        buttonText = GameObject.Find("ReadyText");
        lobbyPlayerList = GameObject.Find("LobbyPlayerList");

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
            lobbyManager.StartClient();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        //UpdateList();
	}

    public void OnGUI()
    {
        if (!lobbyManager.showLobbyGUI)
            return;

        if (lobbyPlayer == null)
        {
            var temp = GameObject.Find("LobbyPlayer(Clone)");
            if (temp != null)
                lobbyPlayer = temp.GetComponent<MyNetworkLobbyPlayer>();
        }
        else
        {
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
    }

    public void KickPlayer()
    {
        //Debug.Log("kicked " + playersInLobby.options[playersInLobby.value].text);
    }

    public void ToggleReady()
    {
        if (lobbyPlayer.isLocalPlayer)
        {
            Debug.Log("start");
            lobbyPlayer.StartGame();
        }
        else
        {
            Debug.Log("toggle ready");
            lobbyPlayer.ToggleReady();
        }
    }

    public void UpdateList()
    {
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
            label += mnlb.readyToBegin ? "Ready!" : "Not ready!";
            
            GameObject listEntry = (GameObject)Instantiate(listEntryObject, listEntryObject.transform.position, listEntryObject.transform.rotation);
            listEntry.transform.SetParent(lobbyPlayerList.transform, false);
            listEntry.GetComponent<Text>().text = label;
            listEntry.transform.localPosition += new Vector3(0, j * listEntrySpacing, 0);
            j--;
        }
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
