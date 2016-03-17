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
    public float tiltLimit;

    //common
    public float jumpLimit;
    public float speedScale;
    public KartState state = null;
    public GameObject mainCamera = null;
    public Rigidbody rigidbody = null;
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
        //stats
        maxSpeed = 65;
        maxReverse = -15;
        turnSpeed = 100;
        acceleration = 0.25f;
        brakeForce = 0.95f;
        engineDeceleration = 0.15f;
        spinSpeed = 250;
        tiltLimit = 0.9f;

        //common
        jumpLimit = 1.75f;
        speedScale = 50.0f;
        state = new Stopped(this.gameObject);
        mainCamera = transform.FindChild("Main Camera").gameObject;
        pw = gameObject.GetComponent<PlayerNetwork>();
        oldPosition = transform.position;
        groundNormal = Vector3.up;

        childKart = transform.Find("Kart");
        originalRotation = childKart.transform.localRotation;

        CopyComponent(childKart.GetComponent<Rigidbody>());
        CopyComponent(childKart.GetComponent<BoxCollider>());
        Destroy(childKart.GetComponent<Rigidbody>());
        Destroy(childKart.GetComponent<BoxCollider>());
        BoxCollider bc = transform.GetComponent<BoxCollider>();
        float x = bc.size.x;
        float y = bc.size.y;
        float z = bc.size.z;
        bc.size = new Vector3(z, y, x) * 2.0f;

        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.centerOfMass = new Vector3(0, rigidbody.transform.GetComponent<BoxCollider>().size.y * -0.25f, 0.0f);
        rigidbody.isKinematic = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
            pw.Spin();

        if (networkState != null)
        {
            state = networkState;
            networkState = null;
        }
        KartState tempState = state.UpdateState();
        if (tempState != null)
            state = tempState;

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
        // TODO find better fix
        childKart.localPosition = new Vector3(0, 0, 0);
        childKart.localEulerAngles = new Vector3(0, 90, 0);
        mainCamera.transform.LookAt(transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        state.CollisionEnter(collision);
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red, 1.0f);
        }
    }

    public bool UpdateGroundDistance()
    {
        RaycastHit relative;
        if (Physics.Raycast(new Ray(transform.position, -transform.up), out relative))
        {
            if (relative.transform.gameObject.tag == "track")
                Debug.DrawRay(transform.position, -transform.up, Color.blue, 0.1f);
        }

        RaycastHit directDown;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out directDown))
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.green, 0.1f);
            if (directDown.transform.gameObject.tag == "track")
            {
                string texture = GetTexture(directDown);
                texture = texture.Replace("(Instance)", "");
                //Debug.Log(texture);
                groundNormal = Vector3.Lerp(groundNormal, directDown.normal, 3.0f * Time.deltaTime);
                Debug.DrawRay(transform.position, -groundNormal, Color.magenta, 0.1f);
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
    public void Reset(float speedMultiplier = 0)
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        rigidbody.velocity *= speedMultiplier;
        rigidbody.angularVelocity *= speedMultiplier;
        speed *= speedMultiplier;
    }

    private void CopyComponent(Component original)
    {
        System.Type type = original.GetType();
        Component copy = gameObject.AddComponent(type);
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                pinfo.SetValue(copy, pinfo.GetValue(original, null), null);
            }
        }
    }

    public void Spin()
    {
        networkState = new Spinning(this.gameObject);
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
