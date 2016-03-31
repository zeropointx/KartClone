using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    InputField inputField;
    private bool pinging = false;
    private float timer = 0;
    private Ping ping = null;

	// Use this for initialization
	void Start () 
    {
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        inputField.text = "127.0.0.1";
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (pinging)
        {
            timer += Time.deltaTime;
            if(ping.isDone)
            {
                Debug.Log("ping: " + ping.time);
                SceneManager.LoadScene("Lobby");
            }
            if (timer > 3)
            {
                pinging = false;
                timer = 0;
                ping.DestroyPing();
                Debug.Log("ping timeout");
            }
        }
	}

    public void StartHost()
    {
        ServerInfo.ip = "127.0.0.1";
        SceneManager.LoadScene("Lobby");
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
        ServerInfo.ip = ip;
        ping = new Ping(ServerInfo.ip);
        pinging = true;
        Debug.Log("started pinging " + ServerInfo.ip);
    }
}
