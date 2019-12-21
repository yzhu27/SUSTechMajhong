using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;

public class EatButton : MonoBehaviour
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
            GameObject.Find("HandTile").GetComponent<HandTile>().StartEat();
            clicked = true;
        }
        else
        {
            clicked = false;
            List<Tile> Fixed = GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles;
            GameObject.Find("WebController").GetComponent<WebController>().w.Eat(Fixed[0].id,Fixed[1].id);
            GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Clear();
        }
        
    }
}
