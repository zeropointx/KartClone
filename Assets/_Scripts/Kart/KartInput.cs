using UnityEngine;
using System.Collections;

public class KartInput : MonoBehaviour
{
    private KartBehaviour kartScript;
    public bool isInputEnabled = false;
    public bool debugMode;

    // Use this for initialization
    void Start()
    {
        debugMode = true;
        kartScript = transform.GetComponent<KartBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInputEnabled)
        {
            kartScript.SetPedal(0);
            kartScript.SetSteer(0);
            return;
        }

        //controls
        float gas = Input.GetAxis("Vertical");
        gas = Mathf.Clamp(gas, -1, 1);
        kartScript.SetPedal(gas);

        float steer = Input.GetAxis("Horizontal");
        steer = Mathf.Clamp(steer, -1, 1);
        kartScript.SetSteer(steer);

        //weapons


        //debug
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.R))
                kartScript.Reset(0, Input.GetKey(KeyCode.LeftAlt));

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

            if (Input.GetButtonDown("Exit"))
            {
                    MenuSettings.OpenMenu();
            }
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
