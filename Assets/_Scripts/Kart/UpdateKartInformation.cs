using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// Class used for changing the model of the kart
public class UpdateKartInformation : NetworkBehaviour {
    public KartBehaviour KB;   // Local KartBehaviour
    GameObject Kart;    // 
    public int kartid = -1;
    public GameObject ShoppingKart, BananaKart, roadwarrior, carriage; //Kart models
    void Awake ()
    {

    }
    public void Start()
    {
        KB = GetComponent<KartBehaviour>();
        Update();
    }
    public void Update()
    {
        if (kartid != -1)
            return;
        var component = GetComponent<NetworkIdentity>();
        var clientAuthority = component.clientAuthorityOwner;
        if (clientAuthority != null && isServer)
        {
            MyNetworkLobbyPlayer.GetLobbyPlayer(clientAuthority).kartNetId = GetComponent<NetworkIdentity>().netId.Value;
            GetComponent<UpdateKartInformation>().KB = GetComponent<KartBehaviour>();
        }
        var lobbyplayers = MyNetworkLobbyPlayer.GetLobbyPlayers();

        foreach (MyNetworkLobbyPlayer lobbyplayer in lobbyplayers)
        {
            if (lobbyplayer.kartNetId == GetComponent<NetworkIdentity>().netId.Value)
            {
                kartid = lobbyplayer.kartId;
            }
        }



        GetComponent<UpdateKartInformation>().ChangeKart(kartid);
    }
    public void ChangeKart(int kartid)
    {

        // Checks stored kart information for ID of the chosen kart
        if (kartid == 0)
        {
            updatePlayerComponents(ShoppingKart, 65, 0.25f, 100);
        }
        if (kartid == 1)
        {
            updatePlayerComponents(BananaKart, 65, 0.25f, 100);
        }
        if (kartid == 2)
        {
            updatePlayerComponents(roadwarrior, 65, 0.25f, 100);
        }
        if (kartid == 3)
        {
            updatePlayerComponents(carriage, 65, 0.25f, 100);
        }
    }
    public void updatePlayerComponents(GameObject Model, float MaxSpeed, float Acceleration, float TurnSpeed)
    {
        // Change the player model to the new kart model loaded before
        GameObject newKart = Instantiate(Model, transform.position, Quaternion.identity) as GameObject;
        newKart.name = "Kart";
        newKart.transform.parent = gameObject.transform;    // Make it child of the player


        // Give it MaximumSpeed, Acceleration & Turn speed that were given before
        KB.maxSpeed = MaxSpeed;
        KB.acceleration = Acceleration;
        KB.turnSpeed = TurnSpeed;
    }
}
