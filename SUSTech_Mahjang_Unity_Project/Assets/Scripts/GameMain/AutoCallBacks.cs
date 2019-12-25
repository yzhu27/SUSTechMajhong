using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Web;
using Assets.Scripts.Util;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace Assets.Scripts.GameMain
{
	public static class AutoCallBacks
	{
		public static PlayDesk playDesk;

		public static User self;

		public static TileFactory tileFactory;

		public static WebController webController;

		public static readonly Dictionary<string, WebCallBack> AutoCallBackDict = new Dictionary<string, WebCallBack>()
		{
			{"PlayerSeqNum", SetPlayerSeqNum},
			{"Game-Start", GameStart},
			{"PlayerTiles", SetPlayerTiles},
			{"DarkTiles", SetHidenTiles},
			{"CurrentPlayer", CheckCurrentPlayer},
			{"PlayerDraw", PlayerDraw},
			{"Accept-Play", AcceptPlay},
            {"No-one response", AcceptResponseFinish},
			{"eat", AcceptResponseEat},
			{"touch", AcceptResponseTouch},
			{"rod", AcceptResponseRod},
			{"selfRod", AcceptSelfRod},
			{"selfRodDraw", RodDraw},
			{"winnerTiles", PlayerWin}
		};

		public static void SetPlayerSeqNum(bool succeed, string sender, string msg)
		{
            Debug.Log("begin setting player seq");

			string[] splayers = msg.Split(' ');

			if (splayers.Length != 5)
				Debug.LogError("there are " + (splayers.Length - 1) + " players, expected 4");

			Dictionary<Seat, Player> players = new Dictionary<Seat, Player>();
			Seat self_seat;

			for (int i = 0; i < splayers.Length - 1; i++)
			{
                string splayer = splayers[i];

                try
				{
					string name = splayer.Split(',')[0];
					Seat seat = (Seat)int.Parse(splayer.Split(',')[1]);

					if (name == self.name)
					{
						self_seat = seat;
						players.Add(seat, new MainPlayer(playDesk));
					}
					else
						players.Add(seat, new Player(new User(name), playDesk));
				}
				catch (Exception e)
				{
					Debug.LogError("Wrong fomat: " + splayer);
					throw e;
				}
			}
            Debug.Log("seq phrese finish");

			playDesk.SetPlayers(players, (Seat)int.Parse(splayers[4]));

			Debug.Log("Player set down");
		}

		public static void GameStart(bool succeed, string sender, string msg)
		{
			Debug.Log("Server: " + msg);
		}

		public static void SetPlayerTiles(bool succeed, string sender, string msg)
		{
			Debug.Log("Setting player hand tiles");

			string[] stiles = msg.Split(',');
			List<Tile> tiles = new List<Tile>();

			foreach(string stile in stiles)
			{
				if (stile != "")
					tiles.Add(tileFactory.GetTile(int.Parse(stile)));
			}

			if (tiles.Count != 13)
				Debug.LogError("Wrong init tile count " + tiles.Count);

			webController.setInitTiles = tiles;
		}

		public static void SetHidenTiles(bool succeed, string sender, string msg)
		{
			Debug.Log("Setting player hiden tiles");

			string[] stiles = msg.Split(',');
			List<Tile> tiles = new List<Tile>();

			foreach (string stile in stiles)
			{
				if (stile != "")
					tiles.Add(tileFactory.GetTile(int.Parse(stile)));
			}

			// bug
			webController.setInitHiden = tiles;
		}

		public static void CheckCurrentPlayer(bool succeed, string sender, string msg)
		{
			Seat current = playDesk.roundPlayer;

			switch (current)
			{
				case Seat.Self:
					Assert.IsTrue(msg == self.name);
					break;
				case Seat.Next:
					Assert.IsTrue(msg == playDesk.next.name);
					break;
				case Seat.Oppo:
					Assert.IsTrue(msg == playDesk.opposite.name);
					break;
				case Seat.Last:
					Assert.IsTrue(msg == playDesk.last.name);
					break;
			}
		}

		public static void PlayerDraw(bool succeed, string sender, string msg)
		{
            if (sender == "Server" && msg == "")
            {
                Debug.Log("responsed");
                return;
            }
            else if (sender == self.name)
            {
                if (msg != "")
                    playDesk.self.Draw(tileFactory.GetTile(int.Parse(msg)));
            }
            else if (sender == playDesk.next.name)
                playDesk.next.Draw();
            else if (sender == playDesk.opposite.name)
                playDesk.opposite.Draw();
            else if (sender == playDesk.last.name)
                playDesk.last.Draw();
            else
                Debug.LogError("Unknown player " + sender);
		}

		public static void AcceptPlay(bool succeed, string sender, string msg)
		{
			Tile tile = tileFactory.GetTile(int.Parse(msg));

			if (sender == self.name)
			{
				playDesk.self.Play(tile);
			}
			else if(sender == playDesk.next.name)
			{
				playDesk.next.Play(tile);
			}
			else if(sender == playDesk.opposite.name)
			{
				playDesk.opposite.Play(tile);
			}
			else if(sender == playDesk.last.name)
			{
				playDesk.last.Play(tile);
			}
			else
			{
                Debug.LogError("Unknown player " + sender);
			}
		}

        public static void AcceptResponseEat(bool succeed, string sender, string msg)
        {
			string[] slist = msg.Split(' ');

			try
			{
				Assert.IsTrue(slist.Length == 3);

				Tile tile1 = tileFactory.GetTile(int.Parse(slist[0]));
				Tile tile2 = tileFactory.GetTile(int.Parse(slist[1]));
				Tile last_tile = tileFactory.GetTile(int.Parse(slist[2]));
				
				if (sender == self.name)
				{
					playDesk.self.Eat(
						tile1,
						tile2
					);
				}
				else if (sender == playDesk.next.name)
				{
					playDesk.next.Eat(tile1, tile2);
				}
				else if (sender == playDesk.opposite.name)
				{
					playDesk.opposite.Eat(tile1, tile2);
				}
				else if (sender == playDesk.last.name)
				{
					playDesk.last.Eat(tile1, tile2);
				}
				else
				{
					Debug.LogError("Unknown player " + sender);
				}
			}
			catch (Exception)
			{
				Debug.LogError("Received " + msg + " for eat");
			}
        }

        public static void AcceptResponseTouch(bool succeed, string sender, string msg)
        {
			string[] slist = msg.Split(' ');

			try
			{
				Assert.IsTrue(slist.Length == 3);

				Tile tile1 = tileFactory.GetTile(int.Parse(slist[0]));
				Tile tile2 = tileFactory.GetTile(int.Parse(slist[1]));
				Tile last_tile = tileFactory.GetTile(int.Parse(slist[2]));

				if (sender == self.name)
				{
					playDesk.self.Touch(
						tile1,
						tile2
					);
				}
				else if (sender == playDesk.next.name)
				{
					playDesk.next.Touch(tile1, tile2);
				}
				else if (sender == playDesk.opposite.name)
				{
					playDesk.opposite.Touch(tile1, tile2);
				}
				else if (sender == playDesk.last.name)
				{
					playDesk.last.Touch(tile1, tile2);
				}
				else
				{
					Debug.LogError("Unknown player " + sender);
				}
			}
			catch (Exception)
			{
				Debug.LogError("Received " + msg + " for touch");
			}
		}

        public static void AcceptResponseRod(bool succeed, string sender, string msg)
        {
			string[] slist = msg.Split(' ');

			try
			{
				Assert.IsTrue(slist.Length == 4);

				Tile tile1 = tileFactory.GetTile(int.Parse(slist[0]));
				Tile tile2 = tileFactory.GetTile(int.Parse(slist[1]));
				Tile tile3 = tileFactory.GetTile(int.Parse(slist[2]));
				Tile last_tile = tileFactory.GetTile(int.Parse(slist[3]));

				if (sender == self.name)
				{
					playDesk.self.Rod(
						tile1,
						tile2,
						tile3
					);
				}
				else if (sender == playDesk.next.name)
				{
					playDesk.next.Rod(tile1, tile2, tile3);
				}
				else if (sender == playDesk.opposite.name)
				{
					playDesk.opposite.Rod(tile1, tile2, tile3);
				}
				else if (sender == playDesk.last.name)
				{
					playDesk.last.Rod(tile1, tile2, tile3);
				}
				else
				{
					Debug.LogError("Unknown player " + sender);
				}
			}
			catch (Exception)
			{
				Debug.LogError("Received " + msg + " for rod");
			}
		}

        public static void AcceptResponseFinish(bool succeed, string sender, string msg)
        {
            playDesk.OnFinish();
        }

		public static void AcceptSelfRod(bool succeed, string sender, string msg)
		{
			Tile tile = tileFactory.GetTile(int.Parse(msg));

			if (sender == self.name)
			{
				playDesk.self.SelfRod(tile);
			}
			else if (sender == playDesk.next.name)
			{
				playDesk.next.SelfRod(tile);
			}
			else if (sender == playDesk.opposite.name)
			{
				playDesk.opposite.SelfRod(tile);
			}
			else if (sender == playDesk.last.name)
			{
				playDesk.last.SelfRod(tile);
			}
			else
			{
				Debug.LogError("Unknown player " + sender);
			}
		}

		public static void RodDraw(bool succeed, string sender, string msg)
		{
			if (sender == self.name)
			{
				playDesk.self.RodDraw(tileFactory.GetTile(int.Parse(msg)));
				if (playDesk.roundPlayer != Seat.Self)
				{
					playDesk.OnFinish(Seat.Self);
				}
			}
			else if (sender == playDesk.next.name)
			{
				playDesk.next.RodDraw();
				if (playDesk.roundPlayer != Seat.Self)
				{
					playDesk.OnFinish(Seat.Next);
				}
			}
			else if (sender == playDesk.opposite.name)
			{
				playDesk.opposite.RodDraw();
				if (playDesk.roundPlayer != Seat.Self)
				{
					playDesk.OnFinish(Seat.Oppo);
				}
			}
			else if (sender == playDesk.last.name)
			{
				playDesk.last.RodDraw();
				if (playDesk.roundPlayer != Seat.Self)
				{
					playDesk.OnFinish(Seat.Last);
				}
			}
			else
				Debug.LogError("Unknown player " + sender);
		}

		public static void PlayerWin(bool succeed, string sender, string msg)
		{
			List<Tile> hand = new List<Tile>();

			string[] shand = msg.Split(' ');

			try
			{
				foreach(string stile in shand)
				{
					hand.Add(tileFactory.GetTile(int.Parse(stile)));
				}
			}
			catch(Exception e)
			{
				Debug.LogError(e);
			}

			if (sender == self.name)
			{
				playDesk.self.Win();
			}
			else if (sender == playDesk.next.name)
			{
				playDesk.next.Win(hand);
			}
			else if (sender == playDesk.opposite.name)
			{
				playDesk.opposite.Win(hand);
			}
			else if (sender == playDesk.last.name)
			{
				playDesk.last.Win(hand);
			}
			else
				Debug.LogError("Unknown player " + sender);
		}
	}
}
