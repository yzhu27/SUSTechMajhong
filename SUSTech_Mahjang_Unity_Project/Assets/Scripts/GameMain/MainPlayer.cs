using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameMain
{
	/// <summary>
	/// 与Player类不同，MainPlayer类的方法
	/// 全部由本地脚本来调用，服务器不会主
	/// 动调用这些方法，除非被作为服务器回
	/// 调函数的委托。
	/// </summary>
	public class MainPlayer : Player
	{
		private Dictionary<Action, HashSet<Tile>> cache;

		/// <summary>
		/// 当前客户端自身的玩家对象
		/// </summary>
		public MainPlayer() { }

		#region Functions to implement

		/// <summary>
		/// 抽牌
		/// </summary>
		/// <param name="tile">抽到的牌</param>
		public void Draw(Tile tile)
		{
			hand.Add(tile);
			hand.Sort();

			// call script here
		}

		new public void Play(Tile tile)
		{
			hand.Remove(tile);

			// call script here
		}

		new public void Eat(Tile tile1, Tile tile2)
		{
			hand.Remove(tile1);
			hand.Remove(tile2);

			List<Tile> tiles = new List<Tile>()
			{
				tile1, tile2, lastPlayedTile()
			};
			tiles.Sort();
			onDesk.Add(tiles);

			// call script here
		}

		new public void Touch(Tile tile1, Tile tile2)
		{
			hand.Remove(tile1);
			hand.Remove(tile2);

			List<Tile> tiles = new List<Tile>()
			{
				tile1, tile2, lastPlayedTile()
			};
			tiles.Sort();
			onDesk.Add(tiles);

			// call script here
		}

		new public void Rod(Tile tile1, Tile tile2, Tile tile3)
		{
			hand.Remove(tile1);
			hand.Remove(tile2);
			hand.Remove(tile3);

			List<Tile> tiles = new List<Tile>()
			{
				tile1, tile2, tile3, lastPlayedTile()
			};
			tiles.Sort();
			onDesk.Add(tiles);

			// call script here
		}

		new public void SelfRod(Tile tile)
		{
			hand.Remove(tile);
			onDesk.Add(new List<Tile>() { tile });

			// call script here
		}

		new public void SelfRod(Tile tile1, Tile tile2, Tile tile3, Tile tile4)
		{
			hand.Remove(tile1);
			hand.Remove(tile2);
			hand.Remove(tile3);
			hand.Remove(tile4);

			List<Tile> tiles = new List<Tile>()
			{
				tile1, tile2, tile3, tile4
			};
			tiles.Sort();
			onDesk.Add(tiles);

			// call script here
		}

		#endregion

		/// <summary>
		/// 响应阶段开始时调用，获取可行的操作，
		/// 传入TouchBar即可更新按钮状态
		/// </summary>
		/// <returns></returns>
		public HashSet<Action> GetActionsOnResponce()
		{
			return charactor.GetAvailableResponces(lastPlayedTile(), cache);
		}

		/// <summary>
		/// 出牌阶段开始时调用，获取可行的操作，
		/// 传入TouchBar即可更新按钮状态
		/// </summary>
		/// <returns></returns>
		public HashSet<Action> GetActionsOnPlay()
		{
			return charactor.GetAvailableActions(cache);
		}

	}
}
