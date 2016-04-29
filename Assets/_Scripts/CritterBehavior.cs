using UnityEngine;
using System.Collections;

public class CritterBehavior : MonoBehaviour {

    GameObject Destination;
    Vector3 RandomVec3;
    public float speed;

	// Use this for initialization
	void Start () 
    {
        Destination = GameObject.Find("RatDestinationPipe");

	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Destination.transform.position, step);

        //Vector3 targetDir = Destination.transform.position - transform.position;
        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        //transform.rotation = Quaternion.LookRotation(newDir);

	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == Destination)
            Destroy(gameObject);

        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
