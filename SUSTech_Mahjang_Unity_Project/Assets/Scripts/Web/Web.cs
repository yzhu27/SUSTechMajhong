using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Scripts.Web
{
	public class Web
	{
		private StompClient client;
		private bool autoLog;

		public Web(Uri uri, bool auto_log = true)
		{
			autoLog = auto_log;
			client = new StompClient(uri, OnMessage, autoLog);
		}

		public void Connect()
		{
			client.Connect();
		}
		
		public void Disconnect()
		{
			client.DisConnect();
		}

		public void test_subs()
		{
			client.Subscribe("/topic/echo");
		}

		public void test_send()
		{
			client.Send("/app/echo", "{\"sender\":\"Unity\",\"type\":\"CHAT\",\"content\":\"ko no DIO da!!!\"}");
		}

		/// <summary>
		/// use in WebController OnUpdate
		/// </summary>
		public void OnUpdate()
		{
			client.SendMessage();
		}

		/// <summary>
		/// message handler function
		/// </summary>
		/// <param name="msg">message received</param>
		public void OnMessage(string msg)
		{
			var received = new StompFrame(msg);

			if (autoLog)
			{
				switch (received.GetServerCommand())
				{
					case ServerCommand.CONNECTED:
					case ServerCommand.MESSAGE:
						Debug.Log("===> " + msg);
						break;
					case ServerCommand.ERROR:
						Debug.LogWarning("===> " + msg);
						break;
					case ServerCommand.RECEIPT:
						break;
				}
			}

			// var j = JsonConvert.DeserializeObject(received.data);
		}


	}
}
