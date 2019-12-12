using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;

public class EatButton : MonoBehaviour
{
   
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseEnter()
    {
        GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("lightupEatable", new Tile(0xf0230));
    }
    public void MouseExit()
    {
        GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("unlight");
    }
    public void MouseClick()
    {
        //GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile");
        GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("StartEat", new Tile(0xf0230));
    }
}
