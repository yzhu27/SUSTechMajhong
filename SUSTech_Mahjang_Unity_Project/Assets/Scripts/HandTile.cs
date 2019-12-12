using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.GameMain;

public class HandTile : MonoBehaviour
{

    public MainPlayer myplayer ;
    //public List<Tile> ChoosedTiles = new List<Tile>();
    private List<GameObject> handTile = new List<GameObject>();
    [SerializeField]
    private float width = 0;

    public void setPlayer(MainPlayer player) => myplayer = player;

    public void unlight()
    {
        foreach(GameObject tile in handTile)
        {
            tile.GetComponentsInChildren<Transform>()[2].GetComponent<Lightuptile>().SendMessage("lightup",1);
        }
        
    }

    public void StartPlay()
    {
        foreach (GameObject tile in handTile)
        {
            tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetPlayCardState");            
        }

    }

    public void StartEat(Tile lastTile)
    {

        List<Tile> tiles = Rule.GetEatableList(lastTile, myplayer.hand);
        foreach (GameObject tile in handTile)
        {
            if (tiles.Contains(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile))
            {
                tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetEatState");
            }
            else
            {
                tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetProhibitedState");
            }
        }
        /*if (ChoosedTiles.Count == 0)
		{

			tiles = Rule.GetEatableList(lastTile, myplayer.hand);
			Debug.Log(tiles.Count);
		}
		else if (ChoosedTiles.Count == 1)
		{
			unlight();
			tiles = Rule.GetEatableList(lastTile, ChoosedTiles[0], myplayer.hand);
			Debug.Log(tiles.Count);
		}*/


    }

    public void lightupTouchable(Tile lastTile)
    {

       // List<Tile> tiles;
        /*if (ChoosedTiles.Count == 0)
        {

            tiles = Rule.GetTouchableList(lastTile, myplayer.hand);
            Debug.Log(tiles.Count);
        }
        else if (ChoosedTiles.Count == 1)
        {
            unlight();
            tiles = Rule.GetTouchableList(lastTile, ChoosedTiles[0], myplayer.hand);
            Debug.Log(tiles.Count);
        }*/
        
    }
    public Tile RemoveSingleTile(Tile tile)
    {
        myplayer.hand.Remove(tile);
        foreach (GameObject obj in handTile)
        {
            if (obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile == tile)
            {
                GameObject temp = obj;
                handTile.Remove(obj);
                Destroy(temp);
                GameObject.Find("lastTile").GetComponent<lastTile>().SetTile(tile);
                break;
            }
        }
        foreach (GameObject obj in handTile)
        {
            obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetProhibitedState");
        }
        Reconstruct();
        return tile;

    }
    public List<Tile> RemoveTile()
    {

        List<Tile> tiles = new List<Tile>(); 
        foreach (GameObject tile in handTile)
        {
            if(!(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tileState is ProhibitState))
            {
                GameObject temp = tile;
                Tile removed = tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile;
                myplayer.hand.Remove(removed);
                tiles.Add(removed);
                handTile.Remove(tile);
                Destroy(temp);
            }
            
        }
        
        Reconstruct();
        return tiles;

    }

    

   

    public void AddTile(int[] Tiles)
    {

    }

    public void AddTile(int TileId)
    {

        myplayer.hand.Add(new Tile(TileId));
        myplayer.hand.Sort();
        Reconstruct();
    }

    private void Reconstruct()
    {
        float length = myplayer.hand.Count * width;
        int bound = myplayer.hand.Count;
        
        foreach (GameObject tile in handTile)
        {
            Destroy(tile);
            
        }
        handTile.Clear();
        
        for (int i = 0; i < bound; i++)
        {
            GameObject instance =(GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Tile"));
            instance.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("setTile", myplayer.hand[i]);
            instance.GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront",myplayer.hand[i]));
            handTile.Add(instance);
            instance.transform.parent = transform;
            instance.transform.rotation = transform.rotation;
            instance.transform.position = transform.position - transform.right * (length - width)*0.5f + transform.right * width *i;
        }
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
