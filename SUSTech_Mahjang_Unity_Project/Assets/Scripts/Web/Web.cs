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

		public void OnUpdate()
		{
			client.SendMessage();
		}

		public void OnMessage(string msg)
		{
			_ = new StompFrame(msg);
		}


	}
}
