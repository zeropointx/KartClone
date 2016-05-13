using UnityEngine;
using System.Collections;
[System.Serializable]
public class HitStatus : StatusEffect
{
    float originalMaxSpeed;
    float boostAmount = 10;
    // niin hyvää hermannia
    public override void ApplyEffect()
    {
        //TODO if maxspeed is higher than normal (like being boosted)
        //It returns as higher (like 100 default, 110 while boosted and boost decreases it but when hit status wears off maxspeed is 110)
        base.ApplyEffect();
        originalMaxSpeed = -KB.defaultMaxSpeed + 0.001f;
        KB.maxSpeedChange = originalMaxSpeed;
        KB.maxSpeed = KB.defaultMaxSpeed + KB.maxSpeedChange;

        //  KB.speed = KB.maxSpeed;
        KB.GetComponent<Rigidbody>().velocity = Vector3.zero;
        KB.transform.Find("Kart").GetComponent<Animator>().SetTrigger("Hit");
        KB.GetComponent<KartInput>().DisableInput();
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        KB.maxSpeedChange = KB.defaultMaxSpeed;
        KB.maxSpeed = KB.defaultMaxSpeed;
        KB.GetComponent<KartInput>().EnableInput();
    }
    public HitStatus()
    {
        effectTimerDelay = 1.5f;

    }
}
