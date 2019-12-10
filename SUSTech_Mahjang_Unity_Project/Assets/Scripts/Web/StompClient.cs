using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace Assets.Scripts.Web
{
	class StompClient
	{
		private ClientWebSocket client;
		private const string acceptVersion = "1.1,1.0";
		private const string heartBeat = "0,0";
		private System.Threading.CancellationToken CancellationToken = new System.Threading.CancellationToken(false);
		public StompClient(Uri uri)
		{
			client = new ClientWebSocket();
			client.ConnectAsync(uri, CancellationToken);
		}

		public async Task Connect()
		{
			var msg = new StompFrame(ClientCommand.CONNECT);
			msg.AddHead("accept-version", acceptVersion);
			msg.AddHead("heart-beat", heartBeat);
			await SendWebSocket(msg);
		}


		private async Task SendWebSocket(StompFrame msg)
		{
			await client.SendAsync(
				new ArraySegment<byte>(Encoding.Default.GetBytes(msg.ToString())),
				WebSocketMessageType.Text,
				false,
				CancellationToken
			);
		}
	}
}
