using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStacks : MonoBehaviour
{
    public Transform[] TileStack;
    // Start is called before the first frame update
    void Start()
    {
        //TileStack = GetComponentsInChildren<Transform>();

        //foreach (Transform child in TileStack)
        //{
        //    var instance = child.GetComponent<TileStack>();
        //    instance.AddTile(20);  
        //}

        BroadcastMessage("AddTile", 20);
        GameObject.Find("TillStack").GetComponent<TileStack>().AddTile(20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
