using UnityEngine;
using System.Collections;

public class EatState: State
{

    private bool choosed;
    public EatState(Vector3 position) : base(position)
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
            GameObject.Find("HandTile").GetComponent<HandTile>().StartEat();
        }
    }
    public override void OnMouseEnter(GameObject tile)
    {
        if (!choosed)
        {
            upPosition = tile.transform.position + upDistance;
            tile.transform.position = upPosition;
            GameObject.Find("Background Camera").GetComponent<AudioManager>().PlayTileEnterSound();
        }
            
    }
    public override void OnMouseExit(GameObject tile)
    {
        if (!choosed)
        {
            GameObject.Find("HandTile").GetComponent<HandTile>().CheckPositions();
            tile.transform.position = new Vector3(tile.transform.position.x, 3.024994f, tile.transform.position.z);
        }
        
    }
    public override void Lightup(GameObject tile)
    {
        tile.GetComponentInParent<Lightuptile>().SendMessage("lightup", 3);
    }

}
