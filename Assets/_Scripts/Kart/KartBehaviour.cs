using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour
{

    public enum KartState
    {
        FORWARD, STOPPED, REVERSE, JUMP
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
    public float spinSpeed;

    //common
    public float jumpLimit;
    public KartState state = KartState.STOPPED;
    private GameObject mainCamera = null;
    private Rigidbody rigidbody = null;
    private KartPhysics physicsScript = null;
    private float stopTimer = 0;
    private float spinTimer = 0;
    public float spinTime = 3;
    PlayerNetwork pw;
    private Vector3 oldPosition = new Vector3(0, 0, 0);
    private float trueSpeed = 0.0f;

    // Use this for initialization
    void Start()
    {
        //stats
        maxSpeed = 65;
        maxReverse = -15;
        turnSpeed = 100;
        acceleration = 0.25f;
        brakeForce = 1.25f;
        engineDeceleration = 0.15f;
        spinSpeed = 250;

        //common
        jumpLimit = 1.75f;
        mainCamera = transform.FindChild("Main Camera").gameObject;
        physicsScript = transform.gameObject.GetComponent<KartPhysics>();
        rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.centerOfMass = new Vector3(0, -3.0f, 0.25f);
        pw = gameObject.GetComponent<PlayerNetwork>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedChange = 0;
        float steer = steeringWheel;

        //controls
        switch (state)
        {
            case KartState.FORWARD:
                if (pedal != 0)
                    speedChange = (pedal > 0) ? acceleration : brakeForce;
                speed -= engineDeceleration;
                speed += speedChange * pedal;
                speed = Mathf.Clamp(speed, 0, maxSpeed);
                if (speed == 0)
                    state = KartState.STOPPED;
                break;

            case KartState.STOPPED:
                stopTimer += Time.deltaTime;
                speed = 0;
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
                steer *= -1.0f;
                if (pedal != 0)
                    speedChange = (pedal > 0) ? brakeForce : acceleration;
                speed += engineDeceleration;
                speed += speedChange * pedal;
                speed = Mathf.Clamp(speed, maxReverse, 0);
                if (speed == 0)
                    state = KartState.STOPPED;
                break;

            case KartState.JUMP:
                speed -= engineDeceleration;
                speed = Mathf.Clamp(speed, 0, maxSpeed);
                break;

            default:
                Debug.LogError("invalid Kart state!");
                break;
        }

        checkHitUpdate();

        Vector3 direction = transform.forward;
        direction -= physicsScript.GetGroundNormal() * 0.15f * Time.deltaTime;
        if (physicsScript.GetGroundDistance() < 3.0f)
            transform.Rotate(new Vector3(0, turnSpeed * steer * Time.deltaTime, 0));
        oldPosition = transform.position;
        transform.position += speed * Time.deltaTime * direction;
        trueSpeed = Vector3.Distance(transform.position, oldPosition) / Time.deltaTime;
    }

    void LateUpdate()
    {
        mainCamera.transform.LookAt(transform);
    }

    //public

    public void SetPedal(float pedalValue)
    {
        pedal = pedalValue;
    }

    public void SetSteer(float wheelPosition)
    {
        steeringWheel = wheelPosition;
    }

    public float GetSpeed()
    {
        return Mathf.Abs(trueSpeed);
    }

    public KartState GetState()
    {
        return state;
    }

    public void SetState(KartState _state)
    {
        state = _state;
    }

    public void Reset(float speedMultiplier = 0)
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        rigidbody.velocity *= speedMultiplier;
        rigidbody.angularVelocity *= speedMultiplier;
        speed *= speedMultiplier;
    }

    public float getJumpLimit()
    {
        return jumpLimit;
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
