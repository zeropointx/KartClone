using UnityEngine;
using System.Collections;

public class grass : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerNetwork.localPlayer != null)
            transform.LookAt(PlayerNetwork.localPlayer.transform.Find("Main Camera").position);
        transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y+180, 0);
	}
}
