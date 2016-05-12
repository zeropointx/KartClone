using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LobbySettings : MonoBehaviour
{

    GameObject canvas = null;
    MyNetworkLobbyManager lobbyManager = null;
    public static int currentMapIndex = 0;
    public string[] scenes;
    public static int lapCount = 2;

    void Start()
    {
        lobbyManager = GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>();
        canvas = GameObject.Find("Lobby Settings");
        ChangePlayerCount(8);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (lobbyManager.showLobbyUI)
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
        lobbyManager.playScene = scenes[number];
        Debug.Log("Map changedstring: " + scenes[number]);

    }
    public void ChangeLapCount(int number)
    {
        lapCount = number + 1;
    }

    public void ChangeCurrentCharacter(int number)
    {
       var localPlayer = MyNetworkLobbyPlayer.GetLocalLobbyPlayer();
       localPlayer.CmdChangeKart(number);
    }

}
