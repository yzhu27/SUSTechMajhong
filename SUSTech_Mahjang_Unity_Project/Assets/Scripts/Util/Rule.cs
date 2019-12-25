using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Assets.Scripts.GameMain;

namespace Assets.Scripts.Util
{
	public static class Rule
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
		/// <param name="tile">手牌</param>
		/// <param name="on_desk">吃碰杠牌区</param>
		/// <returns></returns>
		static bool CanAddRod(Tile tile, List<List<Tile>> on_desk)
		{
			if (on_desk == null) return false;
			for (int i = 0; i < on_desk.Count; i++)
				if (
					on_desk[i].Count == 3 &&
					CanRod(tile, on_desk[i][0], on_desk[i][1], on_desk[i][2])
					) return true;
			return false;
		}

		/// <summary>
		/// 获取可以用于碰的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于碰的牌，为<c>null</c>则没有可以碰的牌</returns>
		public static HashSet<Tile> GetTouchableTiles(Tile last, List<Tile> hand_tile)
		{
			HashSet<Tile> tiles = new HashSet<Tile>();

			for(int i = 0; i < hand_tile.Count; i++)
			{
				if (hand_tile[i] == last)
				{
					tiles.Add(hand_tile[i]);
				}
			}
			if (tiles.Count >= 2) return tiles;
			else return null;
		}

		/// <summary>
		/// 获取可以用于碰的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="fix">已经选中的手牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于碰的牌，为<c>null</c>则没有可以碰的牌</returns>
		public static HashSet<Tile> GetTouchableTiles(Tile last, Tile fix, List<Tile> hand_tile)
		{
			
            if (last != fix) return null;

			HashSet<Tile> tiles = new HashSet<Tile>();

			for(int i = 0; i < hand_tile.Count; i++)
			{
				if (hand_tile[i] == last && !hand_tile[i].Equals(fix))
					tiles.Add(hand_tile[i]);
			}
			if (tiles.Count > 0)
				return tiles;
			else
				return null;
		}

		/// <summary>
		/// 获取可以用于吃的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于吃的牌，为<c>null</c>则没有可以吃的牌</returns>
		public static HashSet<Tile> GetEatableTiles(Tile last, List<Tile> hand_tile)
		{
			HashSet<Tile> tiles = new HashSet<Tile>();
			bool flag;

			for(int i = 0; i < hand_tile.Count - 1; i++)
			{
				if (
					hand_tile[i].GetSpecial() != Special.King &&
					hand_tile[i].GetSpecial() != Special.None
				) continue;

				for(int j = i + 1; j < hand_tile.Count; j++)
				{
					if (
					hand_tile[j].GetSpecial() != Special.King &&
					hand_tile[j].GetSpecial() != Special.None
					) continue;

					flag = false;
					if (CanEat(last, hand_tile[i], hand_tile[j]))
					{
						flag = true;
						tiles.Add(hand_tile[j]);
					}
					if (flag)
					{
						tiles.Add(hand_tile[i]);
					}
				}
			}
			if (tiles.Count > 0)
				return tiles;
			else
				return null;
		}

		/// <summary>
		/// 获取可以用于吃的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="fix">已经选中的手牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于吃的牌，为<c>null</c>则没有可以吃的牌</returns>
		public static HashSet<Tile> GetEatableTiles(Tile last, Tile fix, List<Tile> hand_tile)
		{
			if (last.GetSpecial() != Special.None) return null;

			HashSet<Tile> tiles = new HashSet<Tile>();

			for (int i = 0; i < hand_tile.Count; i++)
				if (CanEat(last, fix, hand_tile[i]))
					tiles.Add(hand_tile[i]);

			return tiles;
		}

		/// <summary>
		/// 获取可以响应杠的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于杠的牌，为<c>null</c>则没有可以杠的牌</returns>
		public static HashSet<Tile> GetRodableTiles(Tile last, List<Tile> hand_tile)
		{
			HashSet<Tile> tiles = new HashSet<Tile>();

			for (int i = 0; i < hand_tile.Count - 2; i++)
			{
				if (CanRod(last, hand_tile[i], hand_tile[i + 1], hand_tile[i + 2]))
				{
					tiles.Add(hand_tile[i]);
					tiles.Add(hand_tile[i + 1]);
					tiles.Add(hand_tile[i + 2]);
				}
			}
			if (tiles.Count > 0) return tiles;
			else return null;
		}

		/// <summary>
		/// 获取可以响应杠的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="fix">已经选中的手牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于杠的牌，为<c>null</c>则没有可以杠的牌</returns>
		public static HashSet<Tile> GetRodableTiles(Tile last, Tile fix, List<Tile> hand_tile)
		{
			if (fix != last) return null;

			HashSet<Tile> tiles = new HashSet<Tile>();

			for (int i = 0; i < hand_tile.Count - 1; i++)
			{
				if (!hand_tile[i].Equals(fix) && !hand_tile[i + 1].Equals(fix)
					&& CanRod(last, fix, hand_tile[i], hand_tile[i + 1]))
				{
					tiles.Add(hand_tile[i]);
					tiles.Add(hand_tile[i + 1]);
				}
			}
			if (tiles.Count > 0) return tiles;
			else return null;
		}

