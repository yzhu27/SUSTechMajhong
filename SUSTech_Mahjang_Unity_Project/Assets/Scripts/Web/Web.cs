using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Scripts.Web
{
	public delegate void WebCallBack(bool succeed, string msg);

	public class Web
	{
		private StompClient client;
		private bool autoLog;
		private string userName;
		private string room;

		public Web(Uri uri, bool auto_log = true)
		{
			autoLog = auto_log;
			client = new StompClient(uri, autoLog);
		}

		public void Connect()
		{
			client.Connect();
		}
		
		public void Disconnect()
		{
			client.DisConnect();
		}

		/// <summary>
		/// use in WebController OnUpdate
		/// </summary>
		public void OnUpdate()
		{
			client.SendMessage();
		}

		public void Login(string user_name)
		{
			client.Subscribe("/user/" + user_name + "/chat", Debug.Log);
			userName = user_name;
		}

		public void JoinRoom(string room_name)
		{
			string json = JsonConvert.SerializeObject(new SendMessage(
				userName, room, ""));
			client.Send("/app/room.adduser", json);

			client.Subscribe("/topic/" + room_name, Debug.Log);
			room = room_name;
		}

	}
}
