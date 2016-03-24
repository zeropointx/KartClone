using UnityEngine;
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
        timer += Time.deltaTime;
        if (timer > maxTime)
            return lastState;
        return null;
    }

    public override void UpdatePhysicsState()
    {
        
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
