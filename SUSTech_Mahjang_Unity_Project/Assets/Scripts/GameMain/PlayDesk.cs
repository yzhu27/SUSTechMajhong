﻿using System.Collections;
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
                    Debug.Log("I am " + self_seat);
				}
			}

			if (self_seat < 0)
				Debug.LogError("Can't find MainPlayer");

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
						Debug.LogError("WTF");
                        break;
				}
			}

            first = (Seat)(((int)first + 4 - self_seat) % 4);

			gameState = new GameState(first);

			prepareFinished = true;
		}

		public IEnumerator SetInitTiles(List<Tile> tiles)
		{
			while (!prepareFinished)
				yield return null;

			foreach(Tile tile in tiles)
			{
				self.Draw(tile, true);
				yield return new WaitForSeconds(0.05f);
				next.Draw(true);
				yield return new WaitForSeconds(0.05f);
				opposite.Draw(true);
				yield return new WaitForSeconds(0.05f);
				last.Draw(true);
				yield return new WaitForSeconds(0.05f);
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
				yield return new WaitForSeconds(0.2f);
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
			Debug.Log("Round start: " + roundPlayer.ToString());
            // script what happens to the playdesk when player start round
            if (roundPlayer == Seat.Self)
            {
                webController.Enqueue(new WebEvent(
                "HandTile",
                "HandTile",
                "StartPlay"
                 ));
                webController.Enqueue(new WebEvent(
                "Timer",
                "Timer",
                "startTimer"
                 ));
              
                self.GetActionsOnPlay();
            }
		}

        public void OnSwap()
        {
            if (roundPlayer == Seat.Self)
            {
                self.GetActionsOnPlay();
            }
        }

        public void OnRodDraw()
        {
            if (roundPlayer == Seat.Self)
            {
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
			Debug.Log("Response begin: " + roundPlayer.ToString());
            // script what happens to the playdesk after player plays a tile then Response begin
            if (roundPlayer != Seat.Self)
            {
                self.GetActionsOnResponse();
            }
        }

		/// <summary>
		/// 玩家回合结束时被调用
		/// </summary>
		public void OnFinish()
		{
			Debug.Log("Response finish: " + roundPlayer.ToString());
            // script what happens to the playdesk when round finished without being resopnced
            webController.Enqueue(new WebEvent(
                "OperateBar",
                "OperateBar",
                 "Prohibt"
                 ));
            webController.Enqueue(new WebEvent(
               "HandTile",
               "HandTile",
               "StartProhibit"
                ));
            webController.Enqueue(new WebEvent(
               "HideTiles",
               "HideTiles",
               "StartProhibit"
                ));
            gameState.OnFinish();
		}

		/// <summary>
		/// 玩家被响应后回合结束时调用
		/// </summary>
		/// <param name="Response_player"></param>
		public void OnFinish(Seat Response_player)
		{
			Debug.Log("Responsed by " + Response_player.ToString() + ": " + roundPlayer.ToString());
			// script what happens to the playdesk when player resopnced a tile

			gameState.OnFinish(Response_player);
			OnStart();
		}
	}
}
