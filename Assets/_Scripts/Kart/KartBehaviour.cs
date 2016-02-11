using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour
{
    //controls
    public float steeringWheel = 0;
    public float pedal = 0;
    public float speed = 0;

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
    public KartState state = null;
    public GameObject mainCamera = null;
    public Rigidbody rigidbody = null;
    public PlayerNetwork pw;
    
    //private
    private Vector3 oldPosition = new Vector3(0, 0, 0);
    private float trueSpeed = 0.0f;

    //physics
    public Vector3 groundNormal = new Vector3(0, 0, 0);
    public float groundDistance = 0;
    public Vector3 lastTrackPosition = new Vector3(0, 0, 0);

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
        state = new Stopped(this.gameObject);
        mainCamera = transform.FindChild("Main Camera").gameObject;
        rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.centerOfMass = new Vector3(0, -2.0f, 0.0f);
        pw = gameObject.GetComponent<PlayerNetwork>();
    }

    // Update is called once per frame
    void Update()
    {
        KartState tempState = state.UpdateState();
        if (tempState != null)
            state = tempState;
    }

    void LateUpdate()
    {
        mainCamera.transform.LookAt(transform);
    }

    //public

    public void GroundCollision()
    {
        RaycastHit relative;
        if (Physics.Raycast(new Ray(transform.position, -transform.up), out relative))
        {
            if (relative.transform.gameObject.tag == "track")
            {
                if (Vector3.Angle(transform.position - groundNormal, transform.position - relative.normal) > 2.5f)
                    transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), 1.0f * Time.deltaTime);
                Debug.DrawRay(transform.position, -transform.up, Color.blue, 0.1f);
            }
        }
    }

    public void UpdateTransform()
    {
        Vector3 direction = transform.forward;
        direction -= groundNormal * 0.15f * Time.deltaTime;
        if (groundDistance < 3.0f)
            transform.Rotate(new Vector3(0, turnSpeed * steeringWheel * Time.deltaTime, 0));
        oldPosition = transform.position;
        transform.position += speed * Time.deltaTime * direction;
        trueSpeed = Vector3.Distance(transform.position, oldPosition) / Time.deltaTime;
    }

    public bool UpdateGroundDistance()
    {
        RaycastHit directDown;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out directDown))
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.green, 0.1f);
            if (directDown.transform.gameObject.tag == "track")
            {
                groundNormal = directDown.normal;
                groundDistance = directDown.distance;
                lastTrackPosition = directDown.point + 3.0f * Vector3.up - 16.0f * transform.forward;
            }
            return true;
        }
        else
        {
            groundDistance = float.MaxValue;
            return false;
        }
    }

    public void Reset(float speedMultiplier = 0)
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        rigidbody.velocity *= speedMultiplier;
        rigidbody.angularVelocity *= speedMultiplier;
        speed *= speedMultiplier;
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
    public float getJumpLimit()
    {
        return jumpLimit;
    }
    public void SetPedal(float pedalValue)
    {
        pedal = pedalValue;
    }
}
