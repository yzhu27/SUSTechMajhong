using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTile : MonoBehaviour
{
    
   
    private List<GameObject> handTile = new List<GameObject>();
    private List<int> handTileId = new List<int>();
    [SerializeField]
    private float width = 0; 
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
        Debug.Log(handTile.Count);
        for (int i = 0; i < bound; i++)
        {
            GameObject instance =(GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Tile"));
            handTile.Add(instance);
            instance.transform.parent = transform;
            instance.transform.rotation = transform.rotation;
            instance.transform.position = transform.position - transform.right * (length - width)*0.5f + transform.right * width *i;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AddTile(20);
        AddTile(20);
        AddTile(20);
        AddTile(20);
        AddTile(20);
        AddTile(20);
        AddTile(20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
