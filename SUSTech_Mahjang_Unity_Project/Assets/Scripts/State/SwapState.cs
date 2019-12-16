using UnityEngine;
using System.Collections;

public class SwapState : State
{

    private bool choosed;
    public SwapState(Vector3 position) : base(position)
    {
        choosed = false;
    }

    public override void OnMouseDown(GameObject tile)
    {
        if (choosed)
        {
            choosed = false;
            GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Remove(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile);
        }
        else
        {
            choosed = true;
            GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Add(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile);
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
    

}
