﻿using UnityEngine;
using System.Collections;

public class PlayCardState : State
{

    public PlayCardState(Vector3 position) : base(position)
    {

    }

    public override void OnMouseDown(GameObject tile)
    {
        GameObject.Find("WebController").GetComponent<WebController>().w.Play(tile.GetComponent<TileScript>().tile.id);
        GameObject.Find("Timer").GetComponent<Timer>().stopTimer();
        
    }
    public override void OnMouseEnter(GameObject tile)
    {
        
        upPosition = tile.transform.position + upDistance;
        downPosition = tile.transform.position;
        tile.transform.position = upPosition;
        Debug.Log(upPosition);
        Debug.Log(downPosition);
    }
    public override void OnMouseExit(GameObject tile)
    {
        GameObject.Find("HandTile").GetComponent<HandTile>().CheckPositions();                
    }

    public override void Lightup(GameObject tile)
    {
        tile.GetComponentInParent<Lightuptile>().SendMessage("lightup", 1);
    }
}

