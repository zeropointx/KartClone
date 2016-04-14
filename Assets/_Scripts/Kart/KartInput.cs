using UnityEngine;
using System.Collections;

public class KartInput : MonoBehaviour {

    private KartBehaviour kartScript;
    public bool isInputEnabled = false;
	// Use this for initialization
	void Start () {
        kartScript = transform.GetComponent<KartBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isInputEnabled)
        {
            kartScript.SetPedal(0);
          kartScript.SetSteer(0);
            return;
        }
            
        float gas = Input.GetAxis("Vertical");
        gas = Mathf.Clamp(gas, -1, 1);
        kartScript.SetPedal(gas);

        float steer = Input.GetAxis("Horizontal");
        steer = Mathf.Clamp(steer, -1, 1);
        kartScript.SetSteer(steer);
        
        if (Input.GetKeyDown(KeyCode.R))
            kartScript.Reset(0, Input.GetKey(KeyCode.LeftShift));

        if (Input.GetKeyDown(KeyCode.T))
            kartScript.BackToTrack();

        if (Input.GetKeyDown(KeyCode.K))
            kartScript.pw.GetStatusEffectHandler().AddStatusEffect(StatusEffectHandler.EffectType.HIT);

        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject hud = Gamemode.hud;
            Debug.Log(hud.activeSelf);
            hud.SetActive(!hud.activeSelf);
            PlayerNetwork.localPlayer.transform.FindChild("Kart").gameObject.SetActive(hud.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            SettingsInterface.OpenSettings();
        }
	}

    public void DisableInput()
    {
        isInputEnabled = false;
    }
    public void EnableInput()
    {
        isInputEnabled = true;
    }
}
