using UnityEngine;
using System.Collections;

public class Forward : KartState {

	public Forward(GameObject _kart): base(_kart)
    {

    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();
        float speedChange = 0;
        float steer = kb.steeringWheel;

        if (kb.pw.hitState == PlayerNetwork.KartHitState.SPINNING)
            return new Spinning(kart);

        //physics
        kb.GroundCollision();
        if (!kb.UpdateGroundDistance())
            return new Jumping(kart);
        
        if (kb.groundDistance >= kb.jumpLimit)
            return new Jumping(kart);

        if (kb.pedal != 0)
            speedChange = (kb.pedal > 0) ? kb.acceleration : kb.brakeForce;
        kb.speed -= kb.engineDeceleration;
        kb.speed += speedChange * kb.pedal;
        kb.speed = Mathf.Clamp(kb.speed, 0, kb.maxSpeed);
        if (kb.speed == 0)
            return new Stopped(kart);

        kb.UpdateTransform();
        return null;
    }
}
