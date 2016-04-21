using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    TrackInformation TI;
    GameObject miniMapIcon;

    Gamemode gm;

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

    //Offset variables (X and Y) - where you want to place your map on screen.
    //int mapOffSetX = 900;
    //int mapOffSetY = 0;

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

    void Start() 
    {
        gm = GameObject.Find("GameMode").GetComponent<Gamemode>();
        TI = GameObject.Find("track").GetComponent<TrackInformation>(); // Get TrackInformation
        miniMapSprite = TI.miniMapTexture;                                                          // Get the sprite of the minimap from TrackInformation
        gameObject.GetComponent<Image>().sprite = miniMapSprite;                                    // Set it to be the current minimap sprite
        mapWidth = gameObject.GetComponent<RectTransform>().rect.width;                             // Width of the minimap
        mapHeight = gameObject.GetComponent<RectTransform>().rect.height;                           // Height of the minimap
        miniMapIcon = Instantiate(defaultIcon);                                                     // Create a minimap marker for a player
        miniMapIcon.transform.parent = transform;                                                   // Set its parent to be minimap GameObject
        sceneWidth = TI.trackWidth;                                                                 // Width of the track
        sceneHeight = TI.trackHeight;                                                               // Height of the track
        offSetX = TI.miniMapOffSetX;                                                                // Offset if the minimap marker is on the wrong place
        offSetZ = TI.miniMapOffSetZ;

        for (int i = 0; i < gm.GetPlayers().Capacity; i++)
        {
            
        }
    }
    void Update()
    {
        if (player == null)
        {
            GameObject playerOb = PlayerNetwork.localPlayer;
            if (playerOb == null)
                return;
            player = PlayerNetwork.localPlayer.transform;
            playerIcon = player.Find("Kart").GetComponent<KartInformation>().miniMapIcon;
            miniMapIcon.GetComponent<Image>().sprite = playerIcon;
        }




            //So that the pivot point of the icon is at the middle of the image.

            pX = GetMapPos(player.transform.position.x - offSetX, mapWidth, sceneWidth);
        pZ = GetMapPos(player.transform.position.z - offSetZ, mapHeight, sceneHeight);
        //playerMapX = pX;
        //playerMapZ = pZ;

        miniMapIcon.GetComponent<RectTransform>().localPosition = new Vector3(pX , pZ , 0);
        
    }

    float GetMapPos(float pos, float mapSize, float sceneSize)
    {
        return pos * mapSize / sceneSize;
    }

}
