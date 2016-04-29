using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        if (hasAuthority)
            CmdTestCommand();
	}
	[Command]
    void CmdTestCommand()
    {
        Debug.Log("HEHHEH");
    }
	// Update is called once per frame
	void Update () {
	
	}
}
