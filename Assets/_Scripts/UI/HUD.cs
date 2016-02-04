using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    GameObject lapText;
       public GameObject localPlayer = null;
     GameObject weaponImageUI;
    GameObject track;
    GameObject placementText;
    GameObject speedText;
     weaponImageScript imageScript;
     public Gamemode gamemode;
     public Sprite weapon1Sprite, weapon2Sprite, weapon3Sprite, noWeaponSprite;
     InventoryScript.WEAPON uiweapon = InventoryScript.WEAPON.noWeapon;
	// Use this for initialization
	void Start () {
        weaponImageUI = transform.FindChild("weaponImageUI").gameObject;
        imageScript = weaponImageUI.GetComponent<weaponImageScript>();
        lapText = GameObject.Find("LapText");
        track = GameObject.FindGameObjectsWithTag("track")[0].transform.root.gameObject;
        placementText = transform.FindChild("Placement").gameObject;
        speedText = transform.FindChild("Speed").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
          if (localPlayer == null)
          {
              updateWeaponTexture(InventoryScript.WEAPON.noWeapon);
              return;
          }
         InventoryScript.WEAPON currentWeapon = localPlayer.GetComponent<InventoryScript>().currentWeapon;
        if(currentWeapon != uiweapon)
            updateWeaponTexture(currentWeapon);
        lapText.GetComponent<Text>().text = "Lap: " + localPlayer.GetComponent<Placement>().currentLap + "\\" + track.GetComponent<TrackInformation>().lapAmount;
        placementText.GetComponent<Text>().text = gamemode.getPlacement(localPlayer) + " th";

        speedText.GetComponent<Text>().text = "Speed: " + (int)localPlayer.GetComponent<KartBehaviour>().GetSpeed();
	}

    public void updateWeaponTexture(InventoryScript.WEAPON currentWeapon)
    {
      
            imageScript.updateSprite(noWeaponSprite);
       
        if (currentWeapon == InventoryScript.WEAPON.noWeapon)
        {
            imageScript.updateSprite(noWeaponSprite);
        }

        else if (currentWeapon == InventoryScript.WEAPON.BowlingBall)
        {
            imageScript.updateSprite(weapon1Sprite);
        }
        uiweapon = currentWeapon;
    }
}
