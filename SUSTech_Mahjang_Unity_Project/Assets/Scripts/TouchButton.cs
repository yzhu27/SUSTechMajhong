using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;
using Assets.Scripts.Util;
public class TouchButton : MonoBehaviour
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
            GameObject.Find("HandTile").GetComponent<HandTile>().StartTouch();
            clicked = true;
        }
        else
        {
            clicked = false;
            List<Tile> Fixed = GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles;
            GameObject.Find("WebController").GetComponent<WebController>().w.Touch(Fixed[0].id, Fixed[1].id);
        }
    }

}
