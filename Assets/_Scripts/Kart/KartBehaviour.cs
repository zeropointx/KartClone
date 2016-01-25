using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour
{

    public enum KartState
    {
        FORWARD, STOPPED, REVERSE, SPINNING
    };

    //TODO set as private
    //controls
    private float steeringWheel = 0;
    private float pedal = 0;
    private float speed = 0;

    //stats
    public float maxSpeed;
    public float maxReverse;
    public float turnSpeed;
    public float acceleration;
    public float brakeForce;
    public float engineDeceleration;
    public float spinSpeed = 250;

    //common
    public float tiltLimitX = 30;
    public float tiltLimitZ = 45;
    public float flyLimit = 2.0f;
    public KartState state = KartState.STOPPED;
    private float groundDistance = 0;
    private GameObject mainCamera = null;
    private Vector3 groundNormal = new Vector3(0, 0, 0);
    private float collisionImmunityTimer = 0;
    private float stopTimer = 0;
    private float spinTimer = 0;

    // Use this for initialization
    void Start()
    {
        maxSpeed = 45;
        maxReverse = -15;
        turnSpeed = 75;
        acceleration = 0.35f;
        brakeForce = 1.25f;
        engineDeceleration = 0.15f;
        mainCamera = transform.FindChild("Main Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //timers
        collisionImmunityTimer += Time.deltaTime;

        float speedChange = 0;

        //controls
        switch (state)
        {
            case KartState.FORWARD:
                if (pedal != 0)
                    speedChange = (pedal > 0) ? acceleration : brakeForce;
                UpdateControls();
                if (groundDistance < flyLimit)
                {
                    speed -= engineDeceleration;
                    speed += speedChange * pedal;
                }
                speed = Mathf.Clamp(speed, 0, maxSpeed);
                if (speed == 0)
                    state = KartState.STOPPED;
                break;

            case KartState.STOPPED:
                stopTimer += Time.deltaTime;
                speed = 0;
                if (stopTimer > 0.25f)
                {
                    if (pedal != 0)
                    {
                        state = (pedal > 0) ? KartState.FORWARD : KartState.REVERSE;
                        stopTimer = 0;
                    }
                }
                break;

            case KartState.REVERSE:
                if (pedal != 0)
                    speedChange = (pedal > 0) ? brakeForce : acceleration;
                UpdateControls();
                if (groundDistance < flyLimit)
                {
                    speed += engineDeceleration;
                    speed += speedChange * pedal;
                }
                speed = Mathf.Clamp(speed, maxReverse, 0);
                if (speed == 0)
                    state = KartState.STOPPED;
                break;

            case KartState.SPINNING:

                spinTimer += Time.deltaTime;

                transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
                if (spinTimer > 3)
                {
                    spinTimer = 0;
                    state = KartState.STOPPED;                 
                }
                break;

            default:
                Debug.LogError("invalid Kart state!");
                break;
        }

        Vector3 direction = transform.forward;
        if (groundDistance > flyLimit)
            direction -= groundNormal * 0.25f;
        transform.position += speed * Time.deltaTime * direction;
    }

    void LateUpdate()
    {
        GroundCollision();
        UpdateTilt();

        //camera
        mainCamera.transform.LookAt(transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        // TODO fix later
        /*
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
        */
    }

    //private

    private bool GroundCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit))
        {
            groundNormal = hit.normal;
            groundDistance = hit.distance;
            if (groundDistance > flyLimit)
                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.FromToRotation(transform.up, groundNormal), Time.deltaTime);

            return true;
        }
        return false;
    }

    private void UpdateTilt()
    {
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

    private void UpdateControls()
    {
        float steer = (Mathf.Abs(speed) > 0) ? steeringWheel : 0;
        if (groundDistance < flyLimit)
        {
            if (Mathf.Abs(steer) > 0)
                transform.Rotate(new Vector3(0, turnSpeed * steer * Time.deltaTime, 0));
        }
    }

    //public

    public void Accelerate(float pedalValue)
    {
        pedal = pedalValue;
        pedal = Mathf.Min(pedal, 1);
        pedal = Mathf.Max(pedal, -1);
    }

    public void Steer(float wheelPosition)
    {
        steeringWheel = wheelPosition;
        steeringWheel = Mathf.Min(steeringWheel, 1);
        steeringWheel = Mathf.Max(steeringWheel, -1);
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void Reset(float speedMultiplier = 0)
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        speed *= speedMultiplier;
    }
}
