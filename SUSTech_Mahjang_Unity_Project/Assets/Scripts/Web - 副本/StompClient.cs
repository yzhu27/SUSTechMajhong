using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using UnityEngine;

namespace Assets.Scripts.Web
{
	delegate void OnMessageHandler(string msg);

	class StompClient
	{
		private Queue<StompFrame> sendList;
		private Uri server;
		private OnMessageHandler handler;

		const int capacity = 4;
		const int maxMessageSize = 1024;

		private ClientWebSocket client;
		private const string acceptVersion = "1.1,1.0";
		private const string heartBeat = "0,0";

		private bool connected;
		private bool autoLog;

		public StompClient(Uri uri, OnMessageHandler handler, bool auto_log = true)
		{
			server = uri;
			this.handler = handler;
			autoLog = auto_log;

			sendList = new Queue<StompFrame>();
			client = new ClientWebSocket();
			connected = false;
		}

		public async Task Connect()
		{
			if (connected)
			{
				Debug.LogWarning("Stomp client already connected");
				return;
			}
			await client.ConnectAsync(server, CancellationToken.None);
			Debug.Log("Websocket connected");

			var msg = new StompFrame(ClientCommand.CONNECT);
			msg.AddHead("accept-version", acceptVersion);
			msg.AddHead("heart-beat", heartBeat);

			sendList.Enqueue(msg);

			_ = HandleWebSocket();
		}

		public async Task DisConnect()
		{
			var msg = new StompFrame(ClientCommand.DISCONNECT);

			sendList.Enqueue(msg);

			await Task.Delay(TimeSpan.FromSeconds(1));
			await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
			connected = false;
		}

		public void Subscribe(string dst)
		{
			var msg = new StompFrame(ClientCommand.SUBSCRIBE);
			msg.AddHead("id", dst + "-" + 0);
			msg.AddHead("destination", dst);

			sendList.Enqueue(msg);
		}

		public void Send(string dst, string content)
		{
			var msg = new StompFrame(ClientCommand.SEND);
			msg.AddHead("destination", dst);
			msg.AddHead("content-length", Encoding.UTF8.GetBytes(content).Length.ToString());
			msg.data = content;

			sendList.Enqueue(msg);
		}

		private async Task HandleWebSocket()
		{
			byte[] sendBuffer;
			byte[] receiveBuffer = new byte[maxMessageSize];

			while (client.State == WebSocketState.Open)
			{
				while (sendList.Count > 0 && connected)
				{
					string sendString = sendList.Dequeue().ToString();
					sendBuffer = Encoding.UTF8.GetBytes(sendString);
					await client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
					if (autoLog) Debug.Log("<== " + sendString);
				}

				WebSocketReceiveResult receiveResult = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

				if (receiveResult.MessageType == WebSocketMessageType.Close)
				{
					await client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
				}
				else if (receiveResult.MessageType == WebSocketMessageType.Binary)
				{
					await client.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept binary frame", CancellationToken.None);
				}
				else
				{
					int count = receiveResult.Count;

					while (receiveResult.EndOfMessage == false)
					{
						if (count >= maxMessageSize)
						{
							string closeMessage = string.Format("Maximum message size: {0} bytes.", maxMessageSize);
							await client.CloseAsync(WebSocketCloseStatus.MessageTooBig, closeMessage, CancellationToken.None);
							return;
						}

						receiveResult = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer, count, maxMessageSize - count), CancellationToken.None);
						count += receiveResult.Count;
					}

					var receivedString = Encoding.UTF8.GetString(receiveBuffer, 0, count);
					if (autoLog) Debug.Log("==> " + receivedString);

					handler(receivedString);
				}
			}
		}
	}
}
