using UnityEngine;
using System.Collections;

public class KartInput : MonoBehaviour {

    private float turnSpeed;
    private float acceleration;

    private KartBehaviour kartScript;

	// Use this for initialization
	void Start () {
        turnSpeed = 125;
        acceleration = 7.5f;
        kartScript = transform.transform.GetComponent<KartBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        //gas
        kartScript.speed += Input.GetAxis("Vertical") * Time.deltaTime * acceleration;

        //turn
        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0));
	}
}
