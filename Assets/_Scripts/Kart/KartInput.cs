using UnityEngine;
using System.Collections;

public class carInput : MonoBehaviour {

    private float turnSpeed;
    private float acceleration;

    private CarBehaviour carScript;

	// Use this for initialization
	void Start () {
        turnSpeed = 125;
        acceleration = 7.5f;
        carScript = transform.transform.GetComponent<CarBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        //gas
        carScript.speed += Input.GetAxis("Vertical") * Time.deltaTime * acceleration;

        //turn
        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0));
	}
}
