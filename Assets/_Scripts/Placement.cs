using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Placement : MonoBehaviour
{
    public int currentLap = 0;              // Local players current lap
    public int currentCheckPointIndex = 0;  // Local players current checkpoint
    int finalPlacement = 0;
    public GameObject miniMapObject;

    // These are fetched from the track information
    int checkpointAmount;
    int lapAmount;
    public bool gameFinished = false;

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
        // If the player reaches the finish line during the last lap the bool will be true
        if (gameFinished)
            Finish();
    }

    void OnTriggerEnter(Collider col)
    {

        GameObject GG = col.gameObject;
        // Check if player hits a checkpoint
        if (GG.tag == "checkPoint")
        {
            // Loop the checkpoints that the track has
            for (int i = 0; i < trackInformation.checkPoints.Count; i++)
            {
                if (GG == trackInformation.checkPoints[i])
                {
                    // If the checkpoint is the one next in line
                    if (GG == trackInformation.checkPoints[currentCheckPointIndex + 1])
                    {
                        // Add 1 more to the index for the next checkpoint
                        currentCheckPointIndex++;

                        // If the checkPointIndex is the same as the number of checkpoints (finish line)
                        if (currentCheckPointIndex == (trackInformation.checkPoints.Count - 1))
                        {                
                            // Add a lap for the player and make the index go back to 0
                            currentLap++;
                            currentCheckPointIndex = 0;

                            // If this was the last lap
                            if (currentLap == trackInformation.lapAmount)
                            {
                                // Player has finished the track
                                KB.Freeze();
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
