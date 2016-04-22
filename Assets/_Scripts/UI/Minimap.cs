using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{

    TrackInformation TI;
    GameObject miniMapIcon;

    Gamemode gm = null;

    //For placing the image of the mini map.
    Sprite miniMapSprite;

    //GameObject miniMapObject;

    // Empty minimap Icon that will be filled later
    public GameObject defaultIcon;

    //Two transform variables, one for the player's and the enemy's, 
    Transform player = null;
    Transform enemy;

    //Icon images for the player and enemy(s) on the map. 
    public Sprite playerIcon;
    public Sprite enemyIcon;

    //The width and height of your map as it'll appear on screen,
    public float mapWidth;
    public float mapHeight;

    //Width and Height of your scene, or the resolution of your terrain.
    public int sceneWidth;
    public int sceneHeight;


    // Variables that are needed in update
    public float pX;
    public float pZ;
    public float playerMapX;
    public float playerMapZ;

    public float offSetX;
    public float offSetZ;
    bool initialized = false;

    // Variables when creating minimap icons
    GameObject tempIcon;
    void Start()
    {
        TI = GameObject.Find("track").GetComponent<TrackInformation>();                             // Get TrackInformation
        miniMapSprite = TI.miniMapTexture;                                                          // Get the sprite of the minimap from TrackInformation
        gameObject.GetComponent<Image>().sprite = miniMapSprite;                                    // Set it to be the current minimap sprite
        mapWidth = gameObject.GetComponent<RectTransform>().rect.width;                             // Width of the minimap
        mapHeight = gameObject.GetComponent<RectTransform>().rect.height;                           // Height of the minimap
        sceneWidth = TI.trackWidth;                                                                 // Width of the track
        sceneHeight = TI.trackHeight;                                                               // Height of the track
        offSetX = TI.miniMapOffSetX;                                                                // Offset if the minimap marker is on the wrong place
        offSetZ = TI.miniMapOffSetZ;


    }
    void Update()
    {
        /// INITIALIZE ///
        if (gm == null || !initialized)
        {
            GameObject gg = GameObject.Find("Gamemode");
            if (gg == null)
                return;
            gm = gg.GetComponent<Gamemode>();
            if (gm.currentState == Gamemode.State.RACING)
                initialized = true;
            else
                return;
            for (int i = 0; i < gm.GetPlayers().Count; i++)
            {


                tempIcon = Instantiate(defaultIcon);                                            // Create a minimap marker for a player
                tempIcon.transform.parent = transform;                                          // Set its parent to be minimap GameObject
                tempIcon.GetComponent<Image>().sprite = gm.GetPlayers()[i].gameObject.transform.Find("Kart").GetComponent<KartInformation>().miniMapIcon;   // Set the icon sprite to be sprite from KartInformation
                gm.GetPlayers()[i].gameObject.GetComponent<Placement>().miniMapObject = tempIcon;   // Set the created icon to be this players personal minimap icon
            }
        }

        /// UPDATE ///
        for (int i = 0; i < gm.GetPlayers().Count; i++)
        {
            pX = GetMapPos(gm.GetPlayers()[i].gameObject.transform.position.x - offSetX, mapWidth, sceneWidth);
            pZ = GetMapPos(gm.GetPlayers()[i].gameObject.transform.position.z - offSetZ, mapHeight, sceneHeight);
            gm.GetPlayers()[i].gameObject.GetComponent<Placement>().miniMapObject.GetComponent<RectTransform>().localPosition = new Vector3(pX, pZ, 0);
        }
    }

    float GetMapPos(float pos, float mapSize, float sceneSize)
    {
        return pos * mapSize / sceneSize;
    }

}
