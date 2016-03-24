﻿using UnityEngine;
using System.Collections;

public class Stopped : KartState {

    private float stopTimer;
    private float minStop;

	public Stopped(GameObject _kart): base(_kart)
    {
        stopTimer = 0;
        minStop = 0.25f;
        name = "stopped";
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();

        if (!kb.UpdateGroundDistance())
            return new Ragdoll(kart, this, 1);
        stopTimer += Time.deltaTime;
        kb.speed = 0;
        if (stopTimer > minStop && kb.pedal != 0)
            return new Drive(kart);
        return null;
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
