using UnityEngine;
using System.Collections;

public class InventoryScript : MonoBehaviour
{

    //TODO: Better names one we know them
    public enum WEAPON { BowlingBall, noWeapon };
    public Sprite weapon1Sprite, weapon2Sprite, weapon3Sprite, noWeaponSprite;
    int weaponAmount;
    public WEAPON currentWeapon;
    public GameObject weaponImageUI;
    weaponImageScript imageScript;
    public int randomSeed = 1337;


    void Start()
    {
        Random.seed = randomSeed;
        currentWeapon = WEAPON.noWeapon;
        weaponImageUI = GameObject.Find("weaponImageUI");
        imageScript = weaponImageUI.GetComponent<weaponImageScript>();
    }

    void Update()
    {
        updateWeaponTexture();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "weaponBox")
        {
            Debug.Log("Collided with weaponBox!");

            if (currentWeapon == WEAPON.noWeapon)
            {
                pickUpRandomWeapon();
            }
        }
    }

    public void pickUpRandomWeapon()
    {
        currentWeapon = ((WEAPON)Random.Range(0, 0));
    }

    public void updateWeaponTexture()
    {
        if (currentWeapon == WEAPON.noWeapon)
        {
            imageScript.updateSprite(noWeaponSprite);
        }

        else if (currentWeapon == WEAPON.BowlingBall)
        {
            imageScript.updateSprite(weapon1Sprite);
        }
    }


}
