using Assets.Scripts.GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int time = 15;
    public void startTimer()
    {
        Debug.Log(time);
        InvokeRepeating("changeNumber", 1, 1);
        
    }

    public void stopTimer()
    {
        Debug.Log("use1");
        CancelInvoke("changeNumber");
        Reset();
    }

    public void changeNumber()
    {
        if (time == 0)
        {
            stopTimer();
            List<Tile> temp = GameObject.Find("HandTile").GetComponent<HandTile>().myplayer.hand;
            GameObject.Find("WebController").GetComponent<WebController>().w.Play(temp[temp.Count - 1].id);
            
        }
        GetComponentInParent<Text>().text = time.ToString();
        time--;
    }

    public void Reset()
    {
        GetComponentInParent<Text>().text = "";
        time = 15;
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
