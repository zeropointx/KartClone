using UnityEngine;
using System.Collections;

public class Drive : KartState {

    bool onReverse;
    private float angleTresshold;

	public Drive(GameObject _kart): base(_kart)
    {
        onReverse = kart.GetComponent<KartBehaviour>().pedal < 0;
        angleTresshold = 5.0f;
        name = "drive";
    }

    public override KartState UpdateState()
    {   
        //pedal

        kb.speed = kb.rigidbody.velocity.magnitude;
      

        if (kb.speed == 0)
            return new Stopped(kart);

        return null;
    }

    public override void UpdatePhysicsState()
    {
        
        if (kb.groundDistance < kb.jumpLimit)
        {
            Vector3 direction = kb.transform.forward;
            direction -= kb.groundNormal;
            float controlMultiplier = 1.0f;//onReverse ? -1 : (1.0f - 0.5f * (kb.speed / kb.maxSpeed));
            kb.transform.Rotate(new Vector3(0, controlMultiplier * kb.turnSpeed * kb.steeringWheel * Time.fixedDeltaTime, 0));

            direction = Vector3.ProjectOnPlane(direction, kb.groundNormal).normalized;

            var forwardSpeed = kb.speed >= kb.maxSpeed ? 0 : 1;//kb.speed / kb.maxSpeed;
          //  forwardSpeed = 1 - forwardSpeed;
            Vector3 forwardVector = kb.pedal * kb.transform.forward;
            Vector3 sideVector = kb.steeringWheel * kb.transform.right;
            kb.rigidbody.AddForce(forwardVector * forwardSpeed * 40.0f); 
            kb.rigidbody.AddForce(sideVector * 30.0f);
           // kb.rigidbody.velocity = new Vector3(direction.x * kb.speed, kb.rigidbody.velocity.y, direction.z * kb.speed);
        }
        kb.Stabilize();
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
