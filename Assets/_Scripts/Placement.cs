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

    public List<GameObject> checkPoints = new List<GameObject>();
    public GameObject track;
    TrackInformation trackInformation;

    void Start()
    {
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
            for (int i = 0; i < checkPoints.Count; i++)
            {
                if (GG == checkPoints[i])
                {
                    if (GG == checkPoints[currentCheckPointIndex + 1])
                    {
                        currentCheckPointIndex++;
                        if (currentCheckPointIndex == (checkPoints.Count - 1))
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
