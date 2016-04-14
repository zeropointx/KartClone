using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mine : NetworkBehaviour {
    PlayerNetwork PN;
	// Use this for initialization
	void Start () {
        if (!isServer)
            this.enabled = false;
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
            PN.RpcApplyStatusEffectClient(StatusEffectHandler.EffectType.HIT);
            PN.GetStatusEffectHandler().AddStatusEffect(StatusEffectHandler.EffectType.HIT);
            Destroy(gameObject);
        }
    }
}
