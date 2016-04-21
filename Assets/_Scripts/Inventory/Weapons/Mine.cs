using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mine : NetworkBehaviour {
    public float currentLightIntensity = 0.0f;
    float maxLightIntensity = 8.0f;
    float minLightIntensity = 0.0f;
    float incrementMultiplier = 4.0f;
    bool isLightRaising = true;
    public GameObject explosionPrefab;
    PlayerNetwork PN;
	// Use this for initialization
	void Start () {
        if (!isServer)
            this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(isLightRaising)
        {
            currentLightIntensity += Time.deltaTime * incrementMultiplier;
            if (currentLightIntensity > maxLightIntensity)
                isLightRaising = false;
        }
        else
        {
            currentLightIntensity -= Time.deltaTime * incrementMultiplier;
            if (currentLightIntensity <= minLightIntensity)
                isLightRaising = true;
        }
        transform.Find("Point light").GetComponent<Light>().intensity = currentLightIntensity;
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
            GameObject.Instantiate(explosionPrefab, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }
}
