using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class weaponImageScript : MonoBehaviour {

    // Changes the Image component of this gameObject to the given sprite (Image of the weapon)
    public void updateSprite(Sprite sprite)
    {
        gameObject.GetComponent<Image>().sprite = sprite;
    }
    public void changeColorAlpha(float alpha)
    {
        Color c = gameObject.GetComponent<Image>().color;
        c.a = alpha;
        gameObject.GetComponent<Image>().color = c;
    }
}
