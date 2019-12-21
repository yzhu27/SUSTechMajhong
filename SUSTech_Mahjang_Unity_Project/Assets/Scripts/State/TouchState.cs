﻿using UnityEngine;
using System.Collections;

public class TouchState : State
{

    private bool choosed;
    public TouchState(Vector3 position) : base(position)
    {
        choosed = false;
    }

    public override void OnMouseDown(GameObject tile)
    {
        if (choosed)
        {
            choosed = false;
            GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Remove(tile.GetComponent<TileScript>().tile);
        }
        else
        {
            choosed = true;
            GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Add(tile.GetComponent<TileScript>().tile);
            GameObject.Find("HandTile").GetComponent<HandTile>().StartTouch();
        }
    }
    public override void OnMouseEnter(GameObject tile)
    {
        upPosition = tile.transform.position + upDistance;
        downPosition = tile.transform.position;
        tile.transform.position = upPosition;
    }
    public override void OnMouseExit(GameObject tile)
    {
        if (!choosed)
            tile.transform.position = downPosition;
    }
    public override void Lightup(GameObject tile)
    {
        tile.GetComponentInParent<Lightuptile>().SendMessage("lightup", 2);
    }
}
