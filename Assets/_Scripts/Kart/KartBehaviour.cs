using UnityEngine;
using System.Reflection;
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
    public float stabilizeTorqueForce;

    //common
    public float jumpLimit;
    public KartState state = null;
    public GameObject mainCamera = null;
    public Rigidbody rigidbody = null;
    public BoxCollider boxCollider = null;
    public PlayerNetwork pw;

    //private
    private float trueSpeed = 0.0f;
    private KartState networkState = null;
    private float trueSpeedTimer = 0.0f;
    private Vector3 oldPosition;

    //physics
    public Vector3 groundNormal = new Vector3(0, 0, 0);
    public float groundDistance = 0;
    public Vector3 lastTrackPosition = new Vector3(0, 0, 0);

    public Quaternion originalRotation;
    public Transform childKart;

    // Use this for initialization
    void Start()
    {
        childKart = transform.Find("Kart");
        originalRotation = childKart.transform.localRotation;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();

        //stats
        maxSpeed = 65;
        maxReverse = -15;
        turnSpeed = 100;
        acceleration = 0.25f;
        brakeForce = 0.95f;
        engineDeceleration = 0.15f;
        spinSpeed = 250;
        stabilizeTorqueForce = 1000.0f;

        //common
        jumpLimit = 2.5f;
        state = new Stopped(this.gameObject);
        mainCamera = transform.FindChild("Main Camera").gameObject;
        pw = gameObject.GetComponent<PlayerNetwork>();
        oldPosition = transform.position;
        groundNormal = Vector3.up;

        childKart.localPosition = new Vector3(0, 0, 0);
        childKart.localEulerAngles = new Vector3(0, -90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (networkState != null)
        {
            state = networkState;
            networkState = null;
        }

        //state
        KartState tempState = state.UpdateState();
        if (tempState != null)
            state = tempState;

        //speedometer
        trueSpeedTimer += Time.deltaTime;
        if (trueSpeedTimer > 0.25f)
        {
            trueSpeed = Vector3.Distance(transform.position, oldPosition) / trueSpeedTimer;
            trueSpeedTimer = 0;
            oldPosition = transform.position;
        }
    }

    void LateUpdate()
    {   
        if (!(state is Frozen))
            mainCamera.transform.LookAt(childKart.transform);
    }

    void FixedUpdate()
    {
        UpdateGroundDistance();
        state.UpdatePhysicsState();
    }

    void OnCollisionEnter(Collision collision)
    {
        state.CollisionEnter(collision);
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red, 1.0f);
        }
    }

    /*
     * Aligns rigidbody with ground normal
     */
    public void Stabilize()
    {
        Vector3 torque = Vector3.Cross(transform.up, groundNormal) * (1.01f - Vector3.Dot(transform.up, groundNormal));
        rigidbody.AddTorque(torque * stabilizeTorqueForce * Time.deltaTime);
    }

    private void UpdateGroundDistance()
    {
        Vector3 rayOrigin = transform.position - new Vector3(0, 2.0f, 0);
        RaycastHit relative;
        if (Physics.Raycast(new Ray(rayOrigin, -transform.up), out relative))
        {
            if (relative.transform.gameObject.tag == "track")
                Debug.DrawRay(rayOrigin, -transform.up, Color.blue, 0.1f);
        }

        RaycastHit directDown;
        if (Physics.Raycast(new Ray(rayOrigin, Vector3.down), out directDown))
        {
            if (directDown.transform.gameObject.tag == "track")
            {
                Debug.DrawRay(rayOrigin, Vector3.down, Color.green, 0.1f);
                groundNormal = Vector3.Lerp(groundNormal, directDown.normal, 3.0f * Time.deltaTime);
                Debug.DrawRay(rayOrigin, -groundNormal, Color.magenta, 0.1f);
                groundDistance = directDown.distance;
                lastTrackPosition = directDown.point + 3.0f * Vector3.up - 16.0f * transform.forward;
                
                string texture = GetTexture(directDown);
                texture = texture.Replace("(Instance)", "");
            }
        }
    }
    string GetTexture(RaycastHit hit)
    {

        Renderer renderer = hit.collider.GetComponent<Renderer>();
        var meshCollider = hit.collider as MeshCollider;

        if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
            return "";

        // now starts my code
        int triangleIdx = hit.triangleIndex;
        Mesh mesh = hit.collider.gameObject.GetComponent<MeshFilter>().mesh;
        int subMeshesNr = mesh.subMeshCount;
        int materialIdx = -1;

        for (int i = 0; i < subMeshesNr; i++)
        {
            var tr = mesh.GetTriangles(i);
            for (var j = 0; j < tr.Length; j++)
            {
                if (tr[j] == triangleIdx)
                {
                    materialIdx = i;
                    break;
                }
            }
            if (materialIdx != -1)
                break;
        }

        if (materialIdx != -1)
            return renderer.materials[materialIdx].name;
        return "";
    }
    public void Reset(float speedMultiplier = 0, bool resetPosition = false)
    {
        if (resetPosition)
            transform.position = Vector3.zero;
        transform.position += new Vector3(0, 7.5f, 0);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        rigidbody.velocity *= speedMultiplier;
        rigidbody.angularVelocity *= speedMultiplier;
        speed *= speedMultiplier;
        groundDistance = 0;
    }

    public void BackToTrack()
    {
        Reset();
        transform.position = lastTrackPosition;
    }

    public void Spin()
    {
        networkState = new Spinning(this.gameObject);
    }

    public void Freeze()
    {
        networkState = new Frozen(this.gameObject);
    }

    public void UnFreeze()
    {
        networkState = new Stopped(this.gameObject);
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
