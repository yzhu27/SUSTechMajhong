using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.GameMain;
using Assets.Scripts.Util;

namespace Assets.Scripts.LocalServer
{
	class TilePool
	{
		private const int playerNum = 4;

		private List<Department> departments;

		private TileFactory tileFactory;

		private List<Tile> pool;

		/// <summary>
		/// total number of tiles in pool
		/// </summary>
		public int Count { get => pool.Count; }

		/// <summary>
		/// remained number of tiles in pool
		/// </summary>
		public int remain { get => pool.Count - next; }

        /// <summary>
        /// whether pool is sorted or shuffled
        /// </summary>
        //public bool shuffled { get => this.shuffled; private set => this.shuffled = value; }
        public bool shuffled { get; private set; }

        private int next;

		/// <summary>
		/// Tile pool is the stack where all players draw
		/// tiles from, every game has one tile pool.
		/// </summary>
		/// <param name="departments">
		/// List of department of players' charactors,
		/// there are 4 players in one game, so the
		/// length of <paramref name="departments"/>
		/// should be 4.
		/// </param>
		/// <exception cref="ArgumentException"/>
		public TilePool(List<Department> departments)
		{
			// check parameter
			if (departments.Count != playerNum) throw new ArgumentException(
				string.Format("length should be {0}", playerNum),
				nameof(departments)
			);
			// init
			this.departments = new List<Department>();
			tileFactory = new TileFactory();
			pool = new List<Tile>();
			shuffled = false;
			next = 0;
			// set department
			List<Department> all = new List<Department>(
				(Department[])Enum.GetValues(typeof(Department))
			);
			foreach (Department department in departments)
			{
				if (!this.departments.Contains(department))
				{
					this.departments.Add(department);
					all.Remove(department);
				}
			}
			if (this.departments.Count < 4)
				Algorithm.Shuffle(all);
			while (this.departments.Count < 4)
			{
				departments.Add(all[this.departments.Count]);
			}
			// generate tile pool
			GenerateSpecial();
			GenerateNone();
		}

		/// <summary>
		/// shuffle the tile pool
		/// </summary>
		public void Shuffle()
		{
			Algorithm.Shuffle(pool);
			shuffled = true;
		}

		/// <summary>
		/// draw from a shuffled tile pool
		/// </summary>
		/// <returns>id of dwawn tile, -1 if pool is empty</returns>
		/// <exception cref="AccessViolationException"/>
		public int Draw()
		{
			if (!shuffled)
				throw new AccessViolationException("can't draw from unshuffled tile pool");
			if (pool.Count > next)
			{
				return pool[next++].id;
			}
			else
				return -1;
		}

		/// <summary>
		/// to string for debugging
		/// </summary>
		/// <returns></returns>
		public string PoolToString()
		{
			string s = "";
			s += string.Format("TilePool with {0} Tiles\r\n", pool.Count);
			foreach (Tile tile in pool)
			{
				s += " - " + tile.ToString();
			}
			return s;
		}

		private int CalcReserve(int reserve_0, int reserve_1)
		{
			if (reserve_0 < 0 || reserve_0 > 3)
				throw new ArgumentOutOfRangeException(nameof(reserve_0), reserve_0, "should in range 0~3");
			else if (reserve_1 < 0 || reserve_1 > 15)
				throw new ArgumentOutOfRangeException(nameof(reserve_1), reserve_1, "should in range 0~15");
			return (reserve_0 << (int)Location.Reserve0) + (reserve_1 << (int)Location.Reserve1);
		}

		private void GenerateKing(int reserve_0 = 0, int reserve_1 = 0)
		{
			int reserve = CalcReserve(reserve_0, reserve_1);

			for (int i = 0; i < 4; i++)
				pool.Add(tileFactory.GetTile(
					(int)Special.King << (int)Location.Special +
					i << (int)Location.Unique +
					reserve
				));
		}

		private void GenerateLogo(int reserve_0 = 0, int reserve_1 = 0)
		{
			int reserve = CalcReserve(reserve_0, reserve_1);

			for (int i = 0; i < 4; i++)
				pool.Add(tileFactory.GetTile(
					(int)Special.Logo << (int)Location.Special +
					i << (int)Location.Unique +
					reserve
				));
		}

		private void GenerateSign(int reserve_0 = 0, int reserve_1 = 0)
		{
			int reserve = CalcReserve(reserve_0, reserve_1);

			foreach (int department in departments)
				for (int i = 0; i < 4; i++)
					pool.Add(tileFactory.GetTile(
						(int)Special.Sign << (int)Location.Special +
						i << (int)Location.Unique +
						reserve
					));
		}

		private void GenerateLand(int reserve_0 = 0, int reserve_1 = 0)
		{
			int reserve = CalcReserve(reserve_0, reserve_1);

			foreach (int land in Enum.GetValues(typeof(Land)))
				for (int i = 0; i < 4; i++)
					pool.Add(tileFactory.GetTile(
						(int)Special.Land << (int)Location.Special +
						land << (int)Location.Land +
						i << (int)Location.Unique +
						reserve
					));
		}

		private void GenerateSpecial()
		{
			GenerateKing();
			GenerateLogo();
			GenerateSign();
			GenerateLand();
		}

		private void GenerateNone()
		{
			foreach (Department department in departments)
				for (int i = 1; i < 10; i++)
					for (int j = 0; j < 4; j++)
						pool.Add(tileFactory.GetTile(
							(int)Special.None << (int)Location.Special +
							(int)department << (int)Location.Department +
							i << (int)Location.Sequence +
							j << (int)Location.Unique
						));
		}
	}
}
