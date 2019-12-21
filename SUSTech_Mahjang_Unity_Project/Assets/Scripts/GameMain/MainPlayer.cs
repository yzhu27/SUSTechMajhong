using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
		public MainPlayer(PlayDesk playDesk) : base (playDesk) {
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
            playDesk.webController.Enqueue(new WebEvent(
               "TileStack",
               "TileStack",
               "RemoveTile"
           ));

            playDesk.webController.Enqueue(new WebEvent(
                "HandTile",
                "HandTile",
                "AddTile",
                tile
            ));
			/*GameObject.Find("TileStack").GetComponent<TileStack>().SendMessage("RemoveTile");
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("AddTile", tile);*/

			playDesk.OnStart();
		}

		new public void Play(Tile tile)
		{
            Debug.Log("play");
            hand.Remove(tile);
            // call script here
            playDesk.webController.Enqueue(new WebEvent(
                "HandTile",
                "HandTile",
                "RemoveSingleTile",
                tile
            ));

            playDesk.webController.Enqueue(new WebEvent(
                "lastTile",
                "lastTile",
                "SetTile",
                tile
            ));
            /*GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveSingleTile", tile);
            GameObject.Find("lastTile").GetComponent<lastTile>().SetTile(tile);*/

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
            playDesk.webController.Enqueue(new WebEvent(
                "HandTile",
                "HandTile",
                "RemoveTile",
                 new List<Tile>() { tile1, tile2 }
            ));

            playDesk.webController.Enqueue(new WebEvent(
               "OnDesk",
               "OnDesk",
               "AddTiles",
                tiles
           ));

            /* GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile", new List<Tile>() { tile1, tile2 });
             GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
            playDesk.OnFinish(Seat.Self);
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
            playDesk.webController.Enqueue(new WebEvent(
               "HandTile",
               "HandTile",
               "RemoveTile",
                new List<Tile>() { tile1, tile2 }
           ));

            playDesk.webController.Enqueue(new WebEvent(
               "OnDesk",
               "OnDesk",
               "AddTiles",
                tiles
           ));
            /*GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile", new List<Tile>() { tile1, tile2 });
            GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
            playDesk.OnFinish(Seat.Self);
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
            playDesk.webController.Enqueue(new WebEvent(
               "HandTile",
               "HandTile",
               "RemoveTile",
                new List<Tile>() { tile1, tile2, tile3 }
           ));

            playDesk.webController.Enqueue(new WebEvent(
               "OnDesk",
               "OnDesk",
               "AddTiles",
                tiles
           ));
            /* GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile", new List<Tile>() { tile1, tile2 ,tile3});
             GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
            playDesk.OnFinish(Seat.Self);
        }

		new public void SelfRod(Tile tile)
		{
			hand.Remove(tile);
            List<Tile> tiles = new List<Tile>() { tile };
            onDesk.Add(tiles);

            // call script here
            playDesk.webController.Enqueue(new WebEvent(
                "HandTile",
                "HandTile",
                "RemoveSingleTile",
                tile
            ));
            playDesk.webController.Enqueue(new WebEvent(
              "OnDesk",
              "OnDesk",
              "AddTiles",
               tiles
          ));
            /* GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveSingleTile",tile );
             GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
        }

		new public void SelfRod(Tile tile1, Tile tile2, Tile tile3, Tile tile4)
		{
			hand.Remove(tile1);
			hand.Remove(tile2);
			hand.Remove(tile3);
			hand.Remove(tile4);
            List<Tile> tiles = new List<Tile>() { tile1, tile2, tile3, tile4 };
            tiles.Sort();
            onDesk.Add(tiles);

            // call script here
            playDesk.webController.Enqueue(new WebEvent(
               "HandTile",
               "HandTile",
               "RemoveTile",
                tiles
           ));
            playDesk.webController.Enqueue(new WebEvent(
              "OnDesk",
              "OnDesk",
              "setUpward",
               false
          ));
            playDesk.webController.Enqueue(new WebEvent(
              "OnDesk",
              "OnDesk",
              "AddTiles",
               tiles
          ));
            /* GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveTile", tiles);
             GameObject.Find("OnDesk").GetComponent<OnDesk>().upward =false;
             GameObject.Find("OnDesk").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
        }

        public void AddHidden(Tile tile)
        {
            hiden.Add(tile);
            hiden.Sort();
            // call script here
            playDesk.webController.Enqueue(new WebEvent(
               "HideTiles",
               "HideTiles",
               "AddTile",
                tile
           ));
            /*GameObject.Find("HideTiles").GetComponent<HideTiles>().SendMessage("AddTile", tile);*/
        }

        /// <summary>
		/// tile1 来自于 手牌 , tile2 来自于 暗牌
		/// </summary>
		/// <returns></returns>
        public void Swap(Tile tile1, Tile tile2)
        {           
            hiden.Add(tile1);
            hand.Add(tile2);
            hiden.Remove(tile2);
            hand.Remove(tile1);
            hiden.Sort();
            hand.Sort();

            // call script here

            playDesk.webController.Enqueue(new WebEvent(
              "HideTiles",
              "HideTiles",
              "RemoveSingleTile",
               tile2
          ));
            playDesk.webController.Enqueue(new WebEvent(
                "HandTile",
                "HandTile",
                "RemoveSingleTile",
                tile1
            ));
            playDesk.webController.Enqueue(new WebEvent(
              "HideTiles",
              "HideTiles",
              "AddTile",
               tile1
          ));
            playDesk.webController.Enqueue(new WebEvent(
                "HandTile",
                "HandTile",
                "AddTile",
                tile2
            ));
            /*GameObject.Find("HideTiles").GetComponent<HideTiles>().SendMessage("RemoveSingleTile", tile2);
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("RemoveSingleTile", tile1);
            GameObject.Find("HideTiles").GetComponent<HideTiles>().SendMessage("AddTile", tile1);
            GameObject.Find("HandTile").GetComponent<HandTile>().SendMessage("AddTile", tile2);*/
        }
        #endregion

        /// <summary>
        /// 响应阶段开始时调用，获取可行的操作，
        /// 传入TouchBar即可更新按钮状态
        /// </summary>
        /// <returns></returns>
        public void GetActionsOnResponse()
		{
			var actions = charactor.GetAvailableResponses(playDesk.lastPlayedTile, cache);

			if (actions.Contains(Action.Eat) && playDesk.roundPlayer != Seat.Last)
				actions.Remove(Action.Eat);

			playDesk.webController.Enqueue(new WebEvent(
                "OperateBar",
                "OperateBar",
                "SetActions",
                actions
            ));
            //SetActions(charactor.GetAvailableResponses(playDesk.lastPlayedTile, cache));
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
		public void GetActionsOnPlay()
		{
            playDesk.webController.Enqueue(new WebEvent(
                "OperateBar",
                "OperateBar",
                "SetActions",
                charactor.GetAvailableActions(cache)
            ));
            //SetActions(charactor.GetAvailableActions(cache));
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
