using UnityEngine;
using System.Collections;
[System.Serializable]
public class StatusEffect {

    public GameObject Kart;
    public KartBehaviour KB;

    protected float effectTimerCurrent;
    protected float effectTimerDelay;
    public enum Status
    {
        ON,
        OFF,
    }
    public Status currentStatus = Status.OFF;
    public StatusEffect()
    {
    }
	public void Start () 
    {
        KB = Kart.GetComponent<KartBehaviour>();
        effectTimerCurrent = 0.0f;
        ApplyEffect();
	}
    public virtual void ApplyEffect() 
    {
        currentStatus = Status.ON;
    }
    public virtual void RemoveEffect() 
    {
        currentStatus = Status.OFF;
    }
    public virtual void Update() 
    {
        effectTimerCurrent += Time.deltaTime;

        if(effectTimerCurrent >= effectTimerDelay)
        {
            RemoveEffect();
        }
	}
}
