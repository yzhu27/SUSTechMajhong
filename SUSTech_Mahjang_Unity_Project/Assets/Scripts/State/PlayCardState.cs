using UnityEngine;
using System.Collections;
using Assets.Scripts.GameMain;

public class PlayCardState : State
{

    public PlayCardState(Vector3 position) : base(position)
    {

    }

    public override void OnMouseDown(GameObject tile)
    {
        Tile temp = tile.GetComponent<TileScript>().tile;
        if (temp.GetSpecial() ==Special.King || temp.GetSpecial() ==Special.Logo)
        {
            GameObject.Find("WebController").GetComponent<WebController>().w.Rod(temp.id);
            return;
        }
        GameObject.Find("WebController").GetComponent<WebController>().w.Play(temp.id);
        GameObject.Find("Timer").GetComponent<Timer>().stopTimer();
        GameObject.Find("OperateBar").GetComponent<OperateBar>().Prohibt();
        
    }
    public override void OnMouseEnter(GameObject tile)
    {
        
        upPosition = tile.transform.position + upDistance;
        tile.transform.position = upPosition;
        
    }
    public override void OnMouseExit(GameObject tile)
    {
        GameObject.Find("HandTile").GetComponent<HandTile>().CheckPositions();
        
        tile.transform.position =new Vector3(tile.transform.position.x,3.024994f,tile.transform.position.z);              
    }

    public override void Lightup(GameObject tile)
    {
        tile.GetComponentInParent<Lightuptile>().SendMessage("lightup", 1);
    }
}

