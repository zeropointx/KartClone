using UnityEngine;
using System.Collections;

public class die : MonoBehaviour {
    public GameObject[] parts;
    public GameObject explosionPos;
	// Use this for initialization
    float power = 1000.0f;
    float radius = 100.0f;
    float upwardsForce = 0.5f;
	void Start () {
        Explode();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Explode()
    {
        foreach (GameObject ob in parts)
        {
            Rigidbody rb = ob.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos.transform.position, radius, upwardsForce);

        }
    }
}
