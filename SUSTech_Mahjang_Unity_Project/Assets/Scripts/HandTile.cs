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
    private List<Vector3> positions = new List<Vector3>();
    [SerializeField]
    private float width = 0;
    private float MoveSpeed = 0.05f;

    private bool check = true;

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

        List<Tile> tiles = Rule.GetEatableTiles(lastTile, myplayer.hand);
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

    

   

 

    public void AddTile(Tile tile)
    {
        myplayer.hand.Add(tile);
        myplayer.hand.Sort();
        Reconstruct();
        
        GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Tile"));
        instance.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("setTile",tile);
        instance.GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront", tile));
        handTile.Add(instance);
        instance.transform.parent = transform;
        instance.transform.rotation = transform.rotation;
        instance.transform.position = positions[myplayer.hand.IndexOf(tile)];
    }

    private void Reconstruct()
    {
        float length = myplayer.hand.Count * width;
        int bound = myplayer.hand.Count;
        positions.Clear();
        
        for (int i = 0; i < bound; i++)
        {
            positions.Add(transform.position - transform.right * (length - width)*0.5f + transform.right * width *i);
        }

        check = false;
        

    }

    private Vector3 getposition(GameObject tile)
    {
        return positions[myplayer.hand.IndexOf(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile)];      
    }

   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!check)
        {
            check = true;
            foreach (GameObject tile in handTile)
            {
               
                Vector3 temp = getposition(tile);
                tile.transform.position = Vector3.MoveTowards(tile.transform.position, temp, 2 * MoveSpeed * Time.deltaTime);
                if (tile.transform.position != temp)
                {
                    check = false;
                }
            }
        }
        
    }
}
