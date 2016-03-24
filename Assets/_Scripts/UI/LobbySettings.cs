using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LobbySettings : MonoBehaviour {

    GameObject canvas = null;
    MyNetworkLobbyManager lobbyManager = null;
    public static int currentMapIndex = 0;
    public static int lapCount = 2;
    public string[] scenes;
	void Start () {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();
        canvas = GameObject.Find("Lobby Settings");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
       // if (lobbyManager.showLobbyGUI)
        if (lobbyManager.showEpicUI)
            canvas.SetActive(true);
        else
            canvas.SetActive(false);
    }
    public void ChangePlayerCount(int number)
    {
        lobbyManager.maxPlayers = number;
    }
    public void ChangeCurrentMap(int number)
    {
        currentMapIndex = number;
        Debug.Log("Map changed: " + number);
        //lobbyManager.onlineScene = scenes[number];
        lobbyManager.playScene = scenes[number];
        Debug.Log("Map changedstring: " + scenes[number]);

    }
    public void ChangeLapCount(int number)
    {
        lapCount = number+1;
    }

    public void ChangeCurrentCharacter(int number)
    {
         StoredKartInfo.characterID = number;
    }

}
