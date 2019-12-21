using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.GameMain
{
	public enum Seat
	{
		/// <summary>
		/// 自己
		/// </summary>
		Self = 0,
		/// <summary>
		/// 下家
		/// </summary>
		Next = 1,
		/// <summary>
		/// 对家
		/// </summary>
		Oppo = 2,
		/// <summary>
		/// 上家
		/// </summary>
		Last = 3
	}

	enum Stage
	{
		Main,
		Responce
	}

	class GameState
	{
		/// <summary>
		/// 当前回合数
		/// </summary>
		public uint round { get; private set; }

		/// <summary>
		/// 当前玩家座次
		/// </summary>
		public Seat turn { get; private set; }

		/// <summary>
		/// 当前回合阶段
		/// </summary>
		public Stage stage { get; private set; }

		/// <summary>
		/// 响应阶段所响应的牌，不在响应阶段则为<c>null</c>
		/// </summary>
		public Tile lastPlayedTile { get; private set; }

		public GameState(Seat firstPlayer, uint round = 0, Stage stage = Stage.Main)
		{
			this.round = round;
			turn = firstPlayer;
			this.stage = stage;
			lastPlayedTile = null;

            Debug.Log("Round " + round + ", player " + turn.ToString() + " stage " + stage);
        }

		/// <summary>
		/// 打出牌时调用
		/// </summary>
		public void OnPlay(Tile tile)
		{
			if (stage != Stage.Main)
            {
                Debug.LogError("can not play on stage " + stage.ToString());
            }

			stage = Stage.Responce;

			lastPlayedTile = tile;

            Debug.Log("Round " + round + ", player " + turn.ToString() + " stage " + stage);
		}

		/// <summary>
		/// 回合结束调用(无玩家响应)
		/// </summary>
		public void OnFinish()
		{
			if (turn == Seat.Last)
				OnFinish(Seat.Self);
			else
				OnFinish(turn + 1);

            Debug.Log("Round " + round + ", player " + turn.ToString() + " stage " + stage);
        }

		/// <summary>
		/// 回合结束调用(响应结算完毕)
		/// </summary>
		/// <param name="responce_player">做出响应的玩家</param>
		public void OnFinish(Seat responce_player)
		{
			lastPlayedTile = null;

			round++;

			turn = responce_player;

			stage = Stage.Main;

            Debug.Log("Round " + round + ", player " + turn.ToString() + " stage " + stage);
        }
	}
}
