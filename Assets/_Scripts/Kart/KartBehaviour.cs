using UnityEngine;
using System.Reflection;
using System.Collections;

public class KartBehaviour : MonoBehaviour
{
    //controls
    public float steeringWheel = 0, pedal = 0, speed = 0;

    //stats
    public float currentTextureSpeedModifier, defaultMaxSpeed, maxSpeedChange, maxSpeed, maxReverse, turnSpeed, acceleration, brakeForce, engineDeceleration, spinSpeed, stabilizeTorqueForce;
    
    public bool drifting = false;
    public string lastTextureName = "";
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
    public KartInformation kartInformation;

    //Camera
    public Vector3 defaultCameraPos;
    public Vector3 targetCameraPos;
    public enum CameraState
    {
        DRIFTING_LEFT, MIDDLE,DRIFTING_RIGHT
    }
    public CameraState currentCameraState = CameraState.MIDDLE;
    float stabilizeTimer = 0.0f;
    float stabilizedelay = 1.0f;
    // Use this for initialization
    void Start()
    {
        defaultCameraPos = transform.Find("Main Camera").localPosition;
        targetCameraPos = defaultCameraPos;
        currentTextureSpeedModifier = 1.0f;
        childKart = transform.Find("Kart");
        originalRotation = childKart.transform.localRotation;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        kartInformation = gameObject.transform.Find("Kart").GetComponent<KartInformation>();

        //stats
        defaultMaxSpeed = 65;
        maxSpeed = defaultMaxSpeed;
        maxSpeedChange = 0;
        maxReverse = 15;
        turnSpeed = 100;
        acceleration = 0.25f;
        brakeForce = 0.95f;
        engineDeceleration = 0.15f;
        spinSpeed = 250;
        stabilizeTorqueForce = 2000.0f;

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
        Vector3 cameraPos = transform.Find("Main Camera").localPosition;
        if(!GetComponent<Placement>().gameFinished)
        transform.Find("Main Camera").localPosition = Vector3.Lerp(cameraPos, targetCameraPos, Time.deltaTime);
        if(transform.position.y <= -200)
        {
            Reset(0, true);
        }

        turnWheels();
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
        if(transform.eulerAngles.z  > 160 && transform.eulerAngles.z < 200)
        {
            stabilizeTimer += Time.fixedDeltaTime;
            if(stabilizeTimer >= stabilizedelay)
            Reset();
        }
        else
        {
            stabilizeTimer = 0.0f;
        }
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
                
                if(GetComponent<PlayerNetwork>().GetStatusEffectHandler().HasEffect(StatusEffectHandler.EffectType.BOOST))
                {
                    rigidbody.AddForce(groundNormal * -1 * (groundDistance / 2) * 10);
                }

                //Get texture right below player
                string texture = GetTexture(directDown);
                //Parse that string
                texture = texture.Replace("(Instance)", "").Replace(" ","");
                //
                lastTextureName = texture;
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
        stabilizeTimer = 0.0f;
    }

    public void BackToTrack()
    {
        Reset();
        transform.position = lastTrackPosition;
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
    public void SetCameraState(CameraState state)
    {

        if(currentCameraState == state)
            return;
        currentCameraState = state;
        switch(state)
        {
            case CameraState.MIDDLE:
                {
                    targetCameraPos = defaultCameraPos;
                    break;
                }
            case CameraState.DRIFTING_LEFT:
                {
                    targetCameraPos = (Vector3.right * 2.0f) + defaultCameraPos;
                    break;
                }
            case CameraState.DRIFTING_RIGHT:
                {
                    targetCameraPos = (Vector3.right * -2.0f) + defaultCameraPos;
                
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void turnWheels()
    {
        for (int i = 0; kartInformation.frontWheels.Length > i; i++ )
            kartInformation.frontWheels[i].transform.localRotation = Quaternion.Euler(new Vector3(270, 90 + steeringWheel * 35, 0 ));
    }
}
