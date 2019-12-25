using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStack : MonoBehaviour
{
    
    [SerializeField]
    GameObject tilePrefab = null;
    List<GameObject> tiles = new List<GameObject>();
    
    //private List<TwoTiles> Tiles = new List<TwoTiles>();
    //public static int Globalpointer = 0;
    
    /*class TwoTiles
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

        public bool RemoveTile()
        {
            if (pointer == 1)
            {
                Destroy(downTile);
                pointer--;
                return true;
            }
            else if(pointer == 2)
            {
                Destroy(upTile);
                pointer--;
                return true;
            }
            else
            {
                Globalpointer++;
                return false;
            }
        }
    }
*/
    public void AddTile(int number)
    {
        
        for (int i = 0; i < number; i++)
        {
            
            var instance = GameObject.Instantiate<GameObject>(tilePrefab, null);
            //Debug.Log(instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x);
            //Debug.Log(instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.y);
            //Debug.Log(instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.z);
            instance.transform.parent = transform;
            instance.transform.rotation = transform.rotation;
            instance.transform.position = transform.position + transform.right * (instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x) * i;
            //TwoTiles temp = new TwoTiles(new Vector3(0, instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.z, 0));
            //temp.AddTile(instance);
            tiles.Add(instance);
            instance = GameObject.Instantiate<GameObject>(tilePrefab, null);
            instance.transform.parent = transform;
            instance.transform.rotation = transform.rotation;
            instance.transform.position = transform.position + transform.right * (instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x) * i;

            tiles.Add(instance);

           
           
        }
        return;
    }
   
   /* public void RemoveTile(int i)
    {

        Tiles[i].RemoveTile();       
        return;
    }*/
    public void RemoveTile()
    {

        tiles.RemoveAt(tiles.Count);
       
    }

    // Start is called before the first frame update
    void Start()
    {

        AddTile(22);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

