using UnityEngine;
using System.Collections;

public class KartInput : MonoBehaviour {

    private KartBehaviour kartScript;

	// Use this for initialization
	void Start () {
        kartScript = transform.GetComponent<KartBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        kartScript.Accelerate(Input.GetAxis("Vertical"));
        kartScript.Steer(Input.GetAxis("Horizontal"));
	}
}
