using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Assets.Scripts.GameMain;

namespace Assets.Scripts.Util
{
	static class Rule
	{
		/// <summary>
		/// 判断三张牌是否为对子
		/// </summary>
		/// <param name="tiles">需要判断的牌</param>
		/// <returns>正确则返回true，牌数量错误或不为对子均返回false</returns>
		static bool IsPair(List<Tile> tiles)
		{
			if(tiles.Count == 3) return
				tiles[0] == tiles[1] &&
				tiles[0] == tiles[2] &&
				tiles[1] == tiles[2];

			else return false;
		}

		/// <summary>
		/// 判断三张牌是否为句子
		/// </summary>
		/// <param name="tiles">需要判断的牌</param>
		/// <returns>正确则返回true，牌数量错误或不为句子均返回false</returns>
		static bool IsSentence(List<Tile> tiles)
		{
			if (tiles.Count != 3) return false;

			int king_count = 0;
			bool department_flag = false;
			int department = -1;

			foreach (Tile tile in tiles)
				if (tile.GetSpecial() == Special.King)
					king_count++;
				else if (tile.GetSpecial() == Special.None)
				{   // Check department
					if (department_flag && (int)tile.GetDepartment() != department)
						return false;
					else if (!department_flag)
					{
						department = (int)tile.GetDepartment();
						department_flag = true;
					}
				}
				else return false;

			if (king_count > 1) return true; // Two kings or more must be true
			else
			{   // One king or less
				Debug.Log(string.Format("seqs: {0:d}, {1:d}, {2:d}, 0xf0>>4 = {3:d}", tiles[0].GetSeq(), tiles[1].GetSeq(), tiles[2].GetSeq(), (0xf0)>>4));
				tiles.Sort();
				if (king_count == 1) return
						tiles[1].GetSeq() + 1 == tiles[2].GetSeq() ||
						tiles[1].GetSeq() + 2 == tiles[2].GetSeq();
				else return
						tiles[0].GetSeq() + 1 == tiles[1].GetSeq() &&
						tiles[0].GetSeq() + 2 == tiles[2].GetSeq();
			}
		}

		/// <summary>
		/// 是否可以碰
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="hand_tile_1">手牌1</param>
		/// <param name="hand_tile_2">手牌2</param>
		/// <returns></returns>
		static bool CanTouch(Tile last, Tile hand_tile_1, Tile hand_tile_2)
		{
			List<Tile> tiles = new List<Tile>
			{
				last,
				hand_tile_1,
				hand_tile_2
			};
			return IsPair(tiles);
		}

		/// <summary>
		/// 是否可以吃
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="hand_tile_1">手牌1</param>
		/// <param name="hand_tile_2">手牌2</param>
		/// <returns></returns>
		static bool CanEat(Tile last, Tile hand_tile_1, Tile hand_tile_2)
		{
			List<Tile> tiles = new List<Tile>
			{
				last,
				hand_tile_1,
				hand_tile_2
			};
			return IsSentence(tiles);
		}

		/// <summary>
		/// 判断是否可以杠
		/// </summary>
		/// <param name="hand_tile">手牌</param>
		/// <returns></returns>
		static bool CanRod(Tile hand_tile)
		{
			return hand_tile.GetSpecial() == Special.King
				|| hand_tile.GetSpecial() == Special.Logo;
		}

		/// <summary>
		/// 判断是否可以杠
		/// </summary>
		/// <param name="tile_1">被打出的牌或手牌</param>
		/// <param name="tile_2">手牌</param>
		/// <param name="tile_3">手牌</param>
		/// <param name="tile_4">手牌</param>
		/// <returns></returns>
		static bool CanRod(Tile tile_1, Tile tile_2, Tile tile_3, Tile tile_4)
		{
			if (
				tile_1.GetSpecial() != Special.King &&
				tile_1.GetSpecial() != Special.Logo &&
				tile_2.GetSpecial() != Special.King &&
				tile_2.GetSpecial() != Special.Logo &&
				tile_3.GetSpecial() != Special.King &&
				tile_3.GetSpecial() != Special.Logo &&
				tile_4.GetSpecial() != Special.King &&
				tile_4.GetSpecial() != Special.Logo
			)
				return tile_1 == tile_2
					&& tile_2 == tile_3
					&& tile_3 == tile_4;
			else return false;
		}

