using UnityEngine;
using System.Collections;

public class HarpoonHead : MonoBehaviour {
    float speed = 5.0f;
    public GameObject harpoon = null;
    bool initialized = false;
    Transform hitObject = null;
    Vector3 hitPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
