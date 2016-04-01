using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    private Vector3 middle;
    private GameObject marker;
    private GameObject player;
    private const float coordinateScale = 0.1f;

	// Use this for initialization
	void Start () 
    {
        middle = GameObject.Find("track").transform.position;
        marker = transform.FindChild("MinimapMarker").gameObject;
        player = PlayerNetwork.localPlayer;
    }
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 dist = (PlayerNetwork.localPlayer.transform.position - middle) * coordinateScale;
        marker.GetComponent<RectTransform>().anchoredPosition = new Vector3(dist.x, dist.z, 0);
    }
}
