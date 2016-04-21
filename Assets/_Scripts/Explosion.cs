using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<ParticleSystem>().Play();
	}
	
	// Update is called once per frame
	void Update () {
	    if(!GetComponent<ParticleSystem>().isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
