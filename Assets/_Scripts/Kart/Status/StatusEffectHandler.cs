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
        //Update effects
        foreach(StatusEffect effect in statusEffects)
        {
            effect.Update();
        }

        //Remove everything that has state OFF

        List<StatusEffect> toRemove = new List<StatusEffect>();
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect.currentStatus == StatusEffect.Status.OFF)
            {
                toRemove.Add(effect);
            }
        }
        statusEffects.RemoveAll(x => toRemove.Contains(x));

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
