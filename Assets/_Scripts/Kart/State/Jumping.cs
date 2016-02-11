using UnityEngine;
using System.Collections;

public class Jumping : KartState {

    private float airTime;

	public Jumping(GameObject _kart): base(_kart)
    {
        airTime = 0;
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();

        if (kb.pw.hitState == PlayerNetwork.KartHitState.SPINNING)
            return new Spinning(kart);

        kb.speed -= kb.engineDeceleration;
        kb.speed = Mathf.Clamp(kb.speed, 0, kb.maxSpeed);

        kb.UpdateGroundDistance();
    
        float z = kart.transform.rotation.eulerAngles.z;
        if (z > 180)
            z = Mathf.Clamp(z, 340, 360);
        else
            z = Mathf.Clamp(z, 0, 20);
        kart.transform.rotation = Quaternion.LerpUnclamped(kart.transform.rotation, Quaternion.Euler(0, kart.transform.rotation.eulerAngles.y, z), Time.deltaTime);
        if (kb.groundDistance < kb.jumpLimit)
            return new Forward(kart);

        if (airTime > 10)
        {
            kb.Reset();
            kart.transform.position = kb.lastTrackPosition;
            return new Stopped(kart);
        }
        airTime += Time.deltaTime;

        kb.UpdateTransform();
        return null;
    }
}
