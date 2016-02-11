using UnityEngine;
using System.Collections;

public class BoxSpawner : MonoBehaviour
{
    public float respawnTime = 10;  // Respawn time for the weaponBox
    public GameObject weaponBox;    // weaponBox prefab
    GameObject wBox;

    // Use this for initialization
    void Start()
    {
        // Spawn a weaponBox at the beginning
        wBox = (GameObject)Instantiate(weaponBox, gameObject.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (wBox == null)
        {
            respawnTime -= Time.deltaTime;
            if(respawnTime <= 0.0f)
            {
                respawnTime = 10;
                wBox = (GameObject)Instantiate(weaponBox, gameObject.transform.position, Quaternion.identity);

            }
        }
    }
}
