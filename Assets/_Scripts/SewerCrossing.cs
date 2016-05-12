using UnityEngine;
using System.Collections;

public class SewerCrossing : MonoBehaviour {
    public GameObject north;
    public GameObject west;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GameObject player = PlayerNetwork.localPlayer;
        if (player == null)
            return;
        int checkpoint = player.GetComponent<Placement>().currentCheckPointIndex;
        if(checkpoint <=3)
        {
            north.SetActive(true);
            west.SetActive(false);
        }
        else
        {
            north.SetActive(false);
            west.SetActive(true);
        }
	}
}
