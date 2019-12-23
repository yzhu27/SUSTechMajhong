using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ready : MonoBehaviour
{
   public void Onclick()
    {
        if(GameObject.Find("Ready_Text").GetComponent<Text>().text == "取消准备")
        {
            GameObject.Find("Ready_Text").GetComponent<Text>().text = "准备";
        }
        else
        {
            GameObject.Find("Ready_Text").GetComponent<Text>().text = "取消准备";
        }
        
    }
}
