using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Hit player!");
            other.transform.root.GetComponent<Rigidbody>().AddExplosionForce(100000.0f, transform.position, 5.0f);
            Destroy(gameObject);
        }
    }
}
