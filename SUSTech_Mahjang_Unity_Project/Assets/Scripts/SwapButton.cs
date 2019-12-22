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

    public void Refresh()
    {
        clicked = false;
    }
    void SendRefresh()
    {
        GameObject.Find("OperateBar").GetComponent<OperateBar>().SendMessage("RefreshSon");
    }
    public void MouseClick()
    {
        if (!clicked)
        {
            SendRefresh();
            clicked = true;
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("StartSwap");
            GameObject.Find("HideTiles").GetComponent<HideTiles>().SendMessage("ShowHide");
        }
        else
        {
            clicked = false;
            if(GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles.Count == 1 && GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Count == 1)
            {

                GameObject.Find("WebController").GetComponent<WebController>().w.Swap(GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles[0].id, GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles[0].id);
                GameObject.Find("HideTiles").GetComponent<HideTiles>().myplayer.Swap(GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles[0], GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles[0]);
                GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Clear();
                GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles.Clear();
                GameObject.Find("GameManager").GetComponent<GameManager>().playDesk.OnStart();
            }
        }
    }
}
