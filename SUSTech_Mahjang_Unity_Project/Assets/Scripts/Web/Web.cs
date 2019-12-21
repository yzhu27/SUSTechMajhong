using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Scripts.Web
{
	public delegate void WebCallBack(bool succeed, string sender, string msg);

	public class Web
	{
		private StompClient client;
		private bool autoLog;
		private string userName;
		private string room;
		

		public Web(Uri uri, Dictionary<string, WebCallBack> autoCallBackDict, bool auto_log = true)
		{
			autoLog = auto_log;
			client = new StompClient(uri, autoCallBackDict, autoLog);
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
        }

		public void NextSingleTest()
		{
			client.Send("/app/room.nextSingleTest", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "")));
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

		public void SendStartSignal()
		{
			client.Send("/app/room.ready", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "initialDarwReady", "yes")));
		}

		public void Play(int tile)
		{
			client.Send("/app/room.roundOperation", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "play", tile.ToString())));
		}

		public void Swap(List<int> hand, List<int> hiden)
		{

		}

		public void Shout(string content)
		{
			client.Send("/app/public.sendMessage", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "", content)));
		}

		public void RoomChat(string content)
		{
			client.Send("/app/room.sendMessage", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "", content)));
		}

		public void Eat(int tile1, int tile2)
		{
			client.Send("/app/room.roundOperationResponse", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "eat", tile1 + " " + tile2)));
		}

		public void Touch(int tile1, int tile2)
		{
			client.Send("/app/room.roundOperationResponse", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "touch", tile1 + " " + tile2)));
		}

		public void Rod(int tile1, int tile2, int tile3)
		{
			client.Send("/app/room.roundOperationResponse", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "rod", tile1 + " " + tile2 + " " + tile3)));
		}

		public void Rod(int tile1, int tile2, int tile3, int tile4)
		{
			client.Send("/app/room.roundOperationResponse", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "darkRod", tile1 + " " + tile2 + " " + tile3 + " " + tile4)));
		}

		public void Rod(int tile)
		{
			client.Send("/app/room.roundOperationResponse", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "rod", tile.ToString())));
		}

		public void AddRod(int tile)
		{
			
		}
	}
}
