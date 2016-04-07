using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkLobbyPlayer : NetworkLobbyPlayer 
{
    [SerializeField]
    public bool showPlayerUI = true;

    void OnGUI()
    {
        if (!showPlayerUI)
            return;

        var lobbyManager = NetworkManager.singleton as MyNetworkLobbyManager;
        if (lobbyManager)
        {
            if (SceneManager.GetActiveScene().name != lobbyManager.lobbyScene)
                return;
        }
        Rect rec = new Rect(100 + slot * 100, 200, 90, 20);

        if (isLocalPlayer)
        {
            GUI.Label(rec, " [ You ]");

            if (base.readyToBegin)
            {
                rec.y += 25;
                if (GUI.Button(rec, "Ready"))
                {
                    base.SendNotReadyToBeginMessage();
                }
            }
            else
            {
                rec.y += 25;
                if (GUI.Button(rec, "Not Ready"))
                {
                    base.SendReadyToBeginMessage();
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
            GUI.Label(rec, "Ready [" + base.readyToBegin + "]");
        }
    }
}
