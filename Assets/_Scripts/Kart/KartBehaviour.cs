using UnityEngine;
using System.Collections;

public class KartBehaviour : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    private float engineDeceleration;

	// Use this for initialization
	void Start () {
        maxSpeed = 50;
        speed = 0;
        engineDeceleration = 10f;
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
