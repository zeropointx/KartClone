using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour {

    private float steeringWheel;
    private float pedal;
    private float speed;
    private float maxSpeed;
    private float turnSpeed;
    private float maxTurn;
    private float acceleration;
    private float brakeForce;
    private float engineDeceleration;

	// Use this for initialization
	void Start () {
        maxSpeed = 50;
        speed = 0;
        turnSpeed = 75;
        acceleration = 25;
        brakeForce = 45;
        engineDeceleration = 10f;
	}
	
	// Update is called once per frame
	void Update () {
        float speedChange = 0;
        float rotation = 0;

        //gas
        if (Mathf.Abs(pedal) > 0)
            speedChange = (pedal < 0) ? brakeForce : acceleration;

        //steer
        if (Mathf.Abs(speed) > 0)
        {
            rotation = steeringWheel;
            if (speed < 0.5f * maxSpeed)
                rotation *= Mathf.Abs(speed) * 0.075f;
            //speedChange *= 0.75f;
        }    
        
        //speed
        speed -= engineDeceleration * Time.deltaTime;
        speed += speedChange * pedal * Time.deltaTime;
        speed = Mathf.Min(speed, maxSpeed);
        speed = Mathf.Max(speed, 0);

        //transform
        transform.Rotate(new Vector3(0, turnSpeed * rotation * Time.deltaTime, 0));
        transform.position += speed * Time.deltaTime * transform.forward;
	}

    public void Accelerate(float pedalValue) {
        pedal = pedalValue;
        pedal = Mathf.Min(pedal, 1);
        pedal = Mathf.Max(pedal, -1);
    }

    public void Steer(float wheelPosition) {
        steeringWheel = wheelPosition;
        steeringWheel = Mathf.Min(steeringWheel, 1);
        steeringWheel = Mathf.Max(steeringWheel, -1);
    }

    public float getSpeed() {
        return speed;
    }
}
