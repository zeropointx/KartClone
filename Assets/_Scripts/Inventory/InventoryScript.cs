using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class InventoryScript : NetworkBehaviour
{

    //TODO: Better names once we know them
    public enum WEAPON { BowlingBall, noWeapon };
    public Sprite weapon1Sprite, weapon2Sprite, weapon3Sprite, noWeaponSprite;
    int weaponAmount;
    [SyncVar]
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
        if (isLocalPlayer)
            return;
        if (other.gameObject.tag == "weaponBox")
        {
            Debug.Log("Collided with weaponBox!");

            if (currentWeapon == WEAPON.noWeapon)
            {
                pickUpRandomWeapon();
                Destroy(other.gameObject);
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
