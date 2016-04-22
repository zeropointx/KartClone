using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    GameObject lapText;
    GameObject stateNameText;
     GameObject weaponImageUI;
    GameObject track;
    GameObject placementText;
     weaponImageScript imageScript;
     public Gamemode gamemode;
     public Sprite[] weaponSprites;
     InventoryScript.WEAPON uiweapon = InventoryScript.WEAPON.noWeapon;
     GameObject speedIndicator;
     public float speedy = 0.0f;
     float speedMeter = 0.0f;
	// Use this for initialization
	void Start () {
        weaponImageUI = transform.FindChild("weaponImageUI").gameObject;
        imageScript = weaponImageUI.GetComponent<weaponImageScript>();
        lapText = GameObject.Find("LapText");
        track = GameObject.FindGameObjectsWithTag("track")[0].transform.root.gameObject;
        placementText = transform.FindChild("Placement").gameObject;
        speedIndicator = GameObject.Find("ATJMittari");
        stateNameText = GameObject.Find("stateNameText");
     
	}
	
	// Update is called once per frame
	void Update () {

        if (gamemode == null)
        {
            var dump = GameObject.Find("Gamemode");
            if(dump != null)
            gamemode = dump.GetComponent<Gamemode>();
            return;
        }
          if (PlayerNetwork.localPlayer == null)
          {
              updateWeaponTexture(InventoryScript.WEAPON.noWeapon);
              return;
          }
          InventoryScript.WEAPON currentWeapon = PlayerNetwork.localPlayer.GetComponent<InventoryScript>().currentWeapon;
        if(currentWeapon != uiweapon)
            updateWeaponTexture(currentWeapon);
        lapText.GetComponent<Text>().text = "Lap:" + PlayerNetwork.localPlayer.GetComponent<Placement>().currentLap + "\\" + track.GetComponent<TrackInformation>().lapAmount;
        int placement = gamemode.getPlacement(PlayerNetwork.localPlayer);
       
        placementText.GetComponent<Text>().text = placement +" "+ getPlacementString(placement);
        stateNameText.GetComponent<Text>().text = PlayerNetwork.localPlayer.GetComponent<KartBehaviour>().state.GetName();

        gamemode.checkGameFinish();
	}
    public string getPlacementString(int placement)
    {
        switch (placement)
        {
            case 1:
                {
                    return "st";
                }
            case 2:
                {
                    return "nd";

                }
            case 3:
                {
                    return "rd";
                }
            default:
                {
                    return "th";
                }
        }
        
    }
    public void updateWeaponTexture(InventoryScript.WEAPON currentWeapon)
    {
        int index = (int)currentWeapon;
        Sprite weaponSprite = weaponSprites[index];
        if (weaponSprite == null)
            return;

        imageScript.updateSprite(weaponSprite);

        uiweapon = currentWeapon;
    }
}
