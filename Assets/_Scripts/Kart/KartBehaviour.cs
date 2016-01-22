using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour {

    enum KartState
    {
        FORWARD, STOPPED, REVERSE
    };

    private float steeringWheel;
    private float pedal;
    private float speed;
    private float maxSpeed;
    private float maxReverse;
    private float turnSpeed;
    private float acceleration;
    private float brakeForce;
    private float engineDeceleration;
    private float groundDistance;
    private Vector3 groundNormal;
    private GameObject mainCamera;
    private float tiltLimitX;
    private float tiltLimitZ;
    KartState state;
    float collisionImmunityTimer;

	// Use this for initialization
	void Start () {
        steeringWheel = 0;
        pedal = 0;
        speed = 0;
        maxSpeed = 45;
        maxReverse = 15;
        turnSpeed = 75;
        acceleration = 0.35f;
        brakeForce = 1.25f;
        engineDeceleration = 0.15f;
        groundDistance = 0;
        mainCamera = transform.FindChild("Main Camera").gameObject;
        tiltLimitX = 45;
        tiltLimitZ = 45;
        collisionImmunityTimer = 0;
        state = KartState.STOPPED;
	}
	
	// Update is called once per frame
	void Update () {
        //timers
        collisionImmunityTimer += Time.deltaTime;

        float speedChange = 0;

        //controls
        switch(state)
        {
            case KartState.FORWARD:
                break;
            case KartState.STOPPED:
                //speed = 0;
                break;
            case KartState.REVERSE:
                break;
            default:
                break;
        }

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

    void OnCollisionEnter(Collision collision){
        if (collisionImmunityTimer > 3)
        {
            if (collision.impulse.magnitude > 100 && speed > 0.25f * maxSpeed)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Ray(transform.position + new Vector3(0, 1.25f, 0), transform.forward), out hit))
                {
                    if (hit.distance < 10)
                    {
                        Reset();
                        transform.position -= 2.0f * transform.forward;
                        collisionImmunityTimer = 0;
                        Debug.Log("hard kart collision");
                    }
                }
            }
        }     
    }

    //own functions

    private float UpdateSteer() {
        float result = 0;
        if (0.0f < Mathf.Abs(speed))
            result = steeringWheel;
        return result;
    }

    private bool GroundCollision() {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit))
        {
            groundNormal = hit.normal;
            groundDistance = hit.distance;
            if (groundDistance > 2.5f)
                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.FromToRotation(transform.up, groundNormal), Time.deltaTime);

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

    public void Reset(float speedMultiplier = 0) {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        speed *= speedMultiplier;
    }
}
