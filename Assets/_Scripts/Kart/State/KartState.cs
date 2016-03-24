using UnityEngine;
using System.Collections;

public abstract class KartState {

    protected GameObject kart = null;
    protected KartState lastState = null;
    protected string name;
    protected KartBehaviour kb;

    public KartState(GameObject _kart, KartState _lastState = null)
    {
        kart = _kart;
        lastState = _lastState;
        name = "nameless state";
        kb = kart.GetComponent<KartBehaviour>();
    }

    /*
     * Returns the new state or null 
     */
    public abstract KartState UpdateState();

    /*
     * Returns the new state or null. Modify rigidbody only in here.
     */
    public abstract void UpdatePhysicsState();

    public abstract void CollisionEnter(Collision collision);

    public string GetName()
    {
        return name;
    }
}
