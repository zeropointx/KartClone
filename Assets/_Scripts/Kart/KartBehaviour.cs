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
        rigidbody.centerOfMass = new Vector3(0, transform.gameObject.GetComponent<BoxCollider>().size.y * -0.35f, 0.0f);
        pw = gameObject.GetComponent<PlayerNetwork>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
           pw.hitState = PlayerNetwork.KartHitState.SPINNING;
        Vector3 oldPosition = transform.position;
        KartState tempState = state.UpdateState();
        if (tempState != null)
            state = tempState;
        trueSpeed = Vector3.Distance(transform.position, oldPosition) / Time.deltaTime;
    }

    void LateUpdate()
    {
        mainCamera.transform.LookAt(transform);
    }

    public void UpdateTransform(float controlMultiplier = 1.0f)
    {
        Vector3 direction = transform.forward;
        direction -= groundNormal * 0.5f * Time.deltaTime;
        if (groundDistance < 2.0f)
            transform.Rotate(new Vector3(0, controlMultiplier * turnSpeed * steeringWheel * Time.deltaTime, 0));
        
        transform.position += speed * Time.deltaTime * direction;
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