		/// <summary>
		/// 获取可以响应杠的手牌列表
		/// </summary>
		/// <param name="last">被打出的牌</param>
		/// <param name="fix_1">已经选中的手牌</param>
		/// <param name="fix_2">已经选中的手牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于杠的牌，为<c>null</c>则没有可以杠的牌</returns>
		public static HashSet<Tile> GetRodableTiles(Tile last, Tile fix_1, Tile fix_2, List<Tile> hand_tile)
		{
			if (fix_1 != last || fix_2 != last) return null;

			HashSet<Tile> tiles = new HashSet<Tile>();

			for (int i = 0; i < hand_tile.Count; i++)
			{
				if (!hand_tile[i].Equals(fix_1) && !hand_tile[i].Equals(fix_2)
					&& CanRod(last, fix_1, fix_2, hand_tile[i]))
				{
					tiles.Add(hand_tile[i]);
				}
			}
			if (tiles.Count > 0) return tiles;
			else return null;
		}

		/// <summary>
		/// 获取可以自己回合杠的手牌列表
		/// </summary>
		/// <param name="on_desk">碰吃杠区</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns>可用于杠的牌，为<c>null</c>则没有可以杠的牌</returns>
		public static HashSet<Tile> GetSelfRodableTiles(List<List<Tile>> on_desk, List<Tile> hand_tile)
		{
			HashSet<Tile> tiles = new HashSet<Tile>();

			// single hand tile
			for (int i = 0; i < hand_tile.Count; i++)
			{
				if (CanRod(hand_tile[i]))
					tiles.Add(hand_tile[i]);
				else if (CanAddRod(hand_tile[i], on_desk))
					tiles.Add(hand_tile[i]);
			}

			// 4 hand tiles
			for (int i = 0; i < hand_tile.Count - 3; i++)
			{
				if (CanRod(hand_tile[i], hand_tile[i+1], hand_tile[i+2], hand_tile[i + 3]))
				{
					tiles.Add(hand_tile[i]);
					tiles.Add(hand_tile[i + 1]);
					tiles.Add(hand_tile[i + 2]);
					tiles.Add(hand_tile[i + 3]);
				}
			}

			if (tiles.Count > 0) return tiles;
			else return null;
		}

		/// <summary>
		/// 获取可以自己回合杠的手牌列表
		/// </summary>
		/// <param name="fix">已经选中的手牌</param>
		/// <param name="hand_tile">手牌</param>
		/// <returns></returns>
		public static HashSet<Tile> GetSelfRodableTiles(List<List<Tile>> on_desk, List<Tile> fix, List<Tile> hand_tile)
		{

			if (fix == null || fix.Count == 0)
			{
				return GetSelfRodableTiles(on_desk, hand_tile);
			}
			else
			{
				List<Tile> temp = new List<Tile>(hand_tile);
				temp.Remove(fix[0]);

				if (fix.Count == 1)
					return GetRodableTiles(fix[0], temp);

				else if (fix.Count == 2)
					return GetRodableTiles(fix[0], fix[1], temp);

				else if (fix.Count == 3)
					return GetRodableTiles(fix[0], fix[1], fix[2], temp);

				else if (fix.Count == 4)
					return null;

				else
					throw new ArgumentException("too many fix tiles on rod");
			}
		}

		/// <summary>
		/// 是否可以胡牌
		/// </summary>
		/// <param name="on_desk"></param>
		/// <param name="hand_tile"></param>
		/// <returns></returns>
		public static bool BasicCanWin(List<Tile> hand_tile)
		{
			Debug.Log("Calculating win");
			List<Tile> tiles = new List<Tile>(hand_tile);

			int king_count = 0;

			while (hand_tile[king_count].GetSpecial() == Special.King)
			{
				king_count++;
			}

			if (king_count > 1)
				return false;

			switch (king_count)
			{
				case 0:
					return CanWinWithoutKing(tiles);
				case 1:
					return CanWinWithKingCount(tiles.GetRange(king_count, tiles.Count - king_count), king_count);
				default:
					return false;
			}
		}

		private static bool CanWinWithoutKing(List<Tile> tiles)
		{
			for (int i = 0; i < tiles.Count - 1; i++)
			{
				if (tiles[i+1] == tiles[i]
					&& (tiles[i].GetSeq() == 2
					|| tiles[i].GetSeq() == 5
					|| tiles[i].GetSeq() == 8)
				)
				{
					Debug.Log("using " + tiles[i] + " " + tiles[i + 1] + " as jiang");
					List<Tile> tiles1 = new List<Tile>(tiles);
					tiles1.RemoveRange(i, 2);
					if (CanWinExceptJiang(tiles1, 0))
						return true;
				}
			}
			return false;
		}

