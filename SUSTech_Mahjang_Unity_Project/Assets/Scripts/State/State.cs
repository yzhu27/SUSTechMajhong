using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{

    public Vector3 upPosition;
    public Vector3 downPosition;
    public Vector3 upDistance = new Vector3(0, 0.015f, 0);
    public State(Vector3 position)
    {
        upPosition = position + upDistance;
        downPosition = position;
    }
    public virtual void OnMouseDown(GameObject tile)
    {

    }
    public virtual void OnMouseEnter(GameObject tile)
    {
        
    }
    public virtual void OnMouseExit(GameObject tile)
    {

    }
    public virtual void Lightup(GameObject tile)
    {

    }
}
