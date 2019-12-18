using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.GameMain;
public class OnDesk : MonoBehaviour
{

    public Player myplayer;
    public bool upward = true;
    Vector3 position;
    public void setPlayer(Player player) => myplayer = player;
     
    public void AddTiles(List<Tile> tiles )
    {
       
        int length = tiles.Count;
        //myplayer.onDesk.Add(tiles);
        GameObject instance;
        for (int i = 0; i < length; i++)
        {
            instance = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/TileOnDesk"));
            instance.transform.parent = transform;
            instance.transform.rotation = transform.rotation;
            instance.transform.position = position;
            instance.transform.position += transform.right * 0.025f*i; 
            if (upward)
            {
                instance.GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront", tiles[i]));
            }
        }
        position += transform.forward * 0.04f;
        upward = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position ;

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
