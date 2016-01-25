using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BowlingBall : NetworkBehaviour
{
    public float thrust = 1;
    public Rigidbody rb;
    float destroyTimer = 0.0f;
    KartBehaviour kBehaviour;
    // Use this for initialization
    void Start()
    {
        //   if (!isServer)
        //   {

        //       this.enabled = false;
        //       return;
        //      }

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }



    void FixedUpdate()
    {
        //  if (isServer)
        //   {
        
        //rb.AddForce(new Vector3(0, -0.1f, 0) * thrust, ForceMode.VelocityChange);
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

    void OnCollisionEnter(Collision col)
    {
        GameObject GG = col.gameObject;
        if(GG.tag == "Player")
        {
            kBehaviour = GG.GetComponent<KartBehaviour>();
            //Spin2Win
            kBehaviour.state = KartBehaviour.KartState.SPINNING;
            Destroy(gameObject);
        }
    }
}
