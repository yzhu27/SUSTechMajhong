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
        if (choosed)
        {
            choosed = false;
            GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles.Remove(tile.GetComponent<TileScript>().tile);
        }
        else
        {
            choosed = true;
            GameObject.Find("HideTiles").GetComponent<HideTiles>().ChoosedTiles.Add(tile.GetComponent<TileScript>().tile);
        }
    }
    public override void OnMouseEnter(GameObject tile)
    {
        if (!choosed)
        {
            tile.transform.rotation = Quaternion.Euler(180f, 0f, 0f);
            tile.transform.position += upDistance - new Vector3(0f, 0.001f, 0f);
            GameObject.Find("Background Camera").GetComponent<AudioManager>().PlayTileEnterSound();
        }
       
    }
    public override void OnMouseExit(GameObject tile)
    {
        if (!choosed)
        {
            tile.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            tile.transform.position -= upDistance - new Vector3(0f, 0.001f, 0f);
        }
            
    }


}

