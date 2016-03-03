using UnityEngine;
using System.Collections;

public class UpdateKartInformation : MonoBehaviour {
    KartBehaviour KB;
    GameObject Kart;

    public GameObject ShoppingKart, BananaKart;
    void Awake ()
    {
        KB = gameObject.GetComponent<KartBehaviour>();
        //Kart = gameObject.transform.Find("Kart").gameObject;

        if(StoredKartInfo.characterID == 0)
        {
            updatePlayerComponents(ShoppingKart, 65, 0.25f, 100);
        }
        if (StoredKartInfo.characterID == 1)
        {
            updatePlayerComponents(BananaKart, 65, 0.25f, 100);
        }
    }
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updatePlayerComponents(GameObject Model, float MaxSpeed, float Acceleration, float TurnSpeed)
    {
        GameObject newKart = Instantiate(Model, transform.position, Quaternion.identity) as GameObject;
        newKart.name = "Kart";
        newKart.transform.parent = gameObject.transform;

       // newKart.transform.localPosition = new Vector3(0, 0, 0);
        //newKart.transform.localScale = new Vector3(1, 1, 1);
      //  newKart.transform.localEulerAngles = new Vector3(0, 0, 0);

        KB.maxSpeed = MaxSpeed;
        KB.acceleration = Acceleration;
        KB.turnSpeed = TurnSpeed;
       // Destroy(this);
    }
}
