﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuSettings : MonoBehaviour {

    public static GameObject Menu = null;
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public static void OpenMenu()
    {
        if (Menu != null)
        {
            Menu.GetComponent<MenuSettings>().Exit();
            return;
        }
        if (SettingsInterface.settingsMenu != null)
        {
            SettingsInterface.OpenSettings();
            return;
        }

        GameObject menu = (GameObject)Resources.Load("_Prefabs/MenuCanvas");
        if (menu == null)
        {
            Debug.Log("failed to load menu");
            return;
        }

            Menu = GameObject.Instantiate(menu);
    }

    public void OpenSettings()
    {
        Destroy(Menu);
        SettingsInterface.OpenSettings();
    }

    public void Continue()
    {
        Destroy(Menu);
    }

    public void QuitGame()
    {
        GameObject.Find("Lobby").GetComponent<MyNetworkLobbyManager>().StopHost();
        Destroy(GameObject.Find("Lobby"));
        Destroy(GameObject.Find("PlayerList"));
        SceneManager.LoadScene("mainMenu");
    }

    public void Back()
    {
        Destroy(Menu);
        Menu = null;
        MenuSettings.OpenMenu();
    }

    public void Exit()
    {
        Destroy(Menu);
        Menu = null;
    }


}
