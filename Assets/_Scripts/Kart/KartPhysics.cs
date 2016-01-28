using UnityEngine;
using System.Collections;

public class KartPhysics : MonoBehaviour
{

    private KartBehaviour kartScript;
    private Vector3 groundNormal = new Vector3(0, 0, 0);
    private float groundDistance = 0;
    private float jumpLimit = 2.0f;

    // Use this for initialization
    void Start()
    {
        kartScript = transform.GetComponent<KartBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (groundDistance >= jumpLimit)
            kartScript.SetState(KartBehaviour.KartState.JUMP);
        */
        switch (kartScript.GetState())
        {
            case KartBehaviour.KartState.FORWARD:
            case KartBehaviour.KartState.REVERSE:
                GroundCollision();
                break;

            case KartBehaviour.KartState.STOPPED:
                break;

            case KartBehaviour.KartState.JUMP:
                kartScript.SetState(KartBehaviour.KartState.STOPPED);
                /*
                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), 1.0f * Time.deltaTime);
                if (groundDistance < jumpLimit)
                    kartScript.SetState(KartBehaviour.KartState.STOPPED);
                */
                break;

            default:
                Debug.Log("Invalid KartState!");
                break;
        }
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
                        if (hit2.distance > 1.5f)
                            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.FromToRotation(transform.up, groundNormal), 2.0f * hit2.distance * Time.deltaTime);
                    }
                    else
                        kartScript.Reset();
                    if (Vector3.Angle(transform.position - hit1.normal, transform.position - hit2.normal) > 5.0f)
                    {
                        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up), 1.0f * Time.deltaTime);
                    }
                }
                else
                    kartScript.Reset(0.25f);
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