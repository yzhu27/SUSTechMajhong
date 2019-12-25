using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTileFront : MonoBehaviour
{

    public void addTileFront(string path)
    {

        Texture2D tex = (Texture2D)Resources.Load(path);
        SpriteRenderer spr = GetComponentsInChildren<Transform>()[1].GetComponent<SpriteRenderer>();
        Sprite sp = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        spr.sprite = sp;
        Transform tr = GetComponentsInChildren<Transform>()[1].GetComponent<Transform>();
        float x = 2.9f;
        float y = 3.4f;
        tr.localScale = new Vector3(x / tex.width, y / tex.height, 1);
    }

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
