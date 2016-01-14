using UnityEngine;
using System.Collections;

public class InventoryScript : MonoBehaviour
{

    //TODO: Better names one we know them
    public enum WEAPON { Weapon1, Weapon2, Weapon3, noWeapon };
    public Sprite weapon1Sprite, weapon2Sprite, weapon3Sprite, noWeaponSprite;
    int weaponAmount;
    public WEAPON currentWeapon;
    public GameObject weaponImage;
    weaponImageScript imageScript;
    

    void Start()
    {
        currentWeapon = WEAPON.noWeapon;
        imageScript = weaponImage.GetComponent<weaponImageScript>();
    }

    void Update()
    {
        updateWeaponTexture();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "weaponBox")
        {
            Debug.Log("Collided with weaponBox!");
            pickUpRandomWeapon();
        }
    }

    public void pickUpRandomWeapon()
    {
        currentWeapon = ((WEAPON)Random.Range(0, weaponAmount-1));
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
