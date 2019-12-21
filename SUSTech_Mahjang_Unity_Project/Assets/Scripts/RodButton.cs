using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;

public class RodButton : MonoBehaviour
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
            GameObject.Find("HandTile").GetComponent<HandTile>().StartRod(GameObject.Find("GameManager").GetComponent<GameManager>().playDesk.roundPlayer);
            clicked = true;
        }
        else
        {
            clicked = false;
            List<Tile> Fixed = GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles;

            if (Fixed.Count > 1)
            {
                GameObject.Find("WebController").GetComponent<WebController>().w.Rod(Fixed[0].id, Fixed[1].id, Fixed[2].id);
            }
            else
            {
                GameObject.Find("WebController").GetComponent<WebController>().w.Rod(Fixed[0].id);
            }
            GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Clear();
        }

    }
}
