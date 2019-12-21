using Assets.Scripts.Util;
using Assets.Scripts.GameMain.Charactors;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.GameMain
{
	/// <summary>
	/// 代表玩家的类，包含用户信息，角色和属于玩家的牌
	/// </summary>
	public class Player
	{
		/// <summary>
		/// 玩家对应用户
		/// </summary>
		protected User user;

		/// <summary>
		/// 玩家所选角色
		/// </summary>
		protected Charactor charactor;

		/// <summary>
		/// 玩家所在牌桌
		/// </summary>
		protected PlayDesk playDesk;

		public string name { get => user.name; }

		public Seat seat { get => playDesk.GetSeat(this); }

		/// <summary>
		/// 手牌
		/// </summary>
		public List<Tile> hand { get; protected set; }

		/// <summary>
		/// 暗牌
		/// </summary>
		public List<Tile> hiden { get; protected set; }

		/// <summary>
		/// 碰/吃/杠区
		/// </summary>
		public List<List<Tile>> onDesk { get; protected set; }

		/// <summary>
		/// 打出的牌
		/// </summary>
		public List<Tile> graveyard { get; protected set; }

		// ------------------构造器----------------------

		public Player(PlayDesk playDesk) 
		{
            hand = new List<Tile>();
            hiden = new List<Tile>();
            onDesk = new List<List<Tile>>();
            charactor = new EmptyCharactor(this);
			graveyard = new List<Tile>();
            this.playDesk = playDesk;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="user">该玩家对应的用户</param>
		public Player(User user, PlayDesk playDesk)
		{
			this.user = user;
			hand = new List<Tile>();
            hiden = new List<Tile>();
            onDesk = new List<List<Tile>>();
			graveyard = new List<Tile>();
			charactor = new EmptyCharactor(this);
			this.playDesk = playDesk;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="user">该玩家对应的用户</param>
		/// <param name="charactor">玩家所选角色</param>
		public Player(User user, Charactor charactor)
		{
			this.user = user;
			this.charactor = charactor;
			hand = new List<Tile>();
			onDesk = new List<List<Tile>>();
		}

		// ------------------方法-------------------------

		public string GetCharactorName() => charactor.name;

		public uint GetCharactorId() => charactor.id;

		// ------------------传入-------------------------

		/**网络类会调用这些方法，加注释的
		 * 部分请在脚本中实现并调用
		 */

		/// <summary>
		/// 玩家发言
		/// </summary>
		/// <param name="content">内容</param>
		public void Speak(string content)
		{
			// call script what player do here
		}

		/// <summary>
		/// 播放技能特效
		/// </summary>
		/// <param name="id">技能id</param>
		public void Skill(int id)
		{
			// call script what player do here
		}

		/// <summary>
		/// 抽牌
		/// </summary>
		public void Draw()
		{
            hand.Add(null);
			// call script what other player do here
			playDesk.webController.Enqueue(new WebEvent(
				"TileStack (" + (int)seat + ")",
				"TileStack",
				"RemoveTile"
			));
			playDesk.webController.Enqueue(new WebEvent(
				"HandTile (" + (int)seat + ")",
				"HandTileOthers",
				"Reconstruct"
			));
			//GameObject.Find("TileStack ("+(int)seat+")").GetComponent<TileStack>().SendMessage("RemoveTile");
			//GameObject.Find("HandTile (" + (int)seat + ")").GetComponent<HandTileOthers>().SendMessage("Reconstruct");

			playDesk.OnStart();
        }

		public void RodDraw()
		{
			hand.Add(null);
			// call script what other player do here

		}

		/// <summary>
		/// 换牌
		/// </summary>
		/// <param name="num">交换暗牌的数量</param>
		public void Exchange(int num)
		{
			// call script what other player do here
		}

		/// <summary>
		/// 打出
		/// </summary>
		/// <param name="tile">打出的牌</param>
		public void Play(Tile tile)
		{
			hand.RemoveAt(hand.Count - 1);

            // call script what other player do here
            playDesk.webController.Enqueue(new WebEvent(
                "HandTile (" + (int)seat + ")",
                "HandTileOthers",
                "Reconstruct"
            ));
            playDesk.webController.Enqueue(new WebEvent(
                "lastTile (" + (int)seat + ")",
                "lastTile",
                "SetTile",
                tile
            ));
            /*GameObject.Find("HandTile (" + (int)seat + ")").GetComponent<HandTileOthers>().SendMessage("Reconstruct");
            GameObject.Find("lastTile (" + (int)seat + ")").GetComponent<lastTile>().SendMessage("SetTile",tile);*/
            playDesk.OnPlay(tile);
		}

		/// <summary>
		/// 自杠
		/// </summary>
		/// <param name="tile">cost癞子或校徽</param>
		public void SelfRod(Tile tile)
		{
			hand.RemoveAt(hand.Count - 1);
            List<Tile> tiles = new List<Tile>() { tile };
            onDesk.Add(tiles);
            // call script what other player do here
            playDesk.webController.Enqueue(new WebEvent(
                "HandTile (" + (int)seat + ")",
                "HandTileOthers",
                "Reconstruct"
            ));

            playDesk.webController.Enqueue(new WebEvent(
                "OnDesk (" + (int)seat + ")",
                "OnDesk",
                "AddTiles", 
                tiles
            ));

            /*GameObject.Find("HandTile (" + (int)seat + ")").GetComponent<HandTileOthers>().SendMessage("Reconstruct");
            GameObject.Find("OnDesk (" + (int)seat + ")").GetComponent<OnDesk>().SendMessage("AddTiles",tiles);*/
        }

		/// <summary>
		/// 自杠
		/// </summary>
		/// <param name="tile1">cost手牌1</param>
		/// <param name="tile2">cost手牌2</param>
		/// <param name="tile3">cost手牌3</param>
		/// <param name="tile4">cost手牌4</param>
		public void SelfRod(Tile tile1, Tile tile2, Tile tile3, Tile tile4)
		{
			hand.RemoveRange(hand.Count - 4, 4);
            List<Tile> tiles = new List<Tile>() { tile1, tile2,tile3,tile4 };
            tiles.Sort();
            onDesk.Add(tiles);
            // call script what other player do here
            playDesk.webController.Enqueue(new WebEvent(
               "HandTile (" + (int)seat + ")",
               "HandTileOthers",
               "Reconstruct"
           ));
            playDesk.webController.Enqueue(new WebEvent(
               "OnDesk (" + (int)seat + ")",
               "OnDesk",
               "setUpward",
               false
           ));
            playDesk.webController.Enqueue(new WebEvent(
                "OnDesk (" + (int)seat + ")",
                "OnDesk",
                "AddTiles",
                tiles
            ));
            /*GameObject.Find("HandTile (" + (int)seat + ")").GetComponent<HandTileOthers>().SendMessage("Reconstruct");
            GameObject.Find("OnDesk (" + (int)seat + ")").GetComponent<OnDesk>().upward = false;
            GameObject.Find("OnDesk (" + (int)seat + ")").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
        }

		/// <summary>
		/// 玩家胡牌，游戏结束
		/// </summary>
		public void Win()
		{
			// call script after someone win here
		}

		/// <summary>
		/// 吃
		/// </summary>
		/// <param name="tile1">cost手牌1</param>
		/// <param name="tile2">cost手牌2</param>
		public void Eat(Tile tile1, Tile tile2)
		{
			hand.RemoveAt(hand.Count - 1);
			List<Tile> tiles = new List<Tile>() { tile1, tile2, playDesk.lastPlayedTile };
			tiles.Sort();
			onDesk.Add(tiles);

            // call script what other player do here
            playDesk.webController.Enqueue(new WebEvent(
               "HandTile (" + (int)seat + ")",
               "HandTileOthers",
               "Reconstruct"
           ));

            playDesk.webController.Enqueue(new WebEvent(
                "OnDesk (" + (int)seat + ")",
                "OnDesk",
                "AddTiles",
                tiles
            ));
            /* GameObject.Find("HandTile (" + (int)seat + ")").GetComponent<HandTileOthers>().SendMessage("Reconstruct");
             GameObject.Find("OnDesk (" + (int)seat + ")").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
            playDesk.OnFinish(seat);
		}

		/// <summary>
		/// 碰
		/// </summary>
		/// <param name="tile1">cost手牌1</param>
		/// <param name="tile2">cost手牌2</param>
		public void Touch(Tile tile1, Tile tile2)
		{
			hand.RemoveAt(hand.Count - 1);
			List<Tile> tiles = new List<Tile>() { tile1, tile2, playDesk.lastPlayedTile };
			tiles.Sort();
			onDesk.Add(tiles);

            // call script what other player do here
            playDesk.webController.Enqueue(new WebEvent(
               "HandTile (" + (int)seat + ")",
               "HandTileOthers",
               "Reconstruct"
           ));

            playDesk.webController.Enqueue(new WebEvent(
                "OnDesk (" + (int)seat + ")",
                "OnDesk",
                "AddTiles",
                tiles
            ));
            /*GameObject.Find("HandTile (" + (int)seat + ")").GetComponent<HandTileOthers>().SendMessage("Reconstruct");
            GameObject.Find("OnDesk (" + (int)seat + ")").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
            playDesk.OnFinish(seat);
		}

		/// <summary>
		/// 杠(专指回合外)
		/// </summary>
		/// <param name="tile1">cost手牌1</param>
		/// <param name="tile2">cost手牌2</param>
		/// <param name="tile3">cost手牌3</param>
		public void Rod(Tile tile1, Tile tile2, Tile tile3)
		{
			hand.RemoveAt(hand.Count - 1);
			List<Tile> tiles = new List<Tile>() { tile1, tile2, tile3, playDesk.lastPlayedTile };
			tiles.Sort();
			onDesk.Add(tiles);

            // call script what other player do here
            playDesk.webController.Enqueue(new WebEvent(
               "HandTile (" + (int)seat + ")",
               "HandTileOthers",
               "Reconstruct"
           ));

            playDesk.webController.Enqueue(new WebEvent(
                "OnDesk (" + (int)seat + ")",
                "OnDesk",
                "AddTiles",
                tiles
            ));
            /*GameObject.Find("HandTile (" + (int)seat + ")").GetComponent<HandTileOthers>().SendMessage("Reconstruct");
            GameObject.Find("OnDesk (" + (int)seat + ")").GetComponent<OnDesk>().SendMessage("AddTiles", tiles);*/
            playDesk.OnFinish(seat);
		}
	}
}
