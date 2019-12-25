using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.GameMain;
public class HandTileOthers : MonoBehaviour
{

    public Player myplayer ;
    private List<GameObject> handTile = new List<GameObject>();
    [SerializeField]
    private float width = 0;


    public void setPlayer(Player player) => myplayer = player;
 

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
            GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/OthersTile"));
            handTile.Add(instance);
            instance.transform.parent = transform;
            instance.transform.rotation = transform.rotation;
            instance.transform.position = transform.position - transform.right * (length - width) * 0.5f + transform.right * width * i;
        }
    }

    public void Win()
    {
    
        for(int i = 0; i < handTile.Count; i++)
        {
            handTile[i].transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            Debug.Log(Path.ImgPathOfTile("TileFront", myplayer.hand[i]));
            Debug.Log(myplayer.hand[i]);
            handTile[i].GetComponentsInChildren<Transform>()[2].GetComponent<AddTileFront>().SendMessage("addTileFront", Path.ImgPathOfTile("TileFront", myplayer.hand[i]));
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
