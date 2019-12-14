using System.Collections.Generic;

namespace Assets.Scripts.GameMain
{
	/// <summary>
	/// 游戏内的主要物件, 每个<c>PlayDesk</c>包含四个<c>Player</c>对象
	/// </summary>
	public class PlayDesk
	{
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

		public PlayDesk() {
            self = new MainPlayer();
            last = new Player();
            opposite = new Player();
            next = new Player();
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
