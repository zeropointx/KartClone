using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkLobbyPlayer : NetworkLobbyPlayer {
    [SerializeField]
    public bool ShowMahGUI = true;

    void OnGUI()
    {
        if (!ShowMahGUI)
            return;

        var lobby = NetworkManager.singleton as MyNetworkLobbyManager;
        if (lobby)
        {
            if (Application.loadedLevelName != lobby.lobbyScene)
                return;
        }

        Rect rec = new Rect(100 + slot * 100, 200, 90, 20);

        if (isLocalPlayer)
        {
            GUI.Label(rec, " [ You ]");

            if (readyToBegin)
            {
                rec.y += 25;
                if (GUI.Button(rec, "Ready"))
                {
                    SendNotReadyToBeginMessage();
                }
            }
            else
            {
                rec.y += 25;
                if (GUI.Button(rec, "Not Ready"))
                {
                    SendReadyToBeginMessage();
                }

                rec.y += 25;
                if (GUI.Button(rec, "Remove"))
                {
                    ClientScene.RemovePlayer(GetComponent<NetworkIdentity>().playerControllerId);
                }
            }
        }
        else
        {
            GUI.Label(rec, "Player [" + netId + "]");
            rec.y += 25;
            GUI.Label(rec, "Ready [" + readyToBegin + "]");
        }
    }
}
