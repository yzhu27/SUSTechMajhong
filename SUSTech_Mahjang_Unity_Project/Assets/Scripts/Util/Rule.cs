using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
					if (department_flag && tile.GetDepartment() != department)
						return false;
					else if (!department_flag)
					{
						department = tile.GetDepartment();
						department_flag = true;
					}
				}
				else return false;

			if (king_count > 1) return true; // Two kings or more must be true
			else
			{	// One king or less
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
	}
}
