using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;
using Assets.Scripts.Util;
using Assets.Scripts.LocalServer;
public class GameManager : MonoBehaviour
{
	private enum GameStatus
	{
		Preparing,
		Prepared,
		Waiting,
		Started
	}

    public PlayDesk playDesk = new PlayDesk();
    TilePool tilePool ;
    TileFactory tileFactory = new TileFactory();

	private GameStatus gameStatus;

    // Start is called before the first frame update
    void Start()
    {
		AutoCallBacks.playDesk = playDesk;
		AutoCallBacks.self = new User("a");
		AutoCallBacks.tileFactory = tileFactory;

		gameStatus = GameStatus.Preparing;
        
        //GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("setPlayer", playDesk.self);
        //GameObject.Find("HandTile (1)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.next);
        //GameObject.Find("HandTile (2)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.opposite);
        //GameObject.Find("HandTile (3)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.last);

        //GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.self);
        //GameObject.Find("OnDesk (1)").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.next);
        //GameObject.Find("OnDesk (2)").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.opposite);
        //GameObject.Find("OnDesk (3)").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.last);
        
        //GameObject.Find("HideTiles").GetComponent<HideTiles>().SendMessage("setPlayer", playDesk.self);

        /*List<Tile> tiles = new List<Tile>();
        tiles.Add(new Tile(0xf0230));
        tiles.Add(new Tile(0xf0230));
        tiles.Add(new Tile(0xf0230));
        tiles.Add(new Tile(0xf0230));
        GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles",tiles);
        GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles",tiles);
        GameObject.Find("OnDesk (1)").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);
        GameObject.Find("OnDesk (1)").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);
        GameObject.Find("OnDesk (2)").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);
        GameObject.Find("OnDesk (2)").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);
        GameObject.Find("OnDesk (3)").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);
        GameObject.Find("OnDesk (3)").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
         //List<Department> departments = new List<Department>();
         //departments.Add(Department.Math);
         //departments.Add(Department.Phy);
         //departments.Add(Department.Chem);
         //departments.Add(Department.Cse);
         //tilePool = new TilePool(departments);
         //tilePool.Shuffle();
        //int[] handtile = { 0xf0230, 0xf0230 , 0xf0230 ,0x30000,0x20000,0xf0220,0xf0210,0x30000, 0x20000, 0xf0220, 0xf0210 ,0x10000, 0x10000 };

       // Debug.Log(tilePool.PoolToString());


    }

    int t = 0;
    // Update is called once per frame
    void Update()
    {
        /* if(t<130 && t % 10 == 0)
         {
             GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("AddTile", tileFactory.GetTile(tilePool.Draw()));
             GameObject.Find("HandTile (1)").GetComponent<HandTileOthers>().SendMessage("AddTile", tileFactory.GetTile(tilePool.Draw()));
             GameObject.Find("HandTile (2)").GetComponent<HandTileOthers>().SendMessage("AddTile", tileFactory.GetTile(tilePool.Draw()));
             GameObject.Find("HandTile (3)").GetComponent<HandTileOthers>().SendMessage("AddTile", tileFactory.GetTile(tilePool.Draw()));
         }
         t++;
         if (t == 130)
         {
             GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("StartPlay");
         }*/

        //if (t < 130 && t % 10 == 0)
        //{
        //    playDesk.self.Draw(tileFactory.GetTile(tilePool.Draw()));
        //    playDesk.self.AddHidden(tileFactory.GetTile(tilePool.Draw()));
        //    playDesk.last.Draw();
        //    playDesk.next.Draw();
        //    playDesk.opposite.Draw();

        //}
        //t++;
        
		if (gameStatus == GameStatus.Preparing)
		{
			if (playDesk.prepareFinished)
			{
				gameStatus = GameStatus.Prepared;
				Debug.Log("Prepared");
			}
		}
		else if (gameStatus == GameStatus.Prepared)
		{
			GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("setPlayer", playDesk.self);
			GameObject.Find("HandTile (1)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.next);
			GameObject.Find("HandTile (2)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.opposite);
			GameObject.Find("HandTile (3)").GetComponent<HandTileOthers>().SendMessage("setPlayer", playDesk.last);

			GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.self);
			GameObject.Find("OnDesk (1)").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.next);
			GameObject.Find("OnDesk (2)").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.opposite);
			GameObject.Find("OnDesk (3)").GetComponent<OnDesk>().SendMessage("setPlayer", playDesk.last);

			GameObject.Find("HideTiles").GetComponent<HideTiles>().SendMessage("setPlayer", playDesk.self);

			gameStatus = GameStatus.Waiting;
		}
		else if (gameStatus == GameStatus.Waiting)
		{
			if (playDesk.canStart)
			{
				gameStatus = GameStatus.Started;
				Debug.Log("Started");
			}
		}
		else if (gameStatus == GameStatus.Started)
		{

		}
        
    }
}
