using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour
{

    public enum KartState
    {
        FORWARD, STOPPED, REVERSE
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
    public float flyLimit = 2.0f;
    public KartState state = KartState.STOPPED;
    private float groundDistance = 0;
    private GameObject mainCamera = null;
    private Rigidbody rigidbody = null;
    private Vector3 groundNormal = new Vector3(0, 0, 0);
    private float collisionImmunityTimer = 0;
    private float stopTimer = 0;

    private float spinTimer = 0;
    public float spinTime = 3;
    PlayerNetwork pw;

    // Use this for initialization
    void Start()
    {
        maxSpeed = 40;
        maxReverse = -15;
        turnSpeed = 75;
        acceleration = 0.275f;
        brakeForce = 1.25f;
        engineDeceleration = 0.15f;
        mainCamera = transform.FindChild("Main Camera").gameObject;
        rigidbody = transform.GetComponent<Rigidbody>();
        pw = gameObject.GetComponent<PlayerNetwork>();
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
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
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

            default:
                Debug.LogError("invalid Kart state!");
                break;
        }

        checkHitUpdate();

        Vector3 direction = transform.forward;
        direction -= groundNormal * 0.25f * Time.deltaTime;
        transform.position += speed * Time.deltaTime * direction;
    }

    void LateUpdate()
    {
        GroundCollision();

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

    private void GroundCollision()
    {
        RaycastHit hit1;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit1))
        {
            if (hit1.transform.gameObject.name == "Track")
            {
                groundNormal = hit1.normal;
                groundDistance = hit1.distance;
                RaycastHit hit2;
                if (Physics.Raycast(new Ray(transform.position, -transform.up), out hit2))
                {
                    if (hit2.transform.gameObject.name == "Track")
                    {
                        if (hit2.distance > 0.75f)
                            transform.rotation = Quaternion.Slerp/*Unclamped*/(transform.rotation, Quaternion.FromToRotation(transform.up, groundNormal), hit2.distance * Time.deltaTime);
                    }
                    else
                        Reset();
                    if (Vector3.Angle(transform.position - hit1.normal, transform.position - hit2.normal) > 5.0f)
                    {
                        transform.rotation = Quaternion.Slerp/*Unclamped*/(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), 1.0f * Time.deltaTime);
                    }
                }
                else
                    Reset(0.25f);
            }
        }
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
        rigidbody.velocity *= speedMultiplier;
        rigidbody.angularVelocity *= speedMultiplier;
        speed *= speedMultiplier;
    }

    void checkHitUpdate()
    {
        if (pw.hitState == PlayerNetwork.KartHitState.SPINNING)
        {
            speed = 0;
            spinTimer += Time.deltaTime;
            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
            if (spinTimer > spinTime)
            {
                spinTimer = 0;
                state = KartState.STOPPED;
                pw.hitState = PlayerNetwork.KartHitState.NORMAL;
            }
        }
    }
}
