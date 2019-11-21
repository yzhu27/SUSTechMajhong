using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTileOthers : MonoBehaviour
{
    private List<GameObject> handTile = new List<GameObject>();
    private List<int> handTileId = new List<int>();
    [SerializeField]
    private float width = 0;

    public void RemoveTile()
    {
        handTileId.RemoveAt(0);
        Reconstruct();
        return;
    }



    public void AddTile(int[] Tiles)
    {

    }

    public void AddTile(int Tile)
    {

        handTileId.Add(Tile);
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
            GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/OthersTile"));
            handTile.Add(instance);
            instance.transform.parent = transform;
            instance.transform.rotation = transform.rotation;
            instance.transform.position = transform.position - transform.right * (length - width) * 0.5f + transform.right * width * i;
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
