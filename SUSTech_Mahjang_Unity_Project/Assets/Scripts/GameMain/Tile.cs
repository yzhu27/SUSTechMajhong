using System;

namespace Assets.Scripts.GameMain
{
	/// <summary>
	/// 特殊位，位于000X0000
	/// </summary>
	public enum Special
	{
		/// <summary>
		/// 普通牌
		/// </summary>
		None = 0xf,
		/// <summary>
		/// 癞子
		/// </summary>
		King = 0x0,
		/// <summary>
		/// 校徽
		/// </summary>
		Logo = 0x1,
		/// <summary>
		/// 签到
		/// </summary>
		Sign = 0x2,
		/// <summary>
		/// 园
		/// </summary>
		Land = 0x3
	}

	/// <summary>
	/// 园牌
	/// </summary>
	public enum Land
	{
		/// <summary>
		/// 欣园
		/// </summary>
		Xin = 0x0,
		/// <summary>
		/// 荔园
		/// </summary>
		Lee = 0x1,
		/// <summary>
		/// 慧园
		/// </summary>
		Hui = 0x2,
		/// <summary>
		/// 创园
		/// </summary>
		Cng = 0x3,
		/// <summary>
		/// 智园
		/// </summary>
		Zhi = 0x4
	}

	/// <summary>
	/// 麻将牌
	/// </summary>
	public class Tile : IComparable<Tile>
	{
		

		/// <summary>
		/// 每张牌唯一确定的id，0表示unknown
		/// </summary>
		public int id;

		/// <summary>
		/// 是否被选中
		/// </summary>
		private bool choosed;

		///////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// 初始化，默认unknown
		/// </summary>
		/// <param name="id"><see cref="id"/></param>
		public Tile(int id = 0)
		{
			this.id = id;
			this.choosed = false;
		}

        public int getId()
        {
            return this.id;
        }
        public void setid(int i)
        {
            this.id = i;
        }
		/// <summary>
		/// 判断为同一张牌(包括unique)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) =>
			obj.GetType() == typeof(Tile) &&
			((Tile)obj).id == id;

		public override int GetHashCode() => id;

		public override string ToString() => id.ToString();

		public string ToString(string fmt) => id.ToString(fmt);

		/// <summary>
		/// 判断两张牌牌面是否相同
		/// </summary>
		/// <param name="tile_1"></param>
		/// <param name="tile_2"></param>
		/// <returns></returns>
		public static bool operator ==(Tile tile_1, Tile tile_2) =>
			tile_1.GetSpecial() == Special.King ||
			tile_2.GetSpecial() == Special.King || // a king in tile_1 or tile_2
			(tile_1.GetSpecial() == Special.Sign && tile_2.GetSpecial() == Special.Sign) || // sign tiles are same
			(tile_1.id | 0xff) == (tile_2.id | 0xff); // same in id

		public static bool operator !=(Tile tile_1, Tile tile_2) => !(tile_1 == tile_2);

		public int CompareTo(Tile other) => id - other.id;

		////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// 获取牌的种类
		/// </summary>
		/// <returns>种类</returns>
		public Special GetSpecial() => (Special)((id | 0xf0000) >> 16);

		/// <summary>
		/// 获取院系编号
		/// </summary>
		/// <returns></returns>
		public int GetDepartment() => (id | 0xff00) >> 8;

		/// <summary>
		/// 获取普通牌的序号
		/// </summary>
		/// <returns>1~9</returns>
		public int GetSeq() => (id | 0xf0) >> 4;

		/// <summary>
		/// 获取园牌园号
		/// </summary>
		/// <returns>园</returns>
		public Land GetLand() => (Land)GetSeq();

		/// <summary>
		/// 选中
		/// </summary>
		public void Choose() => this.choosed = true;

        public void Unchoose()
        {
            this.choosed = false;
        }

        /// <summary>
        /// 是否被选中
        /// </summary>
        /// <returns>bool</returns>
        public bool IsChoosed()
		{
			return this.choosed;
		}
	}
}
