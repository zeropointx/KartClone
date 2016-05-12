using UnityEngine;
using System.Collections;

public class Wheels : MonoBehaviour {

    KartBehaviour KB;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (KB == null)
           KB = PlayerNetwork.localPlayer.GetComponent<KartBehaviour>();

        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, KB.steeringWheel*45));
            //new Vector3(0, 0, KB.steeringWheel);
	}
}
