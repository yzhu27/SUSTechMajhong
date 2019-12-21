using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.GameMain
{
	/// <summary>
	/// 游戏内的主要物件, 每个<c>PlayDesk</c>包含四个<c>Player</c>对象
	/// </summary>
	public class PlayDesk
	{
		private bool handTileSetted;
		private bool hidenTileSetted;
		public WebController webController { get; set; }

		public bool prepareFinished { get; private set; }
		public bool canStart { get => prepareFinished && handTileSetted && hidenTileSetted; }

		/// <summary>
		/// 自己
		/// </summary>
		public MainPlayer self;

		/// <summary>
		/// 上家
		/// </summary>
		public Player last;

		/// <summary>
		/// 对家
		/// </summary>
		public Player opposite;

		/// <summary>
		/// 下家
		/// </summary>
		public Player next;

		/// <summary>
		/// 每回合操作时限, 单位: 秒
		/// </summary>
		public float roundTimeOut;

		/// <summary>
		/// 每次响应的时限(如碰, 吃等)
		/// </summary>
		public float responseTimeOut;

		/// <summary>
		/// 当前进行回合的玩家座次
		/// </summary>
		public Seat roundPlayer { get => gameState.turn; }

		/// <summary>
		/// 当前可进行响应的玩家座次
		/// </summary>
		public HashSet<Seat> responsePlayers;

		/// <summary>
		/// 最后被打出的牌(不处于响应阶段时为<c>null</c>)
		/// </summary>
		public Tile lastPlayedTile { get => gameState.lastPlayedTile; }

		private GameState gameState;

		public PlayDesk()
		{
			prepareFinished = false;
			handTileSetted = false;
			hidenTileSetted = false;
		}

		public void SetPlayers(Dictionary<Seat, Player> players, Seat first)
		{
			int self_seat = -1;

			foreach (KeyValuePair<Seat, Player> pair in players)
			{
				if (pair.Value is MainPlayer)
				{
					self_seat = (int)pair.Key;
				}
			}

			if (self_seat < 0)
				throw new System.Exception("Can't find MainPlayer");

			foreach (KeyValuePair<Seat, Player> pair in players)
			{
				switch(((int)pair.Key + 4 - self_seat) % 4)
				{
					case 0:
						self = (MainPlayer)pair.Value;
						break;
					case 1:
						next = pair.Value;
						break;
					case 2:
						opposite = pair.Value;
						break;
					case 3:
						last = pair.Value;
						break;
					default:
						throw new System.Exception("WTF");
				}
			}

			Assert.IsNotNull(self, "player self setted");
			Assert.IsNotNull(next, "player next setted");
			Assert.IsNotNull(opposite, "player oppo setted");
			Assert.IsNotNull(last, "player last setted");

			gameState = new GameState(first);

			prepareFinished = true;
		}

		public IEnumerator SetInitTiles(List<Tile> tiles)
		{
			while (!prepareFinished)
				yield return null;

			foreach(Tile tile in tiles)
			{
				self.Draw(tile);
				next.Draw();
				opposite.Draw();
				last.Draw();
				yield return new WaitForSeconds(0.2f);
			}
			handTileSetted = true;
		}

		public IEnumerator SetInitHiden(List<Tile> tiles)
		{
			while (!prepareFinished)
				yield return null;

			foreach (Tile tile in tiles)
			{
				self.AddHidden(tile);
				yield return new WaitForSeconds(10f);
			}
			hidenTileSetted = true;
		}

		public Seat GetSeat(Player player)
		{
			if (ReferenceEquals(player, self))
			{
				return Seat.Self;
			}
			else if (ReferenceEquals(player, next))
			{
				return Seat.Next;
			}
			else if (ReferenceEquals(player, opposite))
			{
				return Seat.Oppo;
			}
			else if (ReferenceEquals(player, last))
			{
				return Seat.Last;
			}
			else
				throw new System.ArgumentException("player not found");
		}

		/// <summary>
		/// 玩家回合开始时被调用
		/// </summary>
		public void OnStart()
		{
            // script what happens to the playdesk when player start round
            if (roundPlayer == Seat.Self)
            {
                webController.Enqueue(new WebEvent(
                "HandTile",
                "HandTile",
                "StartPlay"
            ));
                self.GetActionsOnPlay();
            }
		}

		/// <summary>
		/// 玩家打出牌时被调用
		/// </summary>
		/// <param name="tile"></param>
		public void OnPlay(Tile tile)
		{
            // script what happens to the playdesk before player plays a tile
            
            gameState.OnPlay(tile);
			OnResponse();
		}

		/// <summary>
		/// 响应开始时被调用
		/// </summary>
		public void OnResponse()
		{
            // script what happens to the playdesk after player plays a tile then Response begin
             self.GetActionsOnResponse();

        }

		/// <summary>
		/// 玩家回合结束时被调用
		/// </summary>
		public void OnFinish()
		{
			// script what happens to the playdesk when round finished without being resopnced

			gameState.OnFinish();
			OnStart();
		}

		/// <summary>
		/// 玩家被响应后回合结束时调用
		/// </summary>
		/// <param name="Response_player"></param>
		public void OnFinish(Seat Response_player)
		{
			// script what happens to the playdesk when player resopnced a tile

			gameState.OnFinish(Response_player);
			OnStart();
		}
	}
}
