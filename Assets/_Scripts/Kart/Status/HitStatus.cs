using UnityEngine;
using System.Collections;
[System.Serializable]
public class HitStatus : StatusEffect
{
    float baseMaxSpeed;
    float boostAmount = 10;
    // niin hyvää hermannia
    public override void ApplyEffect()
    {
        //TODO if maxspeed is higher than normal (like being boosted)
        //It returns as higher (like 100 default, 110 while boosted and boost decreases it but when hit status wears off maxspeed is 110)
        base.ApplyEffect();
        baseMaxSpeed = KB.maxSpeed;
        KB.maxSpeed = 0;

        //  KB.speed = KB.maxSpeed;
        KB.rigidbody.velocity = Vector3.zero;
        KB.childKart.GetComponent<Animator>().SetTrigger("Hit");
        KB.GetComponent<KartInput>().DisableInput();
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        KB.maxSpeed = baseMaxSpeed;
        KB.GetComponent<KartInput>().EnableInput();
    }
    public HitStatus()
    {
        effectTimerDelay = 2.0f;

    }
}
