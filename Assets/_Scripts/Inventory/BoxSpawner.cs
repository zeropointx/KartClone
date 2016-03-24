using UnityEngine;
using System.Collections;

// Class made for spawning the weaponBox
public class BoxSpawner : MonoBehaviour
{
    public float respawnTime = 10;  // Respawn time for the weaponBox
    public GameObject weaponBox;    // weaponBox prefab
    GameObject wBox;

    void Start()
    {
        // Spawn a weaponBox at the beginning
        wBox = (GameObject)Instantiate(weaponBox, gameObject.transform.position, Quaternion.identity);
    }

    void Update()
    {
        // If there isnt a weabonBox on the spawner
        if (wBox == null)
        {
            // Spawn it in the given respawn time on the spawner
            respawnTime -= Time.deltaTime;
            if(respawnTime <= 0.0f)
            {
                respawnTime = 10;
                wBox = (GameObject)Instantiate(weaponBox, gameObject.transform.position, Quaternion.identity);

            }
        }
    }
}
