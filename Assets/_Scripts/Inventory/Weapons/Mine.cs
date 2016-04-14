using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
    PlayerNetwork PN;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision col)
    {
        ContactPoint cp = col.contacts[0];
        GameObject GG = col.gameObject;

        if (GG.tag == "Player")
        {
            PN = GG.GetComponent<PlayerNetwork>();
            PN.GetStatusEffectHandler().AddStatusEffect(StatusEffectHandler.EffectType.HIT);
            Destroy(gameObject);
        }
    }
}
