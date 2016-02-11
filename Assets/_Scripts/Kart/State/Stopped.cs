﻿using UnityEngine;
using System.Collections;

public class Stopped : KartState {

    private float stopTimer;
    private float minStop;

	public Stopped(GameObject _kart): base(_kart)
    {
        stopTimer = 0;
        minStop = 0.25f;
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();

        if (kb.pw.hitState == PlayerNetwork.KartHitState.SPINNING)
            return new Spinning(kart);
        if (!kb.UpdateGroundDistance())
            return new Jumping(kart);

        stopTimer += Time.deltaTime;
        kb.speed = 0;
        kb.rigidbody.angularVelocity = Vector3.zero;
        if (stopTimer > minStop && kb.pedal != 0)
            return new Drive(kart);

        kb.UpdateTransform();
        return null;
    }
}
