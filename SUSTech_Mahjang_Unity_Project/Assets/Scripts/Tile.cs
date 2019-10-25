using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    /// <summary>
    /// move is a enum class representing the move direction of tile
    /// </summary>
    enum move
    {
        upwords,
        downwords,
        wait
    };
    
    /// <summary>
    /// represent the range of the movement of tile
    /// </summary>
    Vector3 up_position,down_position;
    
    /// <summary>
    ///the speed of the tile movement  
    /// </summary>
    public int MoveSpeed = 5;
    
    /// <summary>
    /// represent whether the tile is clicked
    /// </summary>
    private bool isclick=false;
    
    /// <summary>
    /// initial state of tile
    /// </summary>
    private move status = move.downwords;
    
    

    // Start is called before the first frame update
    void Start()
    {
        up_position=transform.position;
        down_position=up_position;
        up_position+=new Vector3(0,0.5f,0);
    }



    private void /// <summary>
    /// Called while the mouse click down over the Collider
    /// </summary>
    /// 
    OnMouseDown()
    {
        if (isclick)
        {
            isclick = false;
        }
        else
        {
            isclick = true;
        }
        Debug.Log("click down");
    }



    private void /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    OnMouseEnter()
    {   
        if(!isclick)
        status = move.upwords;
    }

    private void /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    OnMouseExit()
    {
        if(!isclick)
        status = move.downwords;
    }
    // Update is called once per frame
    void Update()
    {
        if(isclick){
            ///after clicking ,the tile will move upward more quickly
            transform.position = Vector3.MoveTowards(transform.position, up_position, 2*MoveSpeed * Time.deltaTime);
        }
        else if(status == move.upwords)
        {
            transform.position = Vector3.MoveTowards(transform.position, up_position, MoveSpeed * Time.deltaTime);
            if(transform.position.y==up_position.y)
                status=move.wait;
        }else if(status == move.downwords)
        {
            transform.position = Vector3.MoveTowards(transform.position, down_position, MoveSpeed * Time.deltaTime);
            if(transform.position.y==down_position.y)
                status=move.wait;        
        }
    }
}
