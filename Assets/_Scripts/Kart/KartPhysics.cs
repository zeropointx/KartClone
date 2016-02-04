using UnityEngine;
using System.Collections;

public class KartPhysics : MonoBehaviour
{

    private KartBehaviour kartScript;
    private Vector3 groundNormal = new Vector3(0, 0, 0);
    private float groundDistance = 0;
    public Vector3 lastTrackPosition = new Vector3(0, 0, 0);
    private float airTime = 0;

    // Use this for initialization
    void Start()
    {
        kartScript = transform.GetComponent<KartBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (kartScript.GetState())
        {
            case KartBehaviour.KartState.FORWARD:
            case KartBehaviour.KartState.REVERSE:
                GroundCollision();
                if (groundDistance >= kartScript.getJumpLimit())
                {
                    kartScript.SetState(KartBehaviour.KartState.JUMP);
                }
                break;

            case KartBehaviour.KartState.STOPPED:
                break;

            case KartBehaviour.KartState.JUMP:
                //transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.FromToRotation(transform.forward, jumpDirection), 1.0f * Time.deltaTime);
                float z = transform.rotation.eulerAngles.z;
                if (z > 180)
                    z = Mathf.Clamp(z, 340, 360);
                else
                    z = Mathf.Clamp(z, 0, 20);
                transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, z), Time.deltaTime);
                if (groundDistance < kartScript.getJumpLimit())
                {
                    kartScript.SetState(KartBehaviour.KartState.FORWARD);
                    airTime = 0;
                }
                if (airTime > 4)
                {
                    kartScript.Reset();
                    kartScript.SetState(KartBehaviour.KartState.STOPPED);
                    transform.position = lastTrackPosition;
                    airTime = 0;
                }
                airTime += Time.deltaTime;
                break;

            default:
                Debug.Log("Invalid KartState!");
                break;
        }

        RaycastHit directDown;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out directDown))
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.green, 0.1f);
            if (directDown.transform.gameObject.tag == "track")
            {
                groundNormal = directDown.normal;
                groundDistance = directDown.distance;
                if (kartScript.GetState() != KartBehaviour.KartState.JUMP)
                    lastTrackPosition = directDown.point + 3.0f * Vector3.up - 16.0f * transform.forward;
            }
        }
        else
        {
            groundDistance = float.MaxValue;
            kartScript.SetState(KartBehaviour.KartState.JUMP);
        }
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
        RaycastHit relative;
        if (Physics.Raycast(new Ray(transform.position, -transform.up), out relative))
        {
            if (relative.transform.gameObject.tag == "track")
            {
                if (Vector3.Angle(transform.position - groundNormal, transform.position - relative.normal) > 3.5f)
                    transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), 0.5f * Time.deltaTime);
                Debug.DrawRay(transform.position, -transform.up, Color.blue, 0.1f);
            }
        }
    }

    //public
    public float GetGroundDistance()
    {
        return groundDistance;
    }

    public Vector3 GetGroundNormal()
    {
        return groundNormal;
    }
}