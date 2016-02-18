using UnityEngine;
using System.Collections;

public abstract class KartState {

    protected GameObject kart = null;
    protected KartState lastState = null;

    public KartState(GameObject _kart, KartState _lastState = null)
    {
        kart = _kart;
        lastState = _lastState;
    }

    /*
     * Returns the new state or null 
     */
    public abstract KartState UpdateState();

    public abstract void CollisionEnter(Collision collision);

    public void checkFront(Collision collision)
    {
        //Debug.Log(collision.relativeVelocity.magnitude);
        RaycastHit hit;
        Ray ray = new Ray(kart.transform.position + 2.0f * Vector3.up, kart.transform.forward);
        if (Physics.Raycast(ray, out hit, 2.0f))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 2.0f);
            Debug.DrawRay(hit.transform.position, hit.normal, Color.blue, 2.0f);
            float dot = Vector3.Dot(hit.normal, -ray.direction);
            Debug.Log(dot);
            if (dot > 0.5f)
            {
                kart.GetComponent<KartBehaviour>().Reset(0);
            }
        }
    }
}
