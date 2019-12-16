using UnityEngine;
using System.Collections;

public class HideState : State
{

    private bool choosed;
    public HideState(Vector3 position) : base(position)
    {
        choosed = false;
    }

    public override void OnMouseDown(GameObject tile)
    {
       /* if (choosed)
        {
            choosed = false;
            GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Remove(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile);
        }
        else
        {
            choosed = true;
            GameObject.Find("HandTile").GetComponent<HandTile>().ChoosedTiles.Add(tile.GetComponentsInChildren<Transform>()[2].GetComponent<TileScript>().tile);
        }*/
    }
    public override void OnMouseEnter(GameObject tile)
    {
        tile.transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        tile.transform.position += upDistance;
    }
    public override void OnMouseExit(GameObject tile)
    {
        tile.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        tile.transform.position -= upDistance;
    }


}

