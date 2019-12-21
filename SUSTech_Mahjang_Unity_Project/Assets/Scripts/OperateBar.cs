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

        GameObject.Find("Button_Eat").GetComponent<Button>().interactable = actions.Contains(Action.Eat);
        GameObject.Find("Button_Touch").GetComponent<Button>().interactable = actions.Contains(Action.Touch);
        GameObject.Find("Button_Rod").GetComponent<Button>().interactable = actions.Contains(Action.Rod);
        GameObject.Find("Button_Win").GetComponent<Button>().interactable = actions.Contains(Action.Win);
        GameObject.Find("Button_Swap").GetComponent<Button>().interactable = actions.Contains(Action.Swap);

    }

    public void RefreshSon()
    {
        GameObject.Find("Button_Eat").GetComponent<Button>().SendMessage("Refresh");
        GameObject.Find("Button_Touch").GetComponent<Button>().SendMessage("Refresh");
        GameObject.Find("Button_Rod").GetComponent<Button>().SendMessage("Refresh");
        GameObject.Find("Button_Swap").GetComponent<Button>().SendMessage("Refresh");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
