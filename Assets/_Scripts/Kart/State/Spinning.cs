using UnityEngine;
using System.Collections;

public class Spinning : KartState
{
    Transform childKart;
    int spinSpeed = 12;
    float spinDegrees = 1080;
    float speedReduction = 0.1f;
    float spinIndex = 0;
    Quaternion originalRotation;
    public Spinning(GameObject _kart)
        : base(_kart)
    {
        childKart = _kart.transform.Find("shoppingcart");
        originalRotation = childKart.transform.localRotation;
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();
        if (spinDegrees >= spinIndex)
        {
            if (kb.speed > 0)
            {
                kb.speed -= speedReduction;
            }
            spinIndex += spinSpeed;
            childKart.transform.Rotate(new Vector3(0, spinSpeed, 0));
        }

        else
        {
            spinIndex = 0;
            childKart.transform.localRotation = originalRotation;
            return new Drive(kart);
        }
        kb.UpdateTransform();
        return null;
    }
}