		private static bool CanWinWithKingCount(List<Tile> tiles, int king_count)
		{
			if (king_count == 1)
			{
				for (int i = 0; i < tiles.Count; i++)
				{
					if
					(
						tiles[i].GetSeq() == 2
						|| tiles[i].GetSeq() == 5
						|| tiles[i].GetSeq() == 8
					)
					{
						Debug.Log("using " + tiles[i] + " as jiang");
						List<Tile> tiles1 = new List<Tile>(tiles);
						tiles1.RemoveAt(i);
						if (CanWinExceptJiang(tiles1, 0))
							return true;
					}
				}
				for (int i = 0; i < tiles.Count - 1; i++)
				{
					if (tiles[i + 1] == tiles[i]
						&& (tiles[i].GetSeq() == 2
						|| tiles[i].GetSeq() == 5
						|| tiles[i].GetSeq() == 8)
					)
					{
						Debug.Log("using " + tiles[i] + " " + tiles[i + 1] + " as jiang");
						List<Tile> tiles1 = new List<Tile>(tiles);
						tiles1.RemoveRange(i, 2);
						if (CanWinExceptJiang(tiles1, 1))
							return true;
					}
				}
			}

			return false;
		}

		public static bool CanWinExceptJiang(List<Tile> tiles, int king_count)
		{
			int total = tiles.Count + king_count;

			if (total % 3 != 0)
				Debug.LogError("Wrong count");

			int pair = total / 3;

			// i touches
			for (int i = 0; i <= pair; i++)
			{
				List<Tile> tiles1 = new List<Tile>(tiles);
				int king_remain = king_count;

				int start;
				int touches = 0;

				Debug.Log("trying " + i + " touch");

				// try touch without kings
				start = 0;
				while (touches < i && start < tiles1.Count - 2)
				{
					if (IsPair(new List<Tile>() { tiles1[start], tiles1[start + 1], tiles1[start + 2] }))
					{
						Debug.Log("remove " + tiles1[start] + " " + tiles1[start + 1] + " " + tiles1[start + 2] + " as touch");
						tiles1.RemoveRange(start, 3);
						touches++;
					}
					else
						start++;
				}

				// try touch with one king
				start = 0;
				while (touches < i && start < tiles1.Count - 1)
				{
					// Debug.Log("compairing " + tiles1[start] + " " + tiles1[start + 1] + ", " + king_remain + " king remains");
					if (king_remain >= 1 && tiles1[start] == tiles1[start + 1])
					{
						Debug.Log("remove " + tiles1[start] + " " + tiles1[start + 1] + " as touch");
						tiles1.RemoveRange(start, 2);
						touches++;
						king_remain--;
					}
					else
						start++;
				}

				// try touch with two kings
				start = 0;
				while (touches < i && start < tiles1.Count)
				{
					if (king_remain >= 2)
					{
						Debug.Log("remove " + tiles1[start] + " as touch");
						tiles1.RemoveAt(start);
						touches++;
						king_remain -= 2;
					}
					else
						start++;
				}

				// three kings
				while (touches < i && king_remain >= 3)
				{
					touches++;
					king_remain -= 3;
				}

				// sentences
				if (touches == i)
				{
					// no king
					for (start = 0; start < tiles1.Count - 2; start++)
					{
						for (int j = start + 1; j < tiles1.Count - 1; j++)
						{
							for (int k = j + 1; k < tiles1.Count;)
							{
								if (IsSentence(new List<Tile>() { tiles1[start], tiles1[j], tiles1[k] }))
								{
									Debug.Log("remove " + tiles1[start] + " " + tiles1[j] + " " + tiles1[k] + " as sentence");
									tiles1.RemoveAt(k);
									tiles1.RemoveAt(j);
									tiles1.RemoveAt(start);

									j -= 1;
									k -= 2;
								}
								else
									k++;
							}
						}
					}
					if (tiles1.Count == 0)
						return true;

					// one king
					for (start = 0; start < tiles1.Count - 1 && king_remain >= 1; start++)
					{
						for (int j = start + 1; j < tiles1.Count && king_remain >= 1;)
						{
							// Debug.Log("compairing " + tiles1[start] + " " + tiles1[j] + ", " + king_remain + " king remains");
							if (
								tiles1[start].GetSpecial() == Special.None &&
								tiles1[j].GetSpecial() == Special.None &&
								tiles1[start].GetDepartment() == tiles1[j].GetDepartment() &&
								(
									tiles1[start].GetSeq() + 1 == tiles1[j].GetSeq() ||
									tiles1[start].GetSeq() + 2 == tiles1[j].GetSeq()
								)
							){
								Debug.Log("remove " + tiles1[start] + " " + tiles1[j] + " as sentence");
								tiles1.RemoveAt(j);
								tiles1.RemoveAt(start);
								king_remain--;

								j--;
							}
							else
								j++;
						}
					}
					if (tiles1.Count == 0)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}
