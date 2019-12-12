using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Web;

public class WebController : MonoBehaviour
{
	private Web w;

    // Start is called before the first frame update
    void Start()
    {
		w = new Web(new System.Uri("ws://10.21.63.106:20000/ws/websocket"));
		w.Connect();
		w.test_subs();
    }

    // Update is called once per frame
    void Update()
    {
        w.OnUpdate();
    }

	private void OnDestroy()
	{
		w.Disconnect();
	}

	public void TestClick()
	{
		w.test_send();
	}
}
