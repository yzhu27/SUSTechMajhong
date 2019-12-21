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
			{"Accept-Play", AcceptPlay}
		};

		public static void SetPlayerSeqNum(bool succeed, string sender, string msg)
		{
			string[] splayers = msg.Split(' ');
			if (splayers.Length != 4)
				throw new ArgumentException("there are " + splayers.Length + " players, expected 4");

			Dictionary<Seat, Player> players = new Dictionary<Seat, Player>();
			Seat self_seat;

			foreach (string splayer in splayers)
			{
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

			Assert.IsTrue(players.Count == 4, "4 players got");

			playDesk.SetPlayers(players);

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

			Assert.IsTrue(tiles.Count == 12);

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
			playDesk.self.Draw(tileFactory.GetTile(int.Parse(msg)));
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
				throw new ArgumentException("Unknown player " + sender);
			}
		}
	}
}
