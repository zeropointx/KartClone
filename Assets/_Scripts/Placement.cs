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
    bool gameFinished = false;

    GameObject camera;

    GameObject track;
    TrackInformation trackInformation;
    KartBehaviour KB;

    void Start()
    {
        track = GameObject.FindGameObjectsWithTag("track")[0].transform.root.gameObject;
        trackInformation = track.GetComponent<TrackInformation>();
        camera = transform.Find("Main Camera").gameObject;
        KB = gameObject.GetComponent<KartBehaviour>();
    }

    void Update()
    {
        if (gameFinished)
            Finish();
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
                            currentLap++;
                            currentCheckPointIndex = 0;

                            if (currentLap == trackInformation.lapAmount)
                            {
                                // INSERT KART LOCKSTATE HERE
                                KB.Freeze();

                                // Finished the final lap
                                Debug.Log("Wonnered");
                                gameFinished = true;
                            }
                        }
                    }

                }
            }
        }
    }

    void Finish()
    {
        camera.transform.LookAt(gameObject.transform);
        camera.transform.Translate(Vector3.right * Time.deltaTime);
        //camera.transform.Rotate(new Vector3(0, 1, 0));
    }
}
