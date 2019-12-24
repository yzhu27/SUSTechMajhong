using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.GameMain;

public class HandTile : MonoBehaviour
{

    public MainPlayer myplayer ;
    public List<Tile> ChoosedTiles = new List<Tile>();
    private List<GameObject> handTile = new List<GameObject>();
    private List<Vector3> positions = new List<Vector3>();
    [SerializeField]
    private float width = 0;
    private float MoveSpeed = 0.05f;

    private bool check = true;

    public void setPlayer(MainPlayer player) => myplayer = player;

    public void StartProhibit()
    {
        foreach(GameObject tile in handTile)
        {
            tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetProhibitedState");
        }
        
    }

    public void StartPlay()
    {
        foreach (GameObject tile in handTile)
        {
            tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetPlayCardState");            
        }

    }

    public void StartSwap()
    {
        foreach (GameObject tile in handTile)
        {
            tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetSwapState");
        }

    }

    public void StartEat()
    {
       
        HashSet<Tile> tiles = myplayer.GetResponseTiles(Action.Eat, ChoosedTiles);
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

    public void StartTouch()
    {

        HashSet<Tile> tiles = myplayer.GetResponseTiles(Action.Touch, ChoosedTiles);
        foreach (GameObject tile in handTile)
        {
            if (tiles.Contains(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile))
            {
                tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetTouchState");
            }
            else
            {
                tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetProhibitedState");
            }
        }
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

    public void StartRod(Seat seat)
    {
        HashSet<Tile> tiles;


        if (seat == Seat.Self)
        {
            tiles = myplayer.GetActionTiles(Action.Rod, ChoosedTiles);
        }
        else
        {
            tiles = myplayer.GetResponseTiles(Action.Rod, ChoosedTiles);
        }
        
        foreach (GameObject tile in handTile)
        {
            if (tiles.Contains(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile))
            {
                tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetRodState");
            }
            else
            {
                tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetProhibitedState");
            }
        }
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
        
        foreach (GameObject obj in handTile)
        {
            Debug.Log(obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile);
            Debug.Log(tile);
            if (obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile.id == tile.id)
            {
                GameObject temp = obj;
                handTile.Remove(obj);
                Destroy(temp);
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
    public List<Tile> RemoveTile(List<Tile> tiles)
    {

        foreach (Tile tile in tiles){
            foreach (GameObject obj in handTile)
            {
                if (obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile.id == tile.id)
                {
                    GameObject temp = obj;
                    handTile.Remove(obj);
                    Destroy(temp);
                    break;
                }
            }
        }
        foreach (GameObject obj in handTile)
        {
            obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetProhibitedState");
        }
        Reconstruct();
        return tiles;

    }

    

   

 

    public void AddTile(Tile tile)
    {
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
        try
        {
            return positions[myplayer.hand.IndexOf(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile)];
        }
        catch (System.Exception e)
        {
            Debug.Log(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile);
            throw e;
        }
    }

    public void Win()
    {
        foreach(GameObject tile in handTile)
        {
            tile.transform.rotation =  Quaternion.Euler(90f, 0f, 0f);
        }
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
                Vector3 tempp = tile.transform.position;
                tile.transform.position = Vector3.MoveTowards(tile.transform.position, temp, 2 * MoveSpeed * Time.deltaTime);
                if (tile.transform.position != tempp)
                {
                    check = false;
                }
            }
        }
        
    }
}
