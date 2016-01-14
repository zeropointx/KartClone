using UnityEngine;
using System.Collections;

public class KartInput : MonoBehaviour {

    private float turnSpeed;
    private float maxTurn;
    private float acceleration;
    private float brakeForce;
    private KartBehaviour kartScript;

	// Use this for initialization
	void Start () {
        turnSpeed = 4.5f;
        acceleration = 25;
        brakeForce = 45;
        kartScript = transform.transform.GetComponent<KartBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        //gas
        float pedal = Input.GetAxis("Vertical");
        if (pedal < 0)
            kartScript.speed += pedal * Time.deltaTime * brakeForce;
        else
            kartScript.speed += pedal * Time.deltaTime * acceleration;

        //turn
        float rotation = 0;
        if (Mathf.Abs(kartScript.speed) > 0)
        {
            rotation = Input.GetAxis("Horizontal") * turnSpeed * Mathf.Abs(kartScript.speed)* Time.deltaTime;
            transform.Rotate(new Vector3(0, rotation, 0));
        }
	}
}
