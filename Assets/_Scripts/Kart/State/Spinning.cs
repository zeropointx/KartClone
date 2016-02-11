using UnityEngine;
using System.Collections;

public class Spinning : KartState {

    private float spinTimer;
    private float spinTime;

	public Spinning(GameObject _kart): base(_kart)
    {
        spinTimer = 0;
        spinTime = 3;
    }

    public override KartState UpdateState()
    {
        KartBehaviour kb = kart.GetComponent<KartBehaviour>();

        kb.speed = 0;
        spinTimer += Time.deltaTime;
        kart.transform.Rotate(0, kb.spinSpeed * Time.deltaTime, 0);
        if (spinTimer > spinTime)
        {
            spinTimer = 0;
            kb.pw.hitState = PlayerNetwork.KartHitState.NORMAL;
            return new Stopped(kart);
        }

        kb.UpdateTransform();
        return null;
    }
}
