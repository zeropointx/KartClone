using UnityEngine;
using System.Collections;

public class Stopped : KartState {

    private float stopTimer;

	public Stopped(GameObject _kart): base(_kart)
    {
        stopTimer = 0;
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();
        if (!kb.UpdateGroundDistance())
            return new Jumping(kart);

        stopTimer += Time.deltaTime;
        kb.speed = 0;
        kb.rigidbody.angularVelocity = Vector3.zero;
        if (stopTimer > 0.25f)
        {
            if (kb.pedal != 0)
            {
                if (kb.pedal > 0)
                    return new Forward(kart);
                else
                    return new Reverse(kart);
            }
        }

        kb.UpdateTransform();
        return this;
    }
}
