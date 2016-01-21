using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
    public GameObject localPlayer;
     GameObject weaponImageUI;
     weaponImageScript imageScript;
     public Sprite weapon1Sprite, weapon2Sprite, weapon3Sprite, noWeaponSprite;
	// Use this for initialization
	void Start () {
        weaponImageUI = transform.FindChild("weaponImageUI").gameObject;
        imageScript = weaponImageUI.GetComponent<weaponImageScript>();
	}
	
	// Update is called once per frame
	void Update () {
            updateWeaponTexture();

	}

    public void updateWeaponTexture()
    {
        InventoryScript.WEAPON currentWeapon = localPlayer.GetComponent<InventoryScript>().currentWeapon;
        if (currentWeapon == InventoryScript.WEAPON.noWeapon)
        {
            imageScript.updateSprite(noWeaponSprite);
        }

        else if (currentWeapon == InventoryScript.WEAPON.BowlingBall)
        {
            imageScript.updateSprite(weapon1Sprite);
        }
    }
}
