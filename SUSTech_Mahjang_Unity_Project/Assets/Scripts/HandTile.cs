using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.GameMain;

public class HandTile : MonoBehaviour
{

    public MainPlayer myplayer ;
    private List<GameObject> handTile = new List<GameObject>();
    [SerializeField]
    private float width = 0;

    public void setPlayer(MainPlayer player) => myplayer = player;
    public int RemoveTile()
    {
        int length = handTile.Count;
        for (int i=0; i < length; i++){
            
            if (handTile[i].GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile.IsChoosed() == true)
            {
                int id = myplayer.hand[i].getId();
                myplayer.hand.RemoveAt(i);
                Reconstruct();
                return id;
         }
            
        }
        return 0;
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
