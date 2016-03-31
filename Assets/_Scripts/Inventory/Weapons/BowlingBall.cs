using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BowlingBall : NetworkBehaviour
{
    public float thrust = 50000;

    public Rigidbody rb;
    Vector3 oldVel;

    float destroyTimer = 0.0f;
    PlayerNetwork PN;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * thrust, ForceMode.Force);
    }

    void FixedUpdate()
    {
        oldVel = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime;
        if (destroyTimer > 3.0f)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        ContactPoint cp = col.contacts[0];
        GameObject GG = col.gameObject;
        rb.velocity = Vector3.Reflect(oldVel, cp.normal);
        rb.velocity += cp.normal * 2.0f;
        Vector3 tempVel = rb.velocity;
        tempVel.y = 0.0f;
        rb.velocity = tempVel;

        if(GG.tag == "Player")
        {
            PN = GG.GetComponent<PlayerNetwork>();
            PN.GetStatusEffectHandler().AddStatusEffect(StatusEffectHandler.EffectType.HIT);
            Destroy(gameObject);
        }
    }
}
