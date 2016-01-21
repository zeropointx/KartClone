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
    private float groundDistance;
    private Vector3 groundNormal;
    private GameObject mainCamera;
    private float tiltLimitX;
    private float tiltLimitZ;

	// Use this for initialization
	void Start () {
        steeringWheel = 0;
        pedal = 0;
        speed = 0;
        maxSpeed = 55;
        turnSpeed = 75;
        acceleration = 0.5f;
        brakeForce = 1.5f;
        engineDeceleration = 0.25f;
        groundDistance = 0;
        mainCamera = transform.FindChild("Main Camera").gameObject;
        tiltLimitX = 30;
        tiltLimitZ = 45;
	}
	
	// Update is called once per frame
	void Update () {
        float speedChange = 0;

        //controls
        if (Mathf.Abs(pedal) > 0)
            speedChange = (pedal < 0) ? brakeForce : acceleration;
        float steer = UpdateSteer();

        //transform
        speed -= engineDeceleration;
        if (groundDistance < 5)
        {
            speed += speedChange * pedal;
            if (Mathf.Abs(steer) > 0)
                transform.Rotate(new Vector3(0, turnSpeed * steer * Time.deltaTime, 0));
        }
        speed = Mathf.Min(speed, maxSpeed);
        speed = Mathf.Max(speed, 0);
        Vector3 direction = transform.forward;
        if (groundDistance > 2)
            direction -= groundNormal * 0.25f;

        transform.position += speed * Time.deltaTime * direction;
    }

    void LateUpdate() {
        GroundCollision();
        UpdateTilt();

        //camera
        mainCamera.transform.LookAt(transform);
    }

    private float UpdateSteer() {
        float result = 0;
        if (0.0f < Mathf.Abs(speed))
        {
            result = steeringWheel;
            if (Mathf.Abs(speed) < 1.0f)
                result *= 0.005f;
            else if (Mathf.Abs(speed) > 1.5f)
            {
                result = steeringWheel;
                if (speed < 0.5f * maxSpeed)
                    result *= ((maxSpeed / Mathf.Abs(speed)) * 0.5f);
            }
        }
        return result;
    }

    private bool GroundCollision() {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit))
        {
            groundNormal = hit.normal;
            groundDistance = hit.distance;
            return true;
        }
        return false;
    }

    private void UpdateTilt() {
        
        float x = transform.eulerAngles.x;
        float y = transform.eulerAngles.y;
        float z = transform.eulerAngles.z;

        if (x < 180)
            x = Mathf.Min(x, tiltLimitX);
        else
            x = Mathf.Max(x, 360 - tiltLimitX);

        if (z < 180)
            z = Mathf.Min(z, tiltLimitZ);
        else
            z = Mathf.Max(z, 360 - tiltLimitZ);
        

        transform.rotation = Quaternion.Euler(x, y, z);
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

    public float GetSpeed() {
        return speed;
    }

    public void Reset() {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        speed = 0;
    }
}
