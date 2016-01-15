using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UILogic : MonoBehaviour {

    public InputField inputField = null;
    public NetworkScript networkScript;
    public NetworkManager networkManager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnApplicationQuit()
    {
        networkManager.StopHost();
    }
    public void Host()
    {
        NetworkClient client = networkManager.StartHost();


        //ClientScene.Ready(networkManager.client.connection);

        //if (ClientScene.localPlayers.Count == 0)
        //{
        //    ClientScene.AddPlayer(0);
        //}

       // SceneManager.LoadScene(networkScript.currentLevel);


      //  NetworkServer.Listen(networkScript.port);
        //SceneManager.LoadScene("defaultscene");
     //   clientManager.ConnectLocal();
 
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
        networkManager.networkAddress = ip;
        networkManager.StartClient();
    }
}
