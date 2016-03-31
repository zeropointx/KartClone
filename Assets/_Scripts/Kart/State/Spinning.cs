using UnityEngine;
using System.Collections;

public class Spinning : KartState
{
    
    int spinSpeed = 12;
    float spinDegrees = 1080;
    float speedReduction = 0.1f;
    float spinIndex = 0;
    
    public Spinning(GameObject _kart)
        : base(_kart)
    {
        name = "spinning";
       kb.childKart.GetComponent<Animator>().SetTrigger("Hit");
        
    }

    public override KartState UpdateState()
    {
        /*var animator = kb.childKart.GetComponent<Animator>();
        var x = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(x.length + "," + x.normalizedTime);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ShoppingKartAnimation"))
            return new Drive(kart);
        //animator.
       // if (!.IsPlaying("YourAnimation"))
     //       print("Animation Done");
        else
            return null;*/
        return new Drive(kart);
    }

    public override void UpdatePhysicsState()
    {

    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
