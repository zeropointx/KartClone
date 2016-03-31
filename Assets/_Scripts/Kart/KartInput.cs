using UnityEngine;
using System.Collections;

public class KartInput : MonoBehaviour {

    private KartBehaviour kartScript;

	// Use this for initialization
	void Start () {
        kartScript = transform.GetComponent<KartBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        float gas = Input.GetAxis("Vertical");
        gas = Mathf.Clamp(gas, -1, 1);
        kartScript.SetPedal(gas);

        float steer = Input.GetAxis("Horizontal");
        steer = Mathf.Clamp(steer, -1, 1);
        kartScript.SetSteer(steer);
        
        if (Input.GetKeyDown(KeyCode.R))
            kartScript.Reset(0, (Input.GetKey(KeyCode.LeftShift)));

        if (Input.GetKeyDown(KeyCode.T))
            kartScript.BackToTrack();

        if (Input.GetKeyDown(KeyCode.K))
            kartScript.pw.Spin();
	}
}
