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

        public void SingleTest(string user_name, string room)
        {
            Login(user_name);
            JoinRoom(room);
            client.Send("/app/room.addUserSingleTest", JsonConvert.SerializeObject(new SendMessage(
                user_name, room, "")));
            userName = user_name;
            this.room = room;
            client.Subscribe("/topic/echo", (string msg) => { Debug.Log(msg); });
            client.Send("/app/echo", JsonConvert.SerializeObject( new SendMessage(user_name, room, "HI", "???")));
        }


		public void Login(string user_name)
		{
			client.Subscribe("/user/" + user_name + "/chat", (string msg) => { Debug.Log(msg); });
			userName = user_name;
		}

		public void JoinRoom(string room_name)
		{
			client.Subscribe("/topic/" + room_name, (string msg) => { Debug.Log(msg); });
			room = room_name;
		}

	}
}
