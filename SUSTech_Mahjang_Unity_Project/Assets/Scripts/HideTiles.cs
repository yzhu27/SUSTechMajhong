using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.GameMain;

public class HideTiles : MonoBehaviour
{
    public MainPlayer myplayer;
    public List<Tile> ChoosedTiles = new List<Tile>();
    private List<GameObject> hidenTile = new List<GameObject>();
    private List<Vector3> positions = new List<Vector3>();
    [SerializeField]
    private float width = 0.025f;
    private float MoveSpeed = 0.05f;

    private bool check = true;
    public void setPlayer(MainPlayer player) => myplayer = player;

    public void ShowHide()
    {
        foreach (GameObject tile in hidenTile)
        {
            tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetHideState");
        }

    }
    public Tile RemoveSingleTile(Tile tile)
    {

        foreach (GameObject obj in hidenTile)
        {
            if (obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile == tile)
            {
                GameObject temp = obj;
                hidenTile.Remove(obj);
                Destroy(temp);
                GameObject.Find("lastTile").GetComponent<lastTile>().SetTile(tile);
                break;
            }
        }
        foreach (GameObject obj in hidenTile)
        {
            obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetProhibitedState");
        }
        Reconstruct();
        return tile;

    }
    public List<Tile> RemoveTile(List<Tile> tiles)
    {

        foreach (Tile tile in tiles)
        {
            foreach (GameObject obj in hidenTile)
            {
                if (obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile == tile)
                {
                    GameObject temp = obj;
                    hidenTile.Remove(obj);
                    Destroy(temp);
                    GameObject.Find("lastTile").GetComponent<lastTile>().SetTile(tile);
                    break;
                }
            }
        }
        foreach (GameObject obj in hidenTile)
        {
            obj.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("SetProhibitedState");
        }
        Reconstruct();
        return tiles;

    }







    public void AddTile(Tile tile)
    {
        Reconstruct();
        GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Tilehidden"));
        instance.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().SendMessage("setTile", tile);
        instance.GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront", tile));
        hidenTile.Add(instance);
        instance.transform.parent = transform;
        instance.transform.rotation = transform.rotation;
        instance.transform.position = positions[myplayer.hiden.IndexOf(tile)];
    }

    private void Reconstruct()
    {
        float length = myplayer.hiden.Count * width;
        int bound = myplayer.hiden.Count;
        positions.Clear();

        for (int i = 0; i < bound; i++)
        {
            positions.Add(transform.position - transform.right * (length - width) * 0.5f + transform.right * width * i);
        }

        check = false;


    }

    private Vector3 getposition(GameObject tile)
    {
        return positions[myplayer.hiden.IndexOf(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile)];
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
            foreach (GameObject tile in hidenTile)
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
