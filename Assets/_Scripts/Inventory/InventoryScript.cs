using UnityEngine;
using System.Collections;

public class InventoryScript : MonoBehaviour
{

    //TODO: Better names one we know them
    public enum WEAPON { Weapon1, Weapon2, Weapon3, noWeapon };
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
        currentWeapon = ((WEAPON)Random.Range(0, 2));
    }

    public void updateWeaponTexture()
    {
        if(currentWeapon == WEAPON.noWeapon)
        {
            imageScript.updateSprite(noWeaponSprite);
        }

        else if(currentWeapon == WEAPON.Weapon1)
        {
            imageScript.updateSprite(weapon1Sprite);
        }

        else if(currentWeapon == WEAPON.Weapon2)
        {
            imageScript.updateSprite(weapon2Sprite);
        }

        else if(currentWeapon == WEAPON.Weapon3)
        {
            imageScript.updateSprite(weapon3Sprite);
        }       
    }


}
