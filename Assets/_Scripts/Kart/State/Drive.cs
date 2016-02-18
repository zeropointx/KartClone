using UnityEngine;
using System.Collections;

public class Drive : KartState {

    bool onReverse;
    private float angleTresshold;

	public Drive(GameObject _kart): base(_kart)
    {
        onReverse = kart.GetComponent<KartBehaviour>().pedal < 0;
        angleTresshold = 5.0f;
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();
        float steer = kb.steeringWheel;

        //physics
        if (!kb.UpdateGroundDistance() || kb.groundDistance >= kb.jumpLimit)
            return new Jumping(kart);

        if (Vector3.Dot(kart.transform.up, kb.groundNormal) < 0.95f)
            return new GetUp(kart, this);

        //pedal
        if (onReverse)
        {
            kb.speed += ((kb.pedal > 0) ? kb.brakeForce : kb.acceleration) * kb.pedal + kb.engineDeceleration;
            kb.speed = Mathf.Clamp(kb.speed, kb.maxReverse, 0);
        }
        else
        {
            kb.speed += ((kb.pedal > 0) ? kb.acceleration : kb.brakeForce) * kb.pedal - kb.engineDeceleration;
            kb.speed = Mathf.Clamp(kb.speed, 0, kb.maxSpeed);
        }

        if (kb.speed == 0)
            return new Stopped(kart);
        kb.UpdateTransform(onReverse ? -1 : 1);
        return null;
    }

    public override void CollisionEnter(Collision collision)
    {
        checkFront(collision);
    }
}
