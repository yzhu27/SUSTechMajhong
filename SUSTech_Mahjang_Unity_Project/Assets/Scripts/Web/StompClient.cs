using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

namespace Assets.Scripts.Web
{
	delegate void OnMessageHandler(string msg);

	class StompClient
	{
		private Queue<StompFrame> sendList;

		private WebSocket client;
		private Dictionary<string, OnMessageHandler> subscribes;
		private const string acceptVersion = "1.1,1.0";
		private const string heartBeat = "0,0";

		private bool autoLog;

		public StompClient(Uri uri, bool auto_log = true)
		{
			autoLog = auto_log;

			sendList = new Queue<StompFrame>();
			client = new WebSocket(uri.ToString());
            subscribes = new Dictionary<string, OnMessageHandler>();
		}

		public void Connect()
		{
			if (client.IsAlive)
			{
				Debug.LogWarning("Stomp client already connected");
				return;
			}
			client.Connect();
			Debug.Log("Websocket connected");

			var msg = new StompFrame(ClientCommand.CONNECT);
			msg.AddHead("accept-version", acceptVersion);
			msg.AddHead("heart-beat", heartBeat);

			client.Send(msg.ToString());

			client.OnMessage += (sender, e) => Distributer(e.Data);
		}

		public void DisConnect()
		{
			var msg = new StompFrame(ClientCommand.DISCONNECT);

			client.Send(msg.ToString());

			client.Close();
		}

		public void SendMessage()
		{
			if (sendList.Count > 0)
			{
				string sendString = sendList.Dequeue().ToString();
				client.Send(sendString);
				if (autoLog) Debug.Log("<== " + sendString);
			}
		}

		public void Subscribe(string dst, OnMessageHandler handler)
		{
			var msg = new StompFrame(ClientCommand.SUBSCRIBE);
			msg.AddHead("id", dst + "-" + 0);
			msg.AddHead("destination", dst);

			Enqueue(msg);
			subscribes.Add(dst, handler);
		}

		public void Send(string dst, string content)
		{
			var msg = new StompFrame(ClientCommand.SEND);
			msg.AddHead("destination", dst);
			msg.AddHead("content-length", Encoding.UTF8.GetBytes(content).Length.ToString());
			msg.data = content;

			Enqueue(msg);
		}

		private void Enqueue(StompFrame msg)
		{
			sendList.Enqueue(msg);
			if (autoLog)
				Debug.Log(sendList.Count + " messages in queue");
		}

		private void Distributer(string msg)
		{
			StompFrame stomp = new StompFrame(msg);
            if (autoLog)
            {
                switch (stomp.GetServerCommand())
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
                    default:
                        break;
                }
            }
			subscribes[stomp.GetHead("destination")](stomp.data);
		}
	}
}
