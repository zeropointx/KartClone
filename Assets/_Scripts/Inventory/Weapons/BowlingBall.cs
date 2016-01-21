using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BowlingBall : NetworkBehaviour
{
    public float thrust = 100;
    public Rigidbody rb;
    float destroyTimer = 0.0f;
    // Use this for initialization
    void Start()
    {
     //   if (!isServer)
     //   {
            
     //       this.enabled = false;
     //       return;
  //      }

        rb = GetComponent<Rigidbody>();
    }

    
    
    void FixedUpdate()
    {
      //  if (isServer)
     //   {
            rb.AddForce(transform.forward * thrust);
      //  }
    }

    // Update is called once per frame
    void Update()
    {
      //  if (!isServer)
      //      return;
        destroyTimer += Time.deltaTime;
        if (destroyTimer > 3.0f)
            Destroy(gameObject);
    }
}
