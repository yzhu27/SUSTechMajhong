using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTile : MonoBehaviour
{
    
   
    private List<GameObject> handTile = new List<GameObject>();
    private List<int> handTileId = new List<int>();
    [SerializeField]
    private float width = 0; 
    
    public void RemoveTile()
    {
        int length = handTile.Count;
        for (int i=0; i < length; i++){
            
            if (handTile[i].GetComponentsInChildren<Transform>()[2].GetComponent<Tile>().isclick == true)
            {
                handTileId.RemoveAt(i);
            Reconstruct();
                break;
         }
        }

    }

    public void AddTile(int[] Tiles)
    {

    }

    public void AddTile(int Tile)
    {
        
        handTileId.Add(Tile);
        handTileId.Sort();
        Reconstruct();
    }

    private void Reconstruct()
    {
        float length = handTileId.Count * width;
        int bound = handTileId.Count;
        
        foreach (GameObject tile in handTile)
        {
            Destroy(tile);
            
        }
        handTile.Clear();
        
        for (int i = 0; i < bound; i++)
        {
            GameObject instance =(GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Tile"));
            instance.GetComponentsInChildren<Transform>()[2].GetComponent<Tile>().SendMessage("AddTileFront", handTileId[i]);
            handTile.Add(instance);
            instance.transform.parent = transform;
            instance.transform.rotation = transform.rotation;
            instance.transform.position = transform.position - transform.right * (length - width)*0.5f + transform.right * width *i;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AddTile(1);
        AddTile(2);
        AddTile(2);
        AddTile(3);
        AddTile(1);
        AddTile(3);
        AddTile(1);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
