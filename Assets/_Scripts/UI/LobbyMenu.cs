using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{

    public bool enabled = true;

    private Lobby lobbyScript = null;
    private const int listEntrySpacing = 48;
    public bool everyoneReady = false;

    // Use this for initialization
    void Start()
    {
        lobbyScript = transform.parent.GetComponent<Lobby>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGUI()
    {
        if (lobbyScript.lobbyManager != null)
        {
            if (!lobbyScript.lobbyManager.showLobbyGUI)
                return;
        }
        else
            return;

        if (lobbyScript.lobbyPlayer == null)
        {
            var temp = GameObject.Find("LobbyPlayer(Clone)");
            if (temp != null)
            {
                lobbyScript.lobbyPlayer = temp.GetComponent<MyNetworkLobbyPlayer>();
            }
        }
    }

    public void UpdateList()
    {
        for (int i = 0; i < lobbyScript.lobbyPlayerList.transform.childCount; i++)
        {
            Destroy(lobbyScript.lobbyPlayerList.transform.GetChild(i).gameObject);
        }

        int j = -1;
        int readyCount = 0;
        foreach (var obj in lobbyScript.players)
        {
            MyNetworkLobbyPlayer mnlb = obj.GetComponent<MyNetworkLobbyPlayer>();
            string label = "id " + mnlb.netId.Value + " | ";
            if (mnlb.isLocalPlayer)
                label += "local player | ";
            label += mnlb.readyInLobby ? "Ready!" : "Not ready!";
            readyCount += mnlb.readyInLobby ? 1 : 0;

            GameObject listEntry = (GameObject)Instantiate(lobbyScript.listEntryObject, lobbyScript.listEntryObject.transform.position, lobbyScript.listEntryObject.transform.rotation);
            listEntry.transform.SetParent(lobbyScript.lobbyPlayerList.transform, false);
            listEntry.GetComponent<Text>().text = label;
            listEntry.transform.localPosition += new Vector3(0, j * listEntrySpacing, 0);
            j--;
        }
        everyoneReady = (readyCount == lobbyScript.players.Count);
    }
}