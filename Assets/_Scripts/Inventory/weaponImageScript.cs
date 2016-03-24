using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class weaponImageScript : MonoBehaviour {

    // Changes the Image component of this gameObject to the given sprite (Image of the weapon)
    public void updateSprite(Sprite sprite)
    {
        gameObject.GetComponent<Image>().sprite = sprite;
    }
}
