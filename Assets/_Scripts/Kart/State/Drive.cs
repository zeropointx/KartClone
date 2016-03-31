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
             onReverse = Vector3.Dot(kb.transform.forward, kb.rigidbody.velocity.normalized) <= 0.0f;
            float controlMultiplier = onReverse ? -1 : (1.0f - 0.5f * (kb.speed / kb.maxSpeed));
            kb.transform.Rotate(new Vector3(0, controlMultiplier * kb.turnSpeed * kb.steeringWheel * Time.fixedDeltaTime, 0));

            var speedModifier = 40.0f;
            //If we are on land and player isn't using boost slow him down
                if (kb.lastTextureName == "Land" && !kb.GetComponent<PlayerNetwork>().GetStatusEffectHandler().HasEffect(StatusEffectHandler.EffectType.BOOST))
                {
                    kb.currentTextureSpeedModifier = 0.25f; 
                }
                    //Else let him go full speed
                else
                {
                    kb.currentTextureSpeedModifier = 1.0f;
                    
                }
                var forwardSpeed = kb.speed >= kb.maxSpeed * kb.currentTextureSpeedModifier ? 0 : 1;
            Vector3 forwardVector = kb.pedal * kb.transform.forward;
            Vector3 sideVector = kb.steeringWheel * kb.transform.right;
            if (onReverse)
                sideVector *= -1f;
            kb.rigidbody.AddForce(forwardVector * forwardSpeed * speedModifier); 
            kb.rigidbody.AddForce(sideVector * 30.0f);
        }
        kb.Stabilize();
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
