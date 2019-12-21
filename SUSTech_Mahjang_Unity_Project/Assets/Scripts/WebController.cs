﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Web;
using Assets.Scripts.GameMain;


public class WebController : MonoBehaviour
{
	private Web w;
	public List<Tile> setInitTiles;
	public List<Tile> setInitHiden;

	private Queue<WebEvent> webEvents;

    // Start is called before the first frame update
    void Start()
    {
		w = new Web(new System.Uri("ws://10.21.34.58:20000/ws/websocket"), AutoCallBacks.AutoCallBackDict);
		w.Connect();
		AutoCallBacks.webController = this;
    }

    // Update is called once per frame
    void Update()
    {
        w.OnUpdate();

		if (webEvents.Count > 0)
		{
			var e = webEvents.Dequeue();
			
			if (e.isCoroutine)
			{
				MonoBehaviour mono = (MonoBehaviour)GameObject.Find(e.gameObject).GetComponent(e.component);

				if (e.param is object)
					mono.StartCoroutine(e.function, e.param);
				
				else
					mono.StartCoroutine(e.function);
			}
			else
			{
				if (e.param is object)
					GameObject.Find(e.gameObject).GetComponent(e.component).SendMessage(e.function, e.param);
				
				else
					GameObject.Find(e.gameObject).GetComponent(e.component).SendMessage(e.function);
			}
		}

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

	private void OnDestroy()
	{
		w.Disconnect();
	}

	public void Enqueue(WebEvent webEvent)
	{
		webEvents.Enqueue(webEvent);
	}

	public void TestClick()
	{
        w.SingleTest("a", "1");
	}
}
