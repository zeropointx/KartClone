using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{

    public float activeTimer = 3;
    public float thrust = 1;

    float destroyTime = 0;
    GameObject player;
    Rigidbody playerRB;


    // Use this for initialization
    void Start()
    {
       // player = GameObject.Find("Player");
       // playerRB = player.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        destroyTime += Time.deltaTime;
       // playerRB.AddForce(transform.forward * thrust, ForceMode.Acceleration);
        if (destroyTime > activeTimer)
        {
            Destroy(gameObject);
        }
    }
}
