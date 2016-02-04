using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Placement : MonoBehaviour
{
    public int currentLap = 0;
    public int currentCheckPointIndex = 0;

    // These are fetched from the track information
    int checkpointAmount;
    int lapAmount;

    GameObject track;
    TrackInformation trackInformation;

    void Start()
    {
        track = GameObject.FindGameObjectsWithTag("track")[0].transform.root.gameObject;
        trackInformation = track.GetComponent<TrackInformation>();       
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        GameObject GG = col.gameObject;
        if (GG.tag == "checkPoint")
        {
            for (int i = 0; i < trackInformation.checkPoints.Count; i++)
            {
                if (GG == trackInformation.checkPoints[i])
                {
                    if (GG == trackInformation.checkPoints[currentCheckPointIndex + 1])
                    {
                        currentCheckPointIndex++;
                        if (currentCheckPointIndex == (trackInformation.checkPoints.Count - 1))
                        {
                            Debug.Log("Doge");
                            currentLap++;
                            currentCheckPointIndex = 0;

                            if (currentLap == trackInformation.lapAmount)
                            {
                                //Wonnered
                            }
                        }
                    }

                }
            }
        }
    }
}
