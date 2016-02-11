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

        RaycastHit relative;
        if (Physics.Raycast(new Ray(kart.transform.position, -kart.transform.up), out relative))
        {
            if (relative.transform.gameObject.tag == "track")
            {
                if (Vector3.Dot(kart.transform.up, kb.groundNormal) < 0.975f)
                    kart.transform.rotation = Quaternion.SlerpUnclamped(kart.transform.rotation, Quaternion.FromToRotation(kart.transform.up, kb.groundNormal), 1.0f * Time.deltaTime);
                Debug.DrawRay(kart.transform.position, -kart.transform.up, Color.blue, 0.1f);
            }
        }

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
}
