using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ready : MonoBehaviour
{
    WebController webController;
    public bool newmsg = false;
    public bool newReady = false;
    string[] Usernames ;
    int[] ReadyUser = new int[4];
   public void Onclick()
    {
        if(GameObject.Find("Ready_Text").GetComponent<Text>().text == "取消准备")
        {
            GameObject.Find("Ready_Text").GetComponent<Text>().text = "准备";
        }
        else
        {
            GameObject.Find("Ready_Text").GetComponent<Text>().text = "取消准备";
            webController.w.Ready();
        }
        
    }

    public void Inform(bool succeed, string sender, string msg)
    {
        if (succeed)
        {
            string[] recieve = msg.Split(',');

            Usernames = recieve[0].Split(' ');
            if (!recieve[1].Equals(""))
            {
                string[] names = recieve[1].Split(' ');
                foreach (string ready in names)
                {
                    for (int i = 0; i < Usernames.Length; i++)
                    {
                        if (Usernames[i].Equals(ready))
                        {
                            ReadyUser[i] = 1;
                        }
                    }
                }
                newReady = true;
            }
            newmsg = true;
        }
    }
    public void getReady(bool succeed, string sender, string msg)
    {
        if (succeed)
        {
 ;
            string[] names = msg.Split(' ');
            foreach(string ready in names)
            {
                for(int i = 0; i < Usernames.Length; i++)
                {
                    if (Usernames[i].Equals(ready))
                    {
                        ReadyUser[i] = 1;
                    }
                }
            }
            newReady = true;
        }
    }
    void Start()
    {
        webController = GameObject.Find("WebController").GetComponent<WebController>();
        webController.w.TryJoinRoom("1", Inform, getReady);
    }
    void Update()
    {
        if (newmsg)
        {
            for(int i = 0; i < Usernames.Length; i++)
            {
                
                string player = "Player" + (i + 1).ToString()+"_Text";              
                GameObject.Find(player).GetComponent<Text>().text = Usernames[i];
                
            }
            newmsg = false;
        }
        if (newReady)
        {
            for(int i = 0; i < 4; i++)
            {
               
                if (ReadyUser[i] == 1)
                {
                    string player = "Player" + (i + 1).ToString() ;
                   
                    GameObject.Find(player).GetComponent<Text>().text +="已准备";
                }
                newReady = false;
            }
        }
    }
}
