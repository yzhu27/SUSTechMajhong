using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

using Assets.Scripts.GameMain;
using Dist.SpringWebsocket;
using System.Collections.Generic;
 
namespace Assets.Scripts.Web
{
	class WebRequest
	{
		private readonly Client client;

		public WebRequest(Uri uri)
		{
			client = new Client(uri.ToString(), new Receive(delegate (StompFrame frame)
			{
				Debug.Log(frame.Code.ToString() + ":" + frame.Content);
			}));
		}

		public void Connect()
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("login", "DistChen");
			headers.Add("passcode", "pass");
			this.client.Connect(headers, new Receive(delegate (StompFrame frame)
			{
				Debug.Log(frame.Code.ToString() + ":" + frame.Content);
			}));
		}

		public void Subscribe(string topic)
		{
			client.Subscribe(topic, new Receive(delegate (StompFrame frame)
			{
				Debug.Log(frame.Code.ToString() + ":" + frame.Content);
			}));
		}

		public void Send(string topic, string content)
		{
			client.Send(topic, content);
		}

		public void UnSubscribe(string topic)
		{
			client.UnSubscribe(topic);
		}

	}

}
