﻿using UnityEngine;
using System.Collections;

public class Ragdoll : KartState
{
    float timer;
    float maxTime;

    public Ragdoll(GameObject _kart, KartState _lastState, float time) : base(_kart, _lastState)
    {
        timer = 0;
        maxTime = time;
        name = "ragdoll";
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();
        timer += Time.deltaTime;
        if (timer > maxTime)
            return lastState;
        kb.speed -= kb.engineDeceleration * Time.deltaTime;
        kb.speed = Mathf.Clamp(kb.speed, 0, kb.maxSpeed);
        kb.UpdateTransform(0);
        return null;
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
