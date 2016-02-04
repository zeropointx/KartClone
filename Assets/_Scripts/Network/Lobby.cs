using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

	// Use this for initialization
    GameObject lobby;
    Text inputField;
	void Start () {
        lobby = GameObject.Find("Lobby");
        inputField = GameObject.Find("InputField").transform.FindChild("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartHost()
    {
        lobby.GetComponent<NetworkLobbyManager>().StartHost();
    }
    public void Connect()
    {
                if(inputField == null)
        {
            Debug.Log("Add inputfield to UILogic script!");
            return;
        }
        string ip = inputField.text;
        if(ip == null || ip == "" || ip.Length > 20)
        {
            Debug.Log("Invalid ip!");
            return;
        }
        lobby.GetComponent<NetworkLobbyManager>().networkAddress = ip;
        lobby.GetComponent<NetworkLobbyManager>().StartClient();
        
    }
}
