using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;

public class TileScript : MonoBehaviour
{
    /// <summary>
    /// represent whether the tile is clicked
    /// </summary>
    public Tile tile ;

    public State tileState;

    public void SetEatState()
    {
        tileState = new EatState(transform.position);
        Lightup();
    }

    public void SetTouchState()
    {
        tileState = new TouchState(transform.position);
        Lightup();
    }

    public void SetRodState()
    {
        tileState = new RodState(transform.position);
        Lightup();
    }

    public void SetPlayCardState()
    {
        tileState = new PlayCardState(transform.position);
        Lightup();
    }

    public void SetProhibitedState()
    {
        tileState = new ProhibitState(transform.position);
        Lightup();
    }

    public void SetHideState()
    {
        tileState = new HideState(transform.position);
        Lightup();
    }

    public void SetSwapState()
    {
        tileState = new SwapState(transform.position);
        Lightup();
    }

    public void setTile(Tile theTile) => tile = theTile;
   
    // Start is called before the first frame update
    void Start()
    {
        SetProhibitedState();
    }

    public void Lightup()
    {
        tileState.Lightup(gameObject);
    }

    private void /// <summary>
    /// Called while the mouse click down over the Collider
    /// </summary>
    /// 
    OnMouseDown()
    {
        tileState.OnMouseDown(gameObject);            
    }
   
   
    private void /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    OnMouseEnter()
    {
        tileState.OnMouseEnter(gameObject);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySound("Sounds/DM-CGS-21.wav");
    }

    private void /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    OnMouseExit()
    {
        tileState.OnMouseExit(gameObject);
    }

    

    // Update is called once per frame
    void Update()
    {
 

    }
}
