using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapButton : MonoBehaviour
{

    bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseClick()
    {
        if (!clicked)
        {
            clicked = true;
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("StartSwap");
            GameObject.Find("HideTiles").GetComponent<HideTiles>().SendMessage("ShowHide");
        }
        else
        {
            clicked = false;
            if(GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles.Count == 1 && GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Count == 1)
            {
                GameObject.Find("HideTiles").GetComponent<HideTiles>().myplayer.Swap(GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles[0], GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles[0]);
                GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Clear();
                GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles.Clear();
            }
        }
    }
}
