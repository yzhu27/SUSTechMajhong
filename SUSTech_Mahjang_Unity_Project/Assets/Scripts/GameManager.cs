using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;
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

        List<Tile> tiles = new List<Tile>();
        tiles.Add(new Tile(0xf0230));
        tiles.Add(new Tile(0xf0230));
        tiles.Add(new Tile(0xf0230));
        tiles.Add(new Tile(0xf0230));
        Debug.Log(tiles[0]);
        GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles",tiles);
        GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("AddTile", 0xf0230);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
