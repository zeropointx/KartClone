﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    GameObject lapText;
     
     GameObject weaponImageUI;
    GameObject track;
    GameObject placementText;
     weaponImageScript imageScript;
     public Gamemode gamemode;
     public Sprite bowlingBallSprite, speedBoostSprite, weapon3Sprite, noWeaponSprite;
     InventoryScript.WEAPON uiweapon = InventoryScript.WEAPON.noWeapon;
     GameObject speedIndicator;
     public float speedy = 0.0f;
	// Use this for initialization
	void Start () {
        weaponImageUI = transform.FindChild("weaponImageUI").gameObject;
        imageScript = weaponImageUI.GetComponent<weaponImageScript>();
        lapText = GameObject.Find("LapText");
        track = GameObject.FindGameObjectsWithTag("track")[0].transform.root.gameObject;
        placementText = transform.FindChild("Placement").gameObject;
        speedIndicator = GameObject.Find("ATJMittari");
	}
	
	// Update is called once per frame
	void Update () {
          if (PlayerNetwork.localPlayer == null)
          {
              updateWeaponTexture(InventoryScript.WEAPON.noWeapon);
              return;
          }
          InventoryScript.WEAPON currentWeapon = PlayerNetwork.localPlayer.GetComponent<InventoryScript>().currentWeapon;
        if(currentWeapon != uiweapon)
            updateWeaponTexture(currentWeapon);
        lapText.GetComponent<Text>().text = "Lap: " + PlayerNetwork.localPlayer.GetComponent<Placement>().currentLap + "\\" + track.GetComponent<TrackInformation>().lapAmount;
        int placement = gamemode.getPlacement(PlayerNetwork.localPlayer);
       
        placementText.GetComponent<Text>().text = placement +" "+ getPlacementString(placement);
        int speed = (int)PlayerNetwork.localPlayer.GetComponent<KartBehaviour>().GetSpeed();
        Vector3 eulerAngles = new Vector3();
        eulerAngles.z = 0.0f;
        float eulerMax = -388;
        float eulerMin = -149.0f;
        float maxSpeed = 160.0f;
        eulerAngles.z = -(Mathf.Abs(eulerMax - eulerMin) / maxSpeed * speed) + eulerMin;
        speedIndicator.transform.eulerAngles = eulerAngles;
        speedy = eulerAngles.z;
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
      
            imageScript.updateSprite(noWeaponSprite);
       
        if (currentWeapon == InventoryScript.WEAPON.noWeapon)
        {
            imageScript.updateSprite(noWeaponSprite);
        }

        if (currentWeapon == InventoryScript.WEAPON.BowlingBall)
        {
            imageScript.updateSprite(bowlingBallSprite);
        }

        if (currentWeapon == InventoryScript.WEAPON.SpeedBoost)
        {
            imageScript.updateSprite(speedBoostSprite);
        }
        uiweapon = currentWeapon;
    }
}
