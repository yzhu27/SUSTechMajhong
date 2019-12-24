using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.Web;
using Assets.Scripts.GameMain;

public class LogIn : MonoBehaviour
{
    WebController webController;
    bool set = false;
    bool enter = false;
    
    void Start()
    {
        webController = GameObject.Find("WebController").GetComponent<WebController>();
    }
    public void logIn()
    {
        webController.w.SignUp(GameObject.Find("Text").GetComponent<Text>().text,Loadscene);
        AutoCallBacks.self = new Assets.Scripts.Util.User(GameObject.Find("Text").GetComponent<Text>().text);
    }

   public void Loadscene(bool succeed, string sender,string msg  )
    {
        if (succeed)
        {
            enter = true;
        }
        else
        {
            set = true;
        }
        
    }

    

    void Update()
    {
        if (set)
        {
            GameObject.Find("Placeholder").GetComponent<Text>().text = "The username has been registered";
            GameObject.Find("Text").GetComponent<Text>().text = "";
            set = false;
        }
        if (enter)
        {
            
            SceneManager.LoadScene("Room");
            enter = false;
        }
    }
}
