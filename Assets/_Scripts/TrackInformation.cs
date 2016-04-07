using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Information of the track
public class TrackInformation : MonoBehaviour
{
    public List<GameObject> checkPoints = new List<GameObject>();   // # of the checkpoints in the track
    public int lapAmount;                                           // # of the laps to finish the track
    public Sprite miniMapTexture;                                  // Sprite for the minimap of the track

    void Start()
    {
        lapAmount = LobbySettings.lapCount;
    }

    void Update()
    {

    }
   
}
