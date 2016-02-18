using UnityEngine;
using System.Collections;

public class Jumping : KartState {

    private float airTime;
    private float angleTresshold;

	public Jumping(GameObject _kart): base(_kart)
    {
        airTime = 0;
        angleTresshold = 10.0f;
        name = "jumping";
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();

        kb.speed -= kb.engineDeceleration * 2.0f;
        kb.speed = Mathf.Clamp(kb.speed, 0, kb.maxSpeed);

        kb.UpdateGroundDistance();
        
        float z = kart.transform.rotation.eulerAngles.z;
        if (z > 180)
            z = Mathf.Clamp(z, 360 - angleTresshold, 360);
        else
            z = Mathf.Clamp(z, 0, angleTresshold);
        kart.transform.rotation = Quaternion.LerpUnclamped(kart.transform.rotation, Quaternion.Euler(0, kart.transform.rotation.eulerAngles.y, z), Time.deltaTime);
        
        if (kb.groundDistance < kb.jumpLimit)
            return new Drive(kart);
        
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

    public override void CollisionEnter(Collision collision)
    {
        checkFront(collision);
    }
}
