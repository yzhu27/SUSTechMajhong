using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Web;
using Assets.Scripts.GameMain;

public class WebController : MonoBehaviour
{
	private Web w;
	public List<Tile> setInitTiles;
	public List<Tile> setInitHiden;

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

	public void StartSetInitTiles(List<Tile> tiles)
	{
		Debug.Log("here");

		StartCoroutine(AutoCallBacks.playDesk.SetInitTiles(tiles));
	}

	public void StartSetHidenTiles(List<Tile> tiles)
	{
		StartCoroutine(AutoCallBacks.playDesk.SetInitHiden(tiles));
	}

	public void TestClick()
	{
        w.SingleTest("a", "1");
	}
}
