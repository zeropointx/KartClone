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

        kb.childKart.GetComponent<Animator>().Play("ShoppingKartAnimation");
    }

    public override KartState UpdateState()
    {
        //if (spinDegrees >= spinIndex)
        //{
        //    if (kb.speed > 0)
        //    {
        //        kb.speed -= speedReduction;
        //    }
        //    spinIndex += spinSpeed;
        //    kb.childKart.transform.Rotate(new Vector3(0, spinSpeed, 0));
        //}

        //else
        //{
        //    spinIndex = 0;
        //    kb.childKart.transform.localRotation = kb.originalRotation;
        if (kb.childKart.GetComponent<Animator>().GetTime() > 3)
        {
            kb.childKart.GetComponent<Animator>().Stop();
            return new Drive(kart);
        }

  //  }
        else
            return null;
    }

    public override void UpdatePhysicsState()
    {

    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
