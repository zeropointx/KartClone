using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HarpoonHead : NetworkBehaviour {
    float speed = 100.0f;
    public GameObject harpoon = null;
    bool initialized = false;
    public Transform hitObject = null;
    Vector3 hitPos;
	// Use this for initialization
	void Start () {
        if(isServer)
        Destroy(gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
            return;
        if(hitObject == null)
        transform.position += transform.forward * Time.deltaTime * speed;

        if (harpoon == null)
        {
            Destroy(gameObject);
            return;
        }

        //Set rope location
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, harpoon.transform.position);


        //Calculate tiling right (so texture tiling doesn't loose or gain width)
        Material m = GetComponent<LineRenderer>().material;
        float distance = Vector3.Distance(transform.position,harpoon.transform.position);
        m.mainTextureScale = new Vector2(distance / 1, 1.0f);
        GetComponent<LineRenderer>().material = m;
	}
    void OnTriggerEnter(Collider other)
    {
        hitObject = other.transform;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            hitPos = hit.point;
        }
    }
}
