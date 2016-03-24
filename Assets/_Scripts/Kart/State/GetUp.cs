using UnityEngine;
using System.Collections;

public class GetUp : KartState
{
    float timer;
    float tiltSpeed;

    public GetUp(GameObject _kart, KartState _lastState) : base(_kart, _lastState)
    {
        name = "get up";
        timer = 0;
        tiltSpeed = 1.0f;
    }

    public override KartState UpdateState()
    {
        /*
        if (timer == 0)
        {
            timer += Time.deltaTime;
            return new Ragdoll(kart, this, 0.5f);
        }

        KartBehaviour kb = kart.GetComponent<KartBehaviour>();
        kb.UpdateGroundDistance();

        //kart.transform.rotation = Quaternion.SlerpUnclamped(kart.transform.rotation, Quaternion.FromToRotation(kart.transform.up, kb.groundNormal), tiltSpeed * Time.deltaTime);
        Quaternion rotation = Quaternion.FromToRotation(kart.transform.up, kb.groundNormal);
        kb.rigidbody.angularVelocity = rotation.eulerAngles * tiltSpeed * Time.deltaTime;
        if (Vector3.Dot(kart.transform.up, kb.groundNormal) > kb.tiltLimit)
            return lastState;

        timer += Time.deltaTime;
        */
        return lastState;
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
