using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
		public MainPlayer() {
            cache = new Dictionary<Action, HashSet<Tile>>();
        }

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
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("AddTile", tile);
        }

		new public void Play(Tile tile)
		{
			hand.Remove(tile);
            // call script here
            Debug.Log(tile);
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveSingleTile", tile);
        }

		new public void Eat(Tile tile1, Tile tile2)
		{
			hand.Remove(tile1);
			hand.Remove(tile2);

			List<Tile> tiles = new List<Tile>()
			{
				tile1, tile2, playDesk.lastPlayedTile
			};
			tiles.Sort();
			onDesk.Add(tiles);

            // call script here
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile", new List<Tile>() { tile1, tile2 });
        }

		new public void Touch(Tile tile1, Tile tile2)
		{
			hand.Remove(tile1);
			hand.Remove(tile2);

			List<Tile> tiles = new List<Tile>()
			{
				tile1, tile2, playDesk.lastPlayedTile
			};
			tiles.Sort();
			onDesk.Add(tiles);

            // call script here
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile", new List<Tile>() { tile1, tile2 });
            
        }

		new public void Rod(Tile tile1, Tile tile2, Tile tile3)
		{
			hand.Remove(tile1);
			hand.Remove(tile2);
			hand.Remove(tile3);

			List<Tile> tiles = new List<Tile>()
			{
				tile1, tile2, tile3, playDesk.lastPlayedTile
			};
			tiles.Sort();
			onDesk.Add(tiles);

            // call script here
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile", new List<Tile>() { tile1, tile2 ,tile3});
        }

		new public void SelfRod(Tile tile)
		{
			hand.Remove(tile);
			onDesk.Add(new List<Tile>() { tile });

            // call script here
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveSingleTile",tile );
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
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile", new List<Tile>() { tile1, tile2,tile3,tile4 });
        }

        public void AddHidden(Tile tile)
        {
            hiden.Add(tile);
            hiden.Sort();
            // call script here
            GameObject.Find("HideTiles").GetComponent<HideTiles>().SendMessage("AddTile", tile);
        }
		#endregion

		/// <summary>
		/// 响应阶段开始时调用，获取可行的操作，
		/// 传入TouchBar即可更新按钮状态
		/// </summary>
		/// <returns></returns>
		public HashSet<Action> GetActionsOnResponse()
		{
			return charactor.GetAvailableResponses(playDesk.lastPlayedTile, cache);
		}

		/// <summary>
		/// 在调用<see cref="GetActionsOnResponse"/>
		/// 后可用，获取可用的牌
		/// </summary>
		/// <param name="action">要进行的行为</param>
		/// <param name="fix">已选中的牌</param>
		/// <returns>可用牌的集合，若为空返回空集合</returns>
		/// <exception cref="ArgumentException"/>
		public HashSet<Tile> GetResponseTiles(Action action, List<Tile> fix)
		{
			if (!cache.ContainsKey(action))
				throw new ArgumentException("Response " + action.ToString() + " is not available");
			var res = charactor.GetAvailableResponseTilesByCache(action, playDesk.lastPlayedTile, fix, cache[action]);
			if (res == null) return new HashSet<Tile>();
			else return res;
		}

		/// <summary>
		/// 牌变动时调用，获取可行的操作，
		/// 传入TouchBar即可更新按钮状态
		/// </summary>
		/// <returns></returns>
		public HashSet<Action> GetActionsOnPlay()
		{
			return charactor.GetAvailableActions(cache);
		}

		/// <summary>
		/// 在调用<see cref="GetActionsOnPlay"/>后可用，获取可用的牌
		/// <para/>
		/// 注意：<see cref="Action.Swap"/> 
		/// 应由脚本自行判断，不要传入此方法
		/// </summary>
		/// <param name="action">要进行的行为</param>
		/// <param name="fix">已选中的牌</param>
		/// <returns>可用牌的集合，若为空返回空集合</returns>
		public HashSet<Tile> GetActionTiles(Action action, List<Tile> fix)
		{
			if (!cache.ContainsKey(action))
				throw new ArgumentException("Action " + action.ToString() + " is not available");
			var res = charactor.GetAvailableActionTilesByCache(action, fix, cache[action]);
			if (res == null) return new HashSet<Tile>();
			else return res;
		}
	}
}
