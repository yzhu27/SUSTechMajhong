using UnityEngine;
using System.Collections;

public class EatState : State
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
        }
        else
        {
            choosed = true;
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
        if(!choosed)
        tile.transform.position = downPosition;
    }
    public override void Lightup(GameObject tile)
    {
        tile.GetComponentInParent<Lightuptile>().SendMessage("lightup", 3);
    }

}
