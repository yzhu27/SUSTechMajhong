using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;
using Assets.Scripts.Util;
using Assets.Scripts.LocalServer;
public class GameManager : MonoBehaviour
{
    public PlayDesk playDesk ;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayDesk playDesk = new PlayDesk();
        
        GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("setPlayer", playDesk.self);
        GameObject.Find("HandTile (1)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.last);
        GameObject.Find("HandTile (2)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.opposite);
        GameObject.Find("HandTile (3)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.next);

        GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.self);
        //GameObject.Find("OthersTile").GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront", new Tile(0xf0230)));
        
        //List<Tile> tiles = new List<Tile>();
        //tiles.Add(new Tile(0xf0230));
        //tiles.Add(new Tile(0xf0230));
        //tiles.Add(new Tile(0xf0230));
        //tiles.Add(new Tile(0xf0230));
        //GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles",tiles);
        //GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);
        
        List<Department> departments = new List<Department>();
        departments.Add(Department.Math);
        departments.Add(Department.Math);
        departments.Add(Department.Math);
        departments.Add(Department.Math);
        TilePool tilePool = new TilePool(departments);
        tilePool.Shuffle();
        //int[] handtile = { 0xf0230, 0xf0230 , 0xf0230 ,0x30000,0x20000,0xf0220,0xf0210,0x30000, 0x20000, 0xf0220, 0xf0210 ,0x10000, 0x10000 };
        for (int i = 0; i < 13; i++)
        {
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("AddTile", tilePool.Draw());
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
