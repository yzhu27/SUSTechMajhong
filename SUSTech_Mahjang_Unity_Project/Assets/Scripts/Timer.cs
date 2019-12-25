using Assets.Scripts.GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private int time = 15;
    bool play = false;
    public void startTimer()
    {
        play = false;
        InvokeRepeating("changeNumber", 1, 1);
        
    }

    public void stopTimer()
    {

        CancelInvoke("changeNumber");
        Reset();
    }

    public void changeNumber()
    {
        if (time == 0 && !play)
        {
            stopTimer();
            List<Tile> temp = GameObject.Find("HandTile").GetComponent<HandTile>().myplayer.hand;
            GameObject.Find("WebController").GetComponent<WebController>().w.Play(temp[temp.Count - 1].id);
            play = true;
            return;
        }
        if (!play)
        {
            GetComponentInParent<Text>().text = time.ToString();
            time--;
        }
        
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
