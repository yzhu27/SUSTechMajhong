using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assets.Scripts.Web
{
	class StompFrame
	{
		public string data { get; set; }
		private string method;
		private Dictionary<string, string> head;

		public StompFrame(ClientCommand method)
		{
			this.data = "";
			this.method = method.ToString();
			this.head = new Dictionary<string, string>();
		}

		public StompFrame(string received)
		{
			// init
			string[] list = received.Split('\n');
			data = "";
			head = new Dictionary<string, string>();
			// check method
			method = list[0];
			if (!Enum.IsDefined(typeof(ServerCommand), method))
				throw new KeyNotFoundException(method + "is not defined in" + nameof(ServerCommand));
			// add head
			int i = 1;
			while (list[i] != "")
			{
				string[] pair = list[i].Split(":".ToCharArray(), 2);
				head.Add(pair[0], pair[1]);
				i++;
			}
			// add data
			i++;
			while(list[i] != "\u0000")
			{
				data += list[i] + "\n";
				i++;
			}
		}

		public void AddHead(string key, string value)
		{
			head.Add(key, value);
		}

		public ServerCommand GetServerCommand()
		{
			return (ServerCommand)Enum.Parse(typeof(ServerCommand), method);
		}

		public string GetHead(string key)
		{
			return head[key];
		}

		public override string ToString()
		{
			string rtn = "";
			// method
			rtn += method + "\n";
			// head
			foreach(KeyValuePair<string, string> keyValuePair in head)
			{
				rtn += keyValuePair.Key + ":" + keyValuePair.Value + "\n";
			}
			// \n
			rtn += "\n";
			// data
			if (data != "")
				rtn += data;
			// \u0000
			rtn += "\u0000";
			return rtn;
		}
	}
}
