using UnityEngine;
using System.Collections;

public class Frozen : KartState {

	public Frozen(GameObject _kart): base(_kart)
    {
        name = "frozen";
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();
        return null;
    }

    public override void CollisionEnter(Collision collision)
    {

    }
}
