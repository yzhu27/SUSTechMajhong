using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Web;
using Assets.Scripts.GameMain;

public class WebEvent
{
	public readonly string gameObject;
	public readonly string component;
	public readonly string function;
	public readonly object param;
	public readonly bool isCoroutine;

	public WebEvent(string gameObject, string component, string function, object param = null, bool isCoroutine = false)
	{
		this.gameObject = gameObject;
		this.component = component;
		this.function = function;
		this.param = param;
		this.isCoroutine = isCoroutine;
	}
}

public class WebController : MonoBehaviour
{
	public readonly Web w = new Web(new System.Uri("ws://10.21.34.58:20000/ws/websocket"), AutoCallBacks.AutoCallBackDict);

	public List<Tile> setInitTiles;
	public List<Tile> setInitHiden;

	private Queue<WebEvent> webEvents;
	public bool register = true;
    private bool inGame = false;

     void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
		w.Connect();
		webEvents = new Queue<WebEvent>();
		AutoCallBacks.webController = this;
    }

    // Update is called once per frame
    void Update()
    {
		if (!register)
		{
			PlayDesk playDesk = GameObject.Find("GameManager").GetComponent<GameManager>().playDesk;
			playDesk.webController = this;
            register = true;
            inGame = true;
            w.LoadReady();
            Debug.Log("regested");
        }

        w.OnUpdate();

        if (!inGame)
        {
            Dispatch();
            return;
        }
        else
        {
            if (GameObject.Find("GameManager").GetComponent<GameManager>().gameStatus == GameStatus.Started)
            {
                Dispatch();
            }
            else if (GameObject.Find("GameManager").GetComponent<GameManager>().gameStatus == GameStatus.Waiting)
            {
                if (setInitTiles != null)
                {
                    StartCoroutine(AutoCallBacks.playDesk.SetInitTiles(setInitTiles));
                    setInitTiles = null;
                }
                else if (setInitHiden != null)
                {
                    StartCoroutine(AutoCallBacks.playDesk.SetInitHiden(setInitHiden));
                    setInitHiden = null;
                }
            }
        }
		
    }

    private void Dispatch()
    {
        if (webEvents.Count > 0)
        {
            var e = webEvents.Dequeue();

            if (e.isCoroutine)
            {
                MonoBehaviour mono = (MonoBehaviour)GameObject.Find(e.gameObject).GetComponent(e.component);

                if (e.param != null)
                    mono.StartCoroutine(e.function, e.param);

                else
                    mono.StartCoroutine(e.function);
            }
            else
            {
                if (e.param != null)
                {
                    Debug.Log(e.gameObject);
                    GameObject.Find(e.gameObject).GetComponent(e.component).SendMessage(e.function, e.param);
                }
                else
                    GameObject.Find(e.gameObject).GetComponent(e.component).SendMessage(e.function);
            }
        }
    }

	private void OnDestroy()
	{
		w.Disconnect();
	}
    public void unregisterd()
    {
        register = false;
    }
	public void Enqueue(WebEvent webEvent)
	{
		webEvents.Enqueue(webEvent);
	}

	public void TestClick()
	{
		if (GameObject.Find("GameManager").GetComponent<GameManager>().gameStatus == GameStatus.Preparing)
			w.SingleTest("a", "1");
		else if (GameObject.Find("GameManager").GetComponent<GameManager>().gameStatus == GameStatus.Started)
			w.NextSingleTest();
	}

}
