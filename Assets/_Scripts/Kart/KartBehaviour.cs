using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour {

    public float speed;
    private float maxSpeed;
    private float engineDeceleration;

	// Use this for initialization
	void Start () {
        maxSpeed = 12.5f;
        speed = 0;
        engineDeceleration = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        //gas
        speed -= engineDeceleration * Time.deltaTime;
        speed = Mathf.Min(speed, maxSpeed);
        speed = Mathf.Max(speed, 0);

        //move
        transform.position += speed * Time.deltaTime * transform.forward;
	}
}
