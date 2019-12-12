using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightuptile : MonoBehaviour
{
    public void lightup(int situation)
    {
        switch (situation)
        {
            case 1:
                gameObject.GetComponentInParent< MeshRenderer > ().material = (Material)Resources.Load("commonTile");
                break;
            case 2:
                gameObject.GetComponentInParent<MeshRenderer>().material = (Material)Resources.Load("Touchable");
                break;
            case 3:
                gameObject.GetComponentInParent<MeshRenderer>().material = (Material)Resources.Load("eatable");
                break;
            case 4:
                gameObject.GetComponentInParent<MeshRenderer>().material = (Material)Resources.Load("prohibited");
                break;
        }

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
