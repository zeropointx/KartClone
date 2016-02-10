using UnityEngine;
using System.Collections;

public abstract class KartState {

    protected GameObject kart = null;

    public KartState(GameObject _kart)
    {
        kart = _kart;
    }

    /*
     * Returns the new state or itself 
     */
    public abstract KartState UpdateState();
}
