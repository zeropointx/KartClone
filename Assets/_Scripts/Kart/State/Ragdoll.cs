using UnityEngine;
using System.Collections;

public class Ragdoll : KartState
{
    float timer;
    float maxTime;

    public Ragdoll(GameObject _kart, KartState _lastState) : base(_kart, _lastState)
    {
        timer = 0;
        maxTime = 3.0f;
    }

    public override KartState UpdateState()
    {
        timer += Time.deltaTime;
        if (timer > maxTime)
            return lastState;
        return null;
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
