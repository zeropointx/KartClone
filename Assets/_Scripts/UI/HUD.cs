using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
       public GameObject localPlayer = null;
     GameObject weaponImageUI;
     weaponImageScript imageScript;
     public Sprite weapon1Sprite, weapon2Sprite, weapon3Sprite, noWeaponSprite;
     InventoryScript.WEAPON uiweapon = InventoryScript.WEAPON.noWeapon;
	// Use this for initialization
	void Start () {
        weaponImageUI = transform.FindChild("weaponImageUI").gameObject;
        imageScript = weaponImageUI.GetComponent<weaponImageScript>();
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
