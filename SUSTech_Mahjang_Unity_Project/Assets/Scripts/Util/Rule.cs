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
			for (int i = 0; i < on_desk.Count(); i++)
				if (on_desk[i].Count() == 3 && CanRod(hand_tile, on_desk[i][0], on_desk[i][1], on_desk[i][2])) return i;
			return -1;
		}

	}
}
