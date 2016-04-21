using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class SettingsInterface : MonoBehaviour {
    public static GameObject settingsMenu = null;
    public Dropdown qualitySettings = null;
    public Dropdown resolutionSettings = null;
    bool fullscreen = false;
    Text debugText = null;
    Dropdown windowSettings = null;
    /*
     * BORDERLESS
     */
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;


    /*
     * /BORDERLESS
     * 
     */


	// Use this for initialization
	void Start () {
        qualitySettings = transform.Find("QualitySettings").GetComponent<Dropdown>();
        resolutionSettings = transform.Find("Resolutions").GetComponent<Dropdown>();
        debugText = transform.Find("DEBUG").GetComponent<Text>();
        windowSettings = transform.Find("WindowSettings").GetComponent<Dropdown>();
        SetupInterface();
	}
	
	// Update is called once per frame
	void Update () {

	}
    public static void OpenSettings()
    {
        if(settingsMenu != null)
        {
            settingsMenu.GetComponent<SettingsInterface>().Back();
            return;
        }
        GameObject settings = (GameObject)Resources.Load("_Prefabs/SettingsCanvas");
        if (settings == null)
        {
            Debug.Log("failed to load settings");
            return;
        }
        settingsMenu = GameObject.Instantiate(settings);
    }
    public void SetupInterface()
    {

        //Set quality settings
        string[] names = QualitySettings.names;
        List<string> listNames = new List<string>(names);
        qualitySettings.AddOptions(listNames);
        qualitySettings.value = QualitySettings.GetQualityLevel();

        //set resolution settings
        Resolution[] resolutions = Screen.resolutions;

        List<string> resolutionStrings = new List<string>();
        int resolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++ )
        {
            resolutionStrings.Add(resolutions[i].width + " x " + resolutions[i].height);
            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
                resolutionIndex = i;
        }
        resolutionSettings.AddOptions(resolutionStrings);
        resolutionSettings.value = resolutionIndex;
        debugText.text = Screen.width + ":" + Screen.height; 
    }
    public void ApplySettings()
    {
        QualitySettings.SetQualityLevel(qualitySettings.value, false);

        Resolution selectedResolution = Screen.resolutions[resolutionSettings.value];
   

        switch(windowSettings.options[windowSettings.value].text)
        {
            case "Windowed":
                {
                    fullscreen = false;
                    break;
                }
            case "Borderless":
                {
                    fullscreen = false;
                    //No boderless for unity editor
                    if (Application.isEditor)
                        return;
                    SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);
                    bool result = SetWindowPos(
                        GetForegroundWindow(), 
                        0,
                        0,//X
                        0,//Y
                        selectedResolution.width,
                        selectedResolution.height, 
                        SWP_SHOWWINDOW);
                   
                    break;
                }
            case "Fullscreen":
                {
                    fullscreen = true;


                    break;
                }
            default:
                break;
        }
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreen);

        Back();
    }
    public void Back()
    {
        Destroy(settingsMenu);
        settingsMenu = null;
        MenuSettings.OpenMenu();
    }
}
