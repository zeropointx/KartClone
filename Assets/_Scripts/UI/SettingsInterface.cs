using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SettingsInterface : MonoBehaviour {
    public static GameObject settingsMenu = null;
    public Dropdown qualitySettings = null;
    public Dropdown resolutionSettings = null;
	// Use this for initialization
	void Start () {
        qualitySettings = transform.Find("QualitySettings").GetComponent<Dropdown>();
        resolutionSettings = transform.Find("Resolutions").GetComponent<Dropdown>();
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
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                resolutionIndex = i;
        }
        resolutionSettings.AddOptions(resolutionStrings);
        resolutionSettings.value = resolutionIndex;
    }
    public void ApplySettings()
    {
        QualitySettings.SetQualityLevel(qualitySettings.value);
        Back();
    }
    public void Back()
    {
        Destroy(settingsMenu);
        settingsMenu = null;
    }
}
