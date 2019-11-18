using Assets.Scripts.Util;
using System;
using System.Collections.Generic;

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
		public User user;

		/// <summary>
		/// 玩家所选角色
		/// </summary>
		public Charactor charactor;

		/// <summary>
		/// 手牌
		/// </summary>
		public List<Tile> hand;

		/// <summary>
		/// 暗牌
		/// </summary>
		public List<Tile> hiden;

		/// <summary>
		/// 碰/吃/杠区
		/// </summary>
		public List<Tile> onDesk;

		/// <summary>
		/// 打出的牌
		/// </summary>
		public List<Tile> graveyard;

		// ------------------构造器----------------------

		public Player() 
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="user">该玩家对应的用户</param>
		public Player(User user)
		{
			this.user = user;
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
		}

		// ------------------一般-------------------------

		/// <summary>
		/// 玩家发言
		/// </summary>
		/// <param name="content">内容</param>
		public void Speak(string content) { }

		/// <summary>
		/// 播放技能特效
		/// </summary>
		/// <param name="id">技能id</param>
		public void Skill(int id) { }

		/// <summary>
		/// 抽牌
		/// </summary>
		public void Draw() { }

		/// <summary>
		/// 换牌
		/// </summary>
		/// <param name="num">交换暗牌的数量</param>
		public void Exchange(int num) { }

		/// <summary>
		/// 打出
		/// </summary>
		/// <param name="tile">打出的牌</param>
		public void Play(Tile tile) { }

		/// <summary>
		/// 自杠
		/// </summary>
		/// <param name="tile">cost癞子或校徽</param>
		public void SelfRod(Tile tile) { }

		/// <summary>
		/// 自杠
		/// </summary>
		/// <param name="tile1">cost手牌1</param>
		/// <param name="tile2">cost手牌2</param>
		/// <param name="tile3">cost手牌3</param>
		/// <param name="tile4">cost手牌4</param>
		public void SelfRod(Tile tile1, Tile tile2, Tile tile3, Tile tile4) { }

		/// <summary>
		/// 玩家胡牌，游戏结束
		/// </summary>
		public void Win() { }

		// ------------------回合外操作------------------

		/// <summary>
		/// 吃
		/// </summary>
		/// <param name="tile1">cost手牌1</param>
		/// <param name="tile2">cost手牌2</param>
		public void Eat(Tile tile1, Tile tile2) { }

		/// <summary>
		/// 碰
		/// </summary>
		/// <param name="tile1">cost手牌1</param>
		/// <param name="tile2">cost手牌2</param>
		public void Touch(Tile tile1, Tile tile2) { }

		/// <summary>
		/// 杠(专指回合外)
		/// </summary>
		/// <param name="tile1">cost手牌1</param>
		/// <param name="tile2">cost手牌2</param>
		/// <param name="tile3">cost手牌3</param>
		public void Rod(Tile tile1, Tile tile2, Tile tile3) { }
	}

	public class MainPlayer : Player
	{

		/// <summary>
		/// 当前客户端自身的玩家对象
		/// </summary>
		public MainPlayer() { }

		/// <summary>
		/// 抽牌
		/// </summary>
		/// <param name="tile">抽到的牌</param>
		public void Draw(Tile tile) { }

		/// <summary>
		/// 获取所有被选中的牌
		/// </summary>
		/// <param name="type">种类，'h'手牌，'n'暗牌</param>
		/// <exception cref="ArgumentException"/>
		/// <returns></returns>
		public List<Tile> GetChoosed(char type='h')
		{
			List<Tile> tiles = new List<Tile>();
			switch (type)
			{
				case 'h':
					foreach (Tile t in this.hand)
						if (t.IsChoosed())
							tiles.Add(t);
					break;
				case 'n':
					foreach (Tile t in this.hiden)
						if (t.IsChoosed())
							tiles.Add(t);
					break;
				default:
					throw new ArgumentException("Parameter \'type\' should be \'h\' or \'n\'.", type.ToString());
			}

			return tiles;
		}

	}

}