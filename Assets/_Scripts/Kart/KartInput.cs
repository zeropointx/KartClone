using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class KartInput : NetworkBehaviour
{
    private KartBehaviour kartScript;
    public bool isInputEnabled = false;
    public bool debugMode;
    public bool noclip = false;
    // Use this for initialization
    void Start()
    {
        debugMode = true;
        kartScript = transform.GetComponent<KartBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isInputEnabled && !noclip)
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

        if (Input.GetButtonDown("Exit"))
         {
           MenuSettings.OpenMenu();
         }

         if (Input.GetKeyDown(KeyCode.N))
         {
             noclip = !noclip;
             if (noclip)
             {
                 ToggleUI();
                 CmdToggleNoclip();
                
                 
             }
         }
         if (noclip)
             return;
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
                ToggleUI();
            }


       
        }
    }
    [Command]
    public void CmdToggleNoclip()
    {
        ToggleNoclip();
        RpcToggleNoclip();
    }
    [ClientRpc]
    public void RpcToggleNoclip()
    {
        ToggleNoclip();
    }
    public void ToggleUI()
    {
        GameObject hud = Gamemode.hud;
        Debug.Log(hud.activeSelf);
        hud.SetActive(!hud.activeSelf);
        PlayerNetwork.localPlayer.transform.FindChild("Kart").gameObject.SetActive(hud.activeSelf);
    }
    public void ToggleNoclip()
    {
        GetComponent<KartBehaviour>().enabled = false;
        GetComponent<UpdateKartInformation>().enabled = false;
        GetComponent<Weapon>().enabled = false;
        GetComponent<Placement>().enabled = false;

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;

        transform.Find("Kart").gameObject.active = false;
        gameObject.AddComponent<Noclip>();

        GameObject.Find("Gamemode").GetComponent<Gamemode>().RemovePlayer(gameObject);
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
