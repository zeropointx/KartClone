using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Noclip : MonoBehaviour {
    Transform target = null;
    public float forwardSpeed = 2.0f;
    public float sidewaySpeed = 2.0f;
    Vector3 lockedPos = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float forward = Input.GetAxis("Vertical");

        float left = Input.GetAxis("Horizontal");

        float mouseX = Input.GetAxis("Mouse X");

        float mouseY = Input.GetAxis("Mouse Y");

        if (lockedPos == Vector3.zero)
        {
            transform.position += forward * forwardSpeed * Camera.main.transform.forward;

            transform.position += left * sidewaySpeed * Camera.main.transform.right;
        }
        else
        {
            transform.localPosition = lockedPos;
        }

        if(target == null)
            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0.0f);
        else
        {
            transform.LookAt(target.position + new Vector3(0, 2, 0));
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            if (lockedPos == Vector3.zero)
                lockedPos = transform.localPosition;
            else
                lockedPos = Vector3.zero;
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(target == null)
            {
                var players = GameObject.Find("Gamemode").GetComponent<Gamemode>().GetPlayers();
                //GetComponent<NetworkTransform>().interpolateMovement
                if(players.Count < 1)
                    return;
                target = players[0].gameObject.transform;
                transform.parent = target;
            }

            else
            {
                target = null;
                transform.parent = null;
            }
                
           
        }

	}
}
