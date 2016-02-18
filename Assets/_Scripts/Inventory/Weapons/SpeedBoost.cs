using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    StatusEffect boost = null;
    void Start()
    {
        boost = new Boost();
    }

    void Update()
    {
        boost.Update();
        if (boost.currentStatus == StatusEffect.Status.OFF)
            Destroy(gameObject);
    }
}
