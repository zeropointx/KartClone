using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {


    GameObject miniMapIcon;

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

    void Start() 
    {
        miniMapSprite = GameObject.Find("track").GetComponent<TrackInformation>().miniMapTexture;
        //miniMapObject = transform.Find("Image").gameObject;
        gameObject.GetComponent<Image>().sprite = miniMapSprite;

        mapWidth = gameObject.GetComponent<RectTransform>().rect.width;

        mapHeight = gameObject.GetComponent<RectTransform>().rect.height;

        sceneWidth = (int)(mapWidth * 20);
        sceneHeight = (int)(mapHeight * 20);
        miniMapIcon = Instantiate(defaultIcon);
        miniMapIcon.transform.parent = transform;
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

        pX = GetMapPos(player.transform.position.x, mapWidth, sceneWidth);
        pZ = GetMapPos(player.transform.position.z, mapHeight, sceneHeight);
        playerMapX = pX ;
        playerMapZ = pZ  ;

        miniMapIcon.GetComponent<RectTransform>().localPosition = new Vector3(playerMapX, playerMapZ, 0);
        
    }

    float GetMapPos(float pos, float mapSize, float sceneSize)
    {
        return pos * mapSize / sceneSize;
    }

    //void OnGUI()
    //{
    //    //GUI.BeginGroup(new Rect(mapOffSetX, mapOffSetY, mapWidth, mapHeight), miniMap);


    //    GUI.Box(new Rect(playerMapX, playerMapZ, iconSize, iconSize), playerIcon);
    //    GUI.EndGroup();
    //}
}
