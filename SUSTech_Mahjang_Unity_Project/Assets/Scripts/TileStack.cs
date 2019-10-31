using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStack : MonoBehaviour
{
    
    [SerializeField]
    GameObject tilePrefab = null;
    
    
    private List<GameObject> Tiles;
    private int amount = 0;
    
    void AddTile(int number)
    {
        bool up = false;
        for (int i = 0; i < number; i++)
        {

            var instance = GameObject.Instantiate<GameObject>(tilePrefab, null);
            instance.transform.parent = transform;
            Debug.Log(transform.rotation.y);
            if (transform.rotation.y == 0)
            {
                instance.transform.position = transform.position + new Vector3((instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x) * (amount / 2), 0, 0);
            }else if(transform.rotation.y == 1)
            {
                instance.transform.position = transform.position - new Vector3((instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x) * (amount / 2), 0, 0);
               
            }else if(transform.rotation.y == -0.7071068f)//
            {
                instance.transform.position = transform.position + new Vector3(0, 0, (instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x) * (amount / 2));
            }
            else
            {
                instance.transform.position = transform.position - new Vector3(0, 0, (instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.x) * (amount / 2));
            }
            instance.transform.rotation = transform.rotation;
            Debug.Log(instance.GetComponentsInChildren<Transform>()[2]);
            if (up)
            {
                instance.transform.position += new Vector3(0, instance.GetComponentsInChildren<Transform>()[2].GetComponent<MeshFilter>().mesh.bounds.size.z, 0);
            }
            Tiles.Add(instance);
            up = ((amount+2) % 2 == 0);
            amount++;
        }
        return;
    }
   
    void RemoveTile()
    {
        Destroy(Tiles[amount - 1]);
        Tiles.RemoveAt(amount - 1);
        amount--;
        return;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Tiles = new List<GameObject>();
        AddTile(40);
        RemoveTile();
        RemoveTile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
