using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;

public class TileScript : MonoBehaviour
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
    [SerializeField]
    float upDistance = 0.015f;

    /// <summary>
    ///the speed of the tile movement  
    /// </summary>
    public int MoveSpeed = 5;

    /// <summary>
    /// represent whether the tile is clicked
    /// </summary>
    public Tile tile = new Tile();
    
    /// <summary>
    /// initial state of tile
    /// </summary>
    private move status = move.downwords;

  
    

    // Start is called before the first frame update
    void Start()
    {
        up_position=transform.position;
        down_position=up_position;
        up_position+=new Vector3(0,upDistance,0);
        

        //GetComponentsInChildren<Transform>()[1].GetComponent< SpriteRenderer > ().sprite =Resources.Load("1");


    }


    float t1;
    float t2;
    private void /// <summary>
    /// Called while the mouse click down over the Collider
    /// </summary>
    /// 
    OnMouseDown()
    {
        if (tile.IsChoosed())
        {
            tile.Unchoose();
            gameObject.GetComponent<MeshRenderer>().material = (Material)Resources.Load("commonTile");
        }
        else
        {
            tile.Choose();
            gameObject.GetComponent<MeshRenderer>().material = (Material)Resources.Load("lightingTile");
        }
        Debug.Log("click down");

        t2 = Time.realtimeSinceStartup;
        if (t2 - t1 < 0.2)
        {
            GetComponentInParent<HandTile>().RemoveTile();
        }
        t1 = t2;
        
    }
   
    //IEnumerator Example()
    //{
    //    print(Time.time);
    //    yield return new WaitForSeconds(5);
    //    print(Time.time);
    //}
    //private void OnMouseDrag()
    //{
    //    Debug.Log("dragging");
    //    StartCoroutine(Example());
    //}


    private void /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    OnMouseEnter()
    {   
        if(!tile.IsChoosed())
        status = move.upwords;
    }

    private void /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    OnMouseExit()
    {
        if(!tile.IsChoosed())
        status = move.downwords;
    }

    public void AddTileFront(int TileId)
    {
        tile.setid(TileId);
        string path = "TileFront/" + TileId.ToString();
        Texture2D tex = (Texture2D)Resources.Load(path);
        SpriteRenderer spr = GetComponentsInChildren<Transform>()[1].GetComponent<SpriteRenderer>();
        Sprite sp = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        spr.sprite = sp;
        Transform tr = GetComponentsInChildren<Transform>()[1].GetComponent<Transform>();
        float x = 2.9f;
        float y = 3.4f;
        tr.localScale = new Vector3(x / tex.width, y / tex.height, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(tile.IsChoosed())
        {
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
