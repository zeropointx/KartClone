using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientManager : MonoBehaviour {
    NetworkClient client = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Player " + 0);
    }
    public void Connect(string ip, int port)
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.Connect(ip, port);
    }
    public void ConnectLocal()
    {
        client = ClientScene.ConnectLocalServer();
        client.RegisterHandler(MsgType.Connect, OnConnected);
    }
}
