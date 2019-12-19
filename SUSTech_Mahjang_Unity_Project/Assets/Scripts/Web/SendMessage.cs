using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Web
{
	class SendMessage
	{
		public string sender { set; get; }
		public string room { set; get; }
		public string type { set; get; }
		public string content { set; get; }

		public SendMessage(string sender, string room, string type, string content = "")
		{
			this.sender = sender;
			this.room = room;
			this.type = type;
			this.content = content;
		}
	}
}
