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
        kb.speed = kb.rigidbody.velocity.magnitude;
        if (kb.speed <= 0.1f)
            return new Stopped(kart);
        return null;
    }

    public override void UpdatePhysicsState()
    {
        
        if (kb.groundDistance < kb.jumpLimit)
        {


            /*
             * Texture based speed
             * 
             */
            
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

            /*
             * Forward
             * 
             */
            //If speed is too fast don't accelerate

            //Vector directly forward from player (or backwards)
            Vector3 forwardVector = kb.pedal * kb.transform.forward;

            onReverse = Vector3.Dot(kb.transform.forward, kb.rigidbody.velocity.normalized) <= 0.0f;

          
            //Speed which we go forward
            var forwardSpeed = 40.0f;

            Debug.Log(kb.lastTextureName);
            //If we are going too fast don't add force
            bool goingForwardTooFast;

            if(!onReverse)
                goingForwardTooFast = kb.speed >= kb.maxSpeed * kb.currentTextureSpeedModifier;
            else
                goingForwardTooFast = kb.speed >= kb.maxReverse;

            if (!goingForwardTooFast)
            kb.rigidbody.AddForce(forwardVector * forwardSpeed );


            /*
             * Sideways
             * 
             */
            float curveValue = GetCurveValue(kb.maxSpeed,kb.speed);
            //Vector directly left or right of player
            Vector3 sideVector = kb.steeringWheel * kb.transform.right;


            float controlMultiplier = onReverse ? Mathf.Clamp(-curveValue -0.3f, -1, 0) : Mathf.Clamp(curveValue + 0.15f, 0, 1);


            //If player is going reverse and presses go backward button flip sideways vector
            if (onReverse && kb.pedal <= 0.0f)
                sideVector *= -1f;

            var sideMultiplier = 30.0f * curveValue;

                kb.rigidbody.AddForce(sideVector * sideMultiplier);
                kb.transform.Rotate(new Vector3(0, controlMultiplier * kb.turnSpeed * kb.steeringWheel * Time.fixedDeltaTime, 0));
            
        }
        kb.Stabilize();
    }
    //Return between 0 and 1
    float GetCurveValue(float max, float value)
    {
        value = Mathf.Pow(value,1.5f);
        float finalValue = Mathf.Abs(value / Mathf.Pow(max, 1.5f));
        float clampedValue = Mathf.Clamp(finalValue, 0.0f, 1.0f);
        return clampedValue;
    }
    public override void CollisionEnter(Collision collision)
    {

    }
}
