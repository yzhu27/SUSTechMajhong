using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStack : MonoBehaviour
{
    
    [SerializeField]
    GameObject tilePrefab = null;
    
    
    private List<TwoTiles> Tiles = new List<TwoTiles>();
    
    
    class TwoTiles
    {
        private GameObject upTile;
        private GameObject downTile;
        private int pointer;
        private Vector3 up;
        public TwoTiles(Vector3 up)
        {
            pointer = 0;
            this.up = up;
        }

        public void AddTile(GameObject tile)
        {
            if (pointer == 0)
            {
                downTile = tile;
                pointer++;
            }
            else
            {
                upTile = tile;
                upTile.transform.position += up;
                pointer++;
            }
            
            
        }

        public void RemoveTile()
        {
            if (pointer == 1)
            {
                Destroy(downTile);
                pointer--;
            }
            else
            {
                Destroy(upTile);
                pointer--;
            }
        }
    }

    public void AddTile(int number)
    {
        
        for (int i = 0; i < number; i++)
        {
            
            var instance = GameObject.Instantiate<GameObject>(tilePrefab, null);
            instance.transform.parent = transform;
            
            instance.transform.position = transform.position + transform.right * (instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x) * i;           
            TwoTiles temp = new TwoTiles(new Vector3(0, instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.z, 0));
            temp.AddTile(instance);
            
            instance = GameObject.Instantiate<GameObject>(tilePrefab, null);
            instance.transform.parent = transform;
            instance.transform.position = transform.position + transform.right * (instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x) * i;
            
            temp.AddTile(instance);
            
            Tiles.Add(temp);
           
        }
        return;
    }
   
    public void RemoveTile(int i)
    {

        Tiles[i].RemoveTile();       
        return;
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

