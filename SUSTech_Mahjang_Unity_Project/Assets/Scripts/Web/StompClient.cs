using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace Assets.Scripts.Web
{
	public class StompClient
	{
		private ClientWebSocket client;
		private const string acceptVersion = "1.1,1.0";
		private const string heartBeat = "0,0";
		private System.Threading.CancellationToken CancellationToken = new System.Threading.CancellationToken(false);
		public StompClient(Uri uri)
		{
			client = new ClientWebSocket();
			Task c = client.ConnectAsync(uri, CancellationToken);
			c.Wait();
		}

		

		public async Task Connect()
		{
			var msg = new StompFrame(ClientCommand.CONNECT);
			msg.AddHead("accept-version", acceptVersion);
			msg.AddHead("heart-beat", heartBeat);
			await SendWebSocket(msg);
		}

		public async Task DisConnect()
		{
			var msg = new StompFrame(ClientCommand.DISCONNECT);
			Task t = SendWebSocket(msg);
			t.Wait(1000);
			await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken);
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
				CancellationToken
			);
		}
	}
}
