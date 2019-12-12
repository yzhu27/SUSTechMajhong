using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Web
{
	public class Web
	{
		private StompClient client;

		public Web(Uri uri, bool auto_log = true)
		{
			client = new StompClient(uri, OnMessage, true);
			
		}

		public async Task test()
		{
			await client.Connect();
			client.Subscribe("/topic/echo");
			//for (int i = 0; i < 10; i++)
			//{
			//	client.Send("/app/echo", "{\"sender\":\"Unity\",\"type\":\"CHAT\",\"content\":\"ko no DIO da!!!\"}");
			//	await Task.Delay(TimeSpan.FromSeconds(0.2));
			//}
			//await Task.Delay(TimeSpan.FromSeconds(5));
			//await client.DisConnect();
		}

		public void test_send()
		{
			client.Send("/app/echo", "{\"sender\":\"Unity\",\"type\":\"CHAT\",\"content\":\"ko no DIO da!!!\"}");
		}

		public void OnMessage(string msg)
		{
			_ = new StompFrame(msg);
		}

	}
}
