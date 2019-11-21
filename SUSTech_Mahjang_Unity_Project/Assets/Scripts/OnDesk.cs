using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.GameMain;
public class OnDesk : MonoBehaviour
{

    public MainPlayer myplayer;
    public void setPlayer(MainPlayer player) => myplayer = player;
     
    public void AddTiles(List<Tile> tiles )
    {
        bool upward = true;
      
        int length = tiles.Count;
        myplayer.onDesk.Add(tiles);
        GameObject instance;
        for (int i = 0; i < length; i++)
        {
            instance = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/TileOnDesk"));
            instance.transform.parent = transform;
            instance.transform.position = transform.position;
            instance.transform.position += transform.right * 0.025f*i; 
            if (upward)
            {
                Debug.Log(tiles[i]);
                Debug.Log(instance.GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>());
                instance.GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront", tiles[i]));
            }
        }
        transform.position += transform.up * 0.015f;
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
