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
	public class StompClient
	{
		private ArraySegment<Byte> receive;
		private TimeSpan delay = TimeSpan.FromSeconds(5);
		private ClientWebSocket client;
		private const string acceptVersion = "1.1,1.0";
		private const string heartBeat = "0,0";
		private CancellationToken cancellationToken = new System.Threading.CancellationToken(false);

		public StompClient(Uri uri)
		{
			client = new ClientWebSocket();
			Task c = client.ConnectAsync(uri, cancellationToken);
			c.Wait(delay);
			if (!c.IsCompleted)
			{
				throw new WebSocketException("Failed to establish websocket");
			}
		}

		public async Task Connect()
		{
			var msg = new StompFrame(ClientCommand.CONNECT);
			msg.AddHead("accept-version", acceptVersion);
			msg.AddHead("heart-beat", heartBeat);
			await SendWebSocket(msg);

			await HandleWebSocket(client);
		}

		private async Task HandleWebSocket(WebSocket socket)
		{
			const int maxMessageSize = 1024;
			byte[] receiveBuffer = new byte[maxMessageSize];

			while (socket.State == WebSocketState.Open)
			{
				WebSocketReceiveResult receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

				if (receiveResult.MessageType == WebSocketMessageType.Close)
				{
					await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
				}
				else if (receiveResult.MessageType == WebSocketMessageType.Binary)
				{
					await socket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept binary frame", CancellationToken.None);
				}
				else
				{
					int count = receiveResult.Count;

					while (receiveResult.EndOfMessage == false)
					{
						if (count >= maxMessageSize)
						{
							string closeMessage = string.Format("Maximum message size: {0} bytes.", maxMessageSize);
							await socket.CloseAsync(WebSocketCloseStatus.MessageTooBig, closeMessage, CancellationToken.None);
							return;
						}

						receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer, count, maxMessageSize - count), CancellationToken.None);
						count += receiveResult.Count;
					}

					var receivedString = Encoding.UTF8.GetString(receiveBuffer, 0, count);

					Debug.Log(receivedString);
				}
			}
		}

		public async Task DisConnect()
		{
			var msg = new StompFrame(ClientCommand.DISCONNECT);
			Task t = SendWebSocket(msg);
			t.Wait(1000);
			await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
		}

		public async Task Subscribe(string dst)
		{
			var msg = new StompFrame(ClientCommand.SUBSCRIBE);
			msg.AddHead("id", dst + "-" + 0);
			msg.AddHead("destination", dst);
			await SendWebSocket(msg);
		}

		public async Task Send(string dst, string content)
		{
			var msg = new StompFrame(ClientCommand.SEND);
			msg.AddHead("destination", dst);
			msg.AddHead("content-length", Encoding.UTF8.GetBytes(content).Length.ToString());
			msg.data = content;
			await SendWebSocket(msg);
		}

		private async Task SendWebSocket(StompFrame msg)
		{
			await client.SendAsync(
				new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg.ToString())),
				WebSocketMessageType.Text,
				true,
				CancellationToken.None
			);
		}
	}
}
