using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GameMain;
using UnityEngine.UI;

public class OperateBar : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetActions(HashSet<Action> actions)
    {

        buttonStatus(GameObject.Find("Button_Eat"), actions.Contains(Action.Eat));
        buttonStatus(GameObject.Find("Button_Touch"), actions.Contains(Action.Touch));
        buttonStatus(GameObject.Find("Button_Rod"), actions.Contains(Action.Rod));
        buttonStatus(GameObject.Find("Button_Win"), actions.Contains(Action.Win));
        buttonStatus(GameObject.Find("Button_Swap"), actions.Contains(Action.Swap));

    }
    public void Prohibt()
    {
        /*buttonStatus(GameObject.Find("Button_Eat"), false);
        buttonStatus(GameObject.Find("Button_Touch"), false);
        buttonStatus(GameObject.Find("Button_Rod"), false);
        buttonStatus(GameObject.Find("Button_Win"), false);
        buttonStatus(GameObject.Find("Button_Swap"), false);*/
    }
    public void RefreshSon()
    {
        GameObject.Find("Button_Eat").GetComponent<Button>().SendMessage("Refresh");
        GameObject.Find("Button_Touch").GetComponent<Button>().SendMessage("Refresh");
        GameObject.Find("Button_Rod").GetComponent<Button>().SendMessage("Refresh");
        GameObject.Find("Button_Swap").GetComponent<Button>().SendMessage("Refresh");
    }
    public void buttonStatus(GameObject button,bool canuse)
    {
        button.GetComponent<Button>().enabled = canuse;
        button.GetComponent<Button>().interactable = canuse;

    }
    void Start()
    {
        Prohibt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
