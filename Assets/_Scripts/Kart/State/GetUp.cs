using UnityEngine;
using System.Collections;

public class GetUp : KartState
{

    public GetUp(GameObject _kart, KartState _lastState) : base(_kart, _lastState)
    {
        name = "get up";
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();
        kb.UpdateGroundDistance();

        kart.transform.rotation = Quaternion.SlerpUnclamped(kart.transform.rotation, Quaternion.FromToRotation(kart.transform.up, kb.groundNormal), 1.0f * Time.deltaTime);
        if (Vector3.Dot(kart.transform.up, kb.groundNormal) > kb.tiltLimit)
            return lastState;

        float dir = (kb.speed < 0) ? 1.0f : -1.0f;
        kb.speed += dir *  kb.engineDeceleration * Time.deltaTime;
        if (dir > 0)
            kb.speed = Mathf.Clamp(kb.speed, 0, kb.maxSpeed);
        else
            kb.speed = Mathf.Clamp(kb.speed, kb.maxReverse, 0);
        kb.UpdateTransform();
        return null;
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
