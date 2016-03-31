using UnityEngine;
using System.Collections;

public class Drive : KartState {

    bool onReverse;
    private float angleTresshold;

	public Drive(GameObject _kart): base(_kart)
    {
        onReverse = kart.GetComponent<KartBehaviour>().pedal < 0;
        angleTresshold = 5.0f;
        name = "drive";
    }

    public override KartState UpdateState()
    {   
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

        return null;
    }

    public override void UpdatePhysicsState()
    {
        kb.Stabilize();
        if (kb.groundDistance < kb.jumpLimit)
        {
            Vector3 direction = kb.transform.forward;
            direction -= kb.groundNormal;
            float controlMultiplier = onReverse ? -1 : (1.0f - 0.5f * (kb.speed / kb.maxSpeed));
            kb.transform.Rotate(new Vector3(0, controlMultiplier * kb.turnSpeed * kb.steeringWheel * Time.fixedDeltaTime, 0));

            direction = Vector3.ProjectOnPlane(direction, kb.groundNormal).normalized;
            kb.rigidbody.velocity = new Vector3(direction.x * kb.speed, kb.rigidbody.velocity.y, direction.z * kb.speed);
        }
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
