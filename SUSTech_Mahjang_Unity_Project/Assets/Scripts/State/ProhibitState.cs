using UnityEngine;
using System.Collections;

public class ProhibitState : State
{
    public ProhibitState (Vector3 position):base(position){
        
    }

    public override void Lightup(GameObject tile)
    {
        tile.GetComponentInParent<Lightuptile>().SendMessage("lightup", 4);
    }
    


}
