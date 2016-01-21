using UnityEngine;
using System.Collections;

public class BowlingBall : MonoBehaviour
{
    public float thrust = 100;
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * thrust);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
