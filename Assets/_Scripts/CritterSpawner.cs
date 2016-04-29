using UnityEngine;
using System.Collections;

public class CritterSpawner : MonoBehaviour {

    public GameObject critter;
    public float spawnTimer;
    public float time;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        time += Time.deltaTime;
        if(time >= spawnTimer)
        {
            time = 0;
            Instantiate(critter, transform.position - new Vector3(20, 0, 0), critter.transform.rotation);
            
        }
	}
}
