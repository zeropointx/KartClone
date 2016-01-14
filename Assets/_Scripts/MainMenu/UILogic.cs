using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UILogic : MonoBehaviour {

    bool hosting = false;
    public InputField inputField = null;
    public NetworkScript networkScript;
    public NetworkManager networkManager;
    public ClientManager clientManager;
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
    
      //  NetworkServer.Listen(networkScript.port);
        hosting = true;
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
        clientManager.gameObject.active = true;
        clientManager.Connect(ip, networkScript.port);
    }
}
