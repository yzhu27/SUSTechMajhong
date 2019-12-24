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
            Login();
            
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

        public void SignUp(string user_name, WebCallBack webCallBack)
        {
            client.Subscribe("/user/" + user_name + "/greetings", (string msg) =>
            {
                ReceiveMessage receive = JsonConvert.DeserializeObject<ReceiveMessage>(msg);
                if (receive.type == "Accept-login")
                {
                    webCallBack(true, receive.sender, receive.content);
                    userName = receive.content;
                    Debug.Log("Set user name: " + receive.content);
                    Login();
                }
                else if (receive.type == "Reject-login")
                    webCallBack(false, receive.sender, receive.content);
                else
                    Debug.LogError("Unknown type " + receive.type);

                client.Unsubscribe("/user/" + user_name + "/greeting");
            });
            client.Send("/app/hello/" + user_name, JsonConvert.SerializeObject(new SendMessage(user_name, "", "")));
        }

        public void TryJoinRoom(string room_name, WebCallBack joinCallBack, WebCallBack readyCallBack)
        {
            client.Subscribe("/topic/" + room_name, (string msg) =>
            {
                ReceiveMessage receive = JsonConvert.DeserializeObject<ReceiveMessage>(msg);
                if (receive.type == "Accept-room.addUser")
                {
                    joinCallBack(true, receive.sender, receive.content);
                    JoinRoom(room_name, readyCallBack);
                }
                else if (receive.type == "Reject-room.addUser")
                    joinCallBack(false, receive.sender, receive.content);
                else
                    Debug.LogError("Unknown type " + receive.type);
            });
            client.Send("/app/room.addUser", JsonConvert.SerializeObject(new SendMessage(userName, room_name, "")));
        }

        public void Ready()
        {
            if (room == null)
            {
                throw new Exception("Not joined room yet");
            }
            client.Send("/app/room.ready", JsonConvert.SerializeObject(new SendMessage(userName, room, "")));
        }

		public void LoadReady()
		{
			client.Send("/app/room.ready", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "BackgroundLoadReady")));
		}

		public void Login()
		{
            if (userName == null)
            {
                Debug.LogError("Not registered");
                throw new Exception();
            }
			client.Subscribe("/user/" + userName + "/chat", (string msg) => { Debug.Log(msg); });
		}

		private void JoinRoom(string room_name, WebCallBack webCallBack)
		{
            client.Subscribe("/topic/" + room_name + "/ready", (string msg) => 
            {
                ReceiveMessage receiveMessage = JsonConvert.DeserializeObject<ReceiveMessage>(msg);
                webCallBack(true, receiveMessage.sender, receiveMessage.content);
            });
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

		public void Swap(int hand, int hiden)
		{
            client.Send("/app/room.roundOperation", JsonConvert.SerializeObject(new SendMessage(
                userName, room, "exchange", hand+","+hiden)));
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
			client.Send("/app/room.roundOperation", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "darkRod", tile1 + " " + tile2 + " " + tile3 + " " + tile4)));
		}

		public void Rod(int tile)
		{
			client.Send("/app/room.roundOperation", JsonConvert.SerializeObject(new SendMessage(
				userName, room, "selfRod", tile.ToString())));
		}

		public void AddRod(int tile)
		{
			
		}
	}
}
