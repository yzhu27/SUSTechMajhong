using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;
using Assets.Scripts.Util;
public class lastTile : MonoBehaviour
{

    
    public GameObject theTile;
    Tile lasttile;
    public void SetTile(Tile tile)
    {
        if(theTile == null)
        {
            theTile = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/TileOnDesk"));
            theTile.transform.parent = transform;
            theTile.transform.rotation = transform.rotation;
            theTile.transform.position = transform.position;
        }
        lasttile = tile;
        theTile.GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront",tile));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
