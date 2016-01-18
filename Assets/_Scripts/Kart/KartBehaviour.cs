using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour {

    private float steeringWheel;
    private float pedal;
    private float speed;
    private float maxSpeed;
    private float turnSpeed;
    private float acceleration;
    private float brakeForce;
    private float engineDeceleration;
    private GameObject mainCamera;

	// Use this for initialization
	void Start () {
        steeringWheel = 0;
        pedal = 0;
        speed = 0;
        maxSpeed = 50;
        turnSpeed = 100;
        acceleration = 2.5f;
        brakeForce = 2.5f;
        engineDeceleration = 0.5f;
        mainCamera = transform.FindChild("Main Camera").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        float speedChange = 0;
        float rotation = 0;

        //gas
        if (Mathf.Abs(pedal) > 0)
            speedChange = (pedal < 0) ? brakeForce : acceleration;

        //steer
        if (0.0f < Mathf.Abs(speed))
        {
            rotation = steeringWheel;
            if (Mathf.Abs(speed) < 1.0f)
                rotation *= 0.005f;
            else if (Mathf.Abs(speed) > 1.5f)
            {
                rotation = steeringWheel;
                if (speed < 0.5f * maxSpeed)
                    rotation *= ((maxSpeed / Mathf.Abs(speed)) * 0.5f);
            }
        }
        else
            rotation = 0;

        //speed
        speed -= engineDeceleration;
        speed += speedChange * pedal;
        speed = Mathf.Min(speed, maxSpeed);
        speed = Mathf.Max(speed, 0);

        //transform
        if (Mathf.Abs(rotation) > 0)
            transform.Rotate(new Vector3(0, turnSpeed * rotation * Time.deltaTime, 0));
        transform.position += speed * Time.deltaTime * transform.forward;
        mainCamera.transform.LookAt(transform);
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
