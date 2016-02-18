using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatusEffectHandler {

    public StatusEffect currentEffect;

	public StatusEffectHandler()
    {
        currentEffect = new Boost();
    }


}
