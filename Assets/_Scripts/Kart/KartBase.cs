using UnityEngine;
using System.Collections;

public class KartBase : MonoBehaviour 
{
    KartBehaviour KB;
    GameObject kartModel;
    PlayerNetwork PN;
    //public float kartMaxSpeed;
    //public float kartAcceleration;
    //public float kartTurnSpeed;

    void Start()
    {
        PN = gameObject.GetComponent<PlayerNetwork>();
        KB = gameObject.GetComponent<KartBehaviour>();
        kartModel = transform.Find("Kart").gameObject;
    }

    void Update()
    {

    }

    public void updatePlayerComponents(GameObject Model, float MaxSpeed, float Acceleration, float TurnSpeed)
    {
        kartModel = Model;
        KB.maxSpeed = MaxSpeed;
        KB.acceleration = Acceleration;
        KB.turnSpeed = TurnSpeed;
    }
}
