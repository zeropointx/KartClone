﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
[System.Serializable]
public class StatusEffectHandler {
    public enum EffectType
    {
        BOOST = 0,
        HIT = 1,
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
        StatusEffect effect = GetEffectFromEnum(type);
        if (effect == null)
            return;
        AddStatusEffect(effect);
    }
    public StatusEffect GetEffectFromEnum(EffectType type)
    {
        switch (type)
        {
            case EffectType.BOOST:
                {
                    return new Boost();
                }
            case EffectType.HIT:
                {
                    return new HitStatus();
                }
            default:
                {
                    return null;
                }
        }
    }
    void AddStatusEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
        effect.Start();
    }
    public bool HasEffect(EffectType type)
    {
        foreach(StatusEffect effect in statusEffects)
        {
            Type t = effect.GetType();
            Type TT = GetEffectFromEnum(type).GetType();
           if(t.Equals(TT))
           {
               Debug.Log("EQUALS!");
               return true;
           }
           else
           {
               Debug.Log("DOESNT EQUAL!");
               
           }

        }
        return false;
       
    }
}
