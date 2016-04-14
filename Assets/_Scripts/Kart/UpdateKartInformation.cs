using UnityEngine;
using System.Collections;

// Class used for changing the model of the kart
public class UpdateKartInformation : MonoBehaviour {
    KartBehaviour KB;   // Local KartBehaviour
    GameObject Kart;    // 

    public GameObject ShoppingKart, BananaKart, roadwarrior; //Kart models
    void Awake ()
    {
        KB = gameObject.GetComponent<KartBehaviour>();

        // Checks stored kart information for ID of the chosen kart
        if(StoredKartInfo.characterID == 0)
        {
            updatePlayerComponents(ShoppingKart, 65, 0.25f, 100);
        }
        if (StoredKartInfo.characterID == 1)
        {
            updatePlayerComponents(BananaKart, 65, 0.25f, 100);
        }
    }

    public void updatePlayerComponents(GameObject Model, float MaxSpeed, float Acceleration, float TurnSpeed)
    {
        // Change the player model to the new kart model loaded before
        GameObject newKart = Instantiate(Model, transform.position, Quaternion.identity) as GameObject;
        newKart.name = "Kart";
        newKart.transform.parent = gameObject.transform;    // Make it child of the player

       // newKart.transform.localPosition = new Vector3(0, 0, 0);
        //newKart.transform.localScale = new Vector3(1, 1, 1);
      //  newKart.transform.localEulerAngles = new Vector3(0, 0, 0);

        // Give it MaximumSpeed, Acceleration & Turn speed that were given before
        KB.maxSpeed = MaxSpeed;
        KB.acceleration = Acceleration;
        KB.turnSpeed = TurnSpeed;
       // Destroy(this);
    }
}
