using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;
using Assets.Scripts.Util;
public class lastTile : MonoBehaviour
{

    
    public List<GameObject>theTile = new List<GameObject>();
    [SerializeField]
    private float width = 0.025f;
    
    public void SetTile(Tile tile)
    {
        GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/TileOnDesk"));
        instance.transform.parent = transform;
        instance.transform.rotation = transform.rotation;
        instance.transform.position = transform.position;
        instance.GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront",tile));
        theTile.Add(instance);
        Reconstruct();
    }
    private void Reconstruct()
    {
        float length = theTile.Count * width;
        int bound = theTile.Count;
        

        for (int i = 0; i < bound; i++)
        {
            theTile[i].transform.position = transform.position - transform.right * (length - width) * 0.5f + transform.right * width * i;
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
