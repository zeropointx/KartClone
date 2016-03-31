using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boost : StatusEffect
{
    float baseMaxSpeed;
    float boostAmount = 10;
    GameObject FLAMES;
    Weapon weaponScript;
    // niin hyvää hermannia
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        baseMaxSpeed = KB.maxSpeed;
        KB.maxSpeed += boostAmount;

        
        weaponScript = Kart.GetComponent<Weapon>();
        GameObject FIRE = weaponScript.speedBoost;
        FLAMES = GameObject.Instantiate(FIRE, Kart.transform.position, Quaternion.identity) as GameObject;

        
      //  KB.speed = KB.maxSpeed;
        KB.rigidbody.velocity = KB.transform.forward * KB.maxSpeed;
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        KB.maxSpeed -= boostAmount;
        GameObject.Destroy(FLAMES);
        if(KB.speed > KB.maxSpeed)
        {
            KB.rigidbody.velocity = KB.transform.forward * KB.maxSpeed;
        }
    }
    public Boost()
    {
        effectTimerDelay = 2.0f;

    }
    public override void Update()
    {
        base.Update();
        FLAMES.transform.localRotation = Kart.transform.localRotation;
        FLAMES.transform.localPosition = Kart.transform.localPosition - Kart.transform.forward * 2;
    }
}
