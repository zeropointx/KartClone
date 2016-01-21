using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class InventoryScript : NetworkBehaviour
{

    //TODO: Better names once we know them
    public enum WEAPON { BowlingBall, noWeapon };

    int weaponAmount;
    [SyncVar]
    public WEAPON currentWeapon;


    public int randomSeed = 1337;


    void Start()
    {
        Random.seed = randomSeed;
        currentWeapon = WEAPON.noWeapon;

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer)
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




}
