using UnityEngine;
using System.Collections;
[System.Serializable]
public class Boost : StatusEffect
{
    float baseMaxSpeed;
    float boostAmount = 10;
    // niin hyvää hermannia
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        baseMaxSpeed = KB.maxSpeed;

        KB.maxSpeed += boostAmount;
        KB.speed = baseMaxSpeed;
       
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        KB.maxSpeed -= boostAmount;
        
        if(KB.speed > KB.maxSpeed)
        {
            KB.speed = KB.maxSpeed;
        }
    }
    public Boost()
    {
        effectTimerDelay = 2.0f;

    }
}
