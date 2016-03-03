using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class StatusEffectHandler {
    public enum EffectType
    {
        BOOST = 0,
    }
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
	public StatusEffectHandler()
    {
    }
    public void Update()
    {
        foreach(StatusEffect effect in statusEffects)
        {
            effect.Update();
            if (effect.currentStatus == StatusEffect.Status.OFF)
                statusEffects.Remove(effect);
        }
    }
    public void AddStatusEffect(EffectType type)
    {
        switch(type)
        {
            case EffectType.BOOST:
                {
                    AddStatusEffect(new Boost());
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    void AddStatusEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
        effect.Start();
    }

}