		/// <summary>
		/// 是否可以蓄杠
		/// </summary>
		/// <param name="hand_tile">手牌</param>
		/// <param name="on_desk">吃碰杠牌区</param>
		/// <returns>返回可以蓄杠的牌堆序号，若不能蓄杠则返回-1</returns>
		static int CanAddRod(Tile hand_tile, List<List<Tile>> on_desk)
		{
			for (int i = 0; i < on_desk.Count; i++)
				if (
					on_desk[i].Count == 3 &&
					CanRod(hand_tile, on_desk[i][0], on_desk[i][1], on_desk[i][2])
					) return i;
			return -1;
		}

		/// <summary>
		/// 获取可以用于碰的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="hand_tiles">手牌</param>
		/// <returns>可用于碰的牌，为空则没有可以碰的牌</returns>
		public static List<Tile> GetTouchableList(Tile last, List<Tile> hand_tiles)
		{
			List<Tile> tiles = new List<Tile>();

			for(int i = 0; i < hand_tiles.Count; i++)
			{
				if (hand_tiles[i] == last)
				{
					tiles.Add(hand_tiles[i]);
				}
			}
			if (tiles.Count > 1) return tiles;
			else return new List<Tile>();
		}

		/// <summary>
		/// 获取可以用于碰的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="fix">已经选中的手牌</param>
		/// <param name="hand_tiles">手牌</param>
		/// <returns>可用于碰的牌，为空则没有可以碰的牌</returns>
		public static List<Tile> GetTouchableList(Tile last, Tile fix, List<Tile> hand_tiles)
		{
			
            if (last != fix) return new List<Tile>();

			List<Tile> tiles = new List<Tile>();

			for(int i = 0; i < hand_tiles.Count; i++)
			{
				if (hand_tiles[i] == last)
					tiles.Add(hand_tiles[i]);
			}
			return tiles;
		}

		/// <summary>
		/// 获取可以用于吃的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="hand_tiles">手牌</param>
		/// <returns>可用于吃的牌，为空则没有可以吃的牌</returns>
		public static List<Tile> GetEatableList(Tile last, List<Tile> hand_tiles)
		{
			List<Tile> tiles = new List<Tile>();
			bool flag;

			for(int i = 0; i < hand_tiles.Count - 1; i++)
			{
				if (
					hand_tiles[i].GetSpecial() != Special.King &&
					hand_tiles[i].GetSpecial() != Special.None
				) continue;

				for(int j = i + 1; j < hand_tiles.Count; j++)
				{
					if (
					hand_tiles[j].GetSpecial() != Special.King &&
					hand_tiles[j].GetSpecial() != Special.None
					) continue;

					flag = false;
					if (CanEat(last, hand_tiles[i], hand_tiles[j]))
					{
						flag = true;
						if (!tiles.Exists(tile => tile.Equals(hand_tiles[j])))
						{
							tiles.Add(hand_tiles[j]);
						}
					}
					if (flag && !tiles.Exists(tile => tile.Equals(hand_tiles[i])))
					{
						tiles.Add(hand_tiles[i]);
					}
				}
			}
			tiles.Sort();
			return tiles;
		}

		/// <summary>
		/// 获取可以用于吃的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="fix">已经选中的手牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于吃的牌，为空则没有可以吃的牌</returns>
		public static List<Tile> GetEatableList(Tile last, Tile fix, List<Tile> hand_tile)
		{
			if (last.GetSpecial() != Special.None) return new List<Tile>();

			List<Tile> tiles = new List<Tile>();

			for (int i = 0; i < hand_tile.Count; i++)
				if (CanEat(last, fix, hand_tile[i]))
					tiles.Add(hand_tile[i]);

			return tiles;
		}

		public static List<Tile> GetSelfRodableList(List<List<Tile>> on_dest, List<Tile> fix, List<Tile> hand_tile)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 是否可以胡牌
		/// <para>注意：这是不考虑角色技能和院系的基础判断函数，
		/// 不要直接调用该函数，作为替代应使用</para>
		/// </summary>
		/// <param name="on_desk"></param>
		/// <param name="hand_tiles"></param>
		/// <returns></returns>
		public static bool BasicCanWin(List<List<Tile>> on_desk, List<Tile> hand_tiles)
		{
			return false;
		}
	}
}
