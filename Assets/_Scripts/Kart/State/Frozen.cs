using UnityEngine;
using System.Collections;

public class Frozen : KartState {

	public Frozen(GameObject _kart): base(_kart)
    {
        name = "frozen";
    }

    public override KartState UpdateState()
    {
        return null;
    }

    public override void UpdatePhysicsState()
    {
        
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
