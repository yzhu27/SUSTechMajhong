
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
		None = 0xf0000,
		/// <summary>
		/// 癞子
		/// </summary>
		King = 0x00000,
		/// <summary>
		/// 校徽
		/// </summary>
		Logo = 0x10000,
		/// <summary>
		/// 签到
		/// </summary>
		Sign = 0x20000,
		/// <summary>
		/// 园
		/// </summary>
		Land = 0x30000
	}

	/// <summary>
	/// 园牌
	/// </summary>
	public enum Land
	{
		/// <summary>
		/// 欣园
		/// </summary>
		Xin = 0x00,
		/// <summary>
		/// 荔园
		/// </summary>
		Lee = 0x10,
		/// <summary>
		/// 慧园
		/// </summary>
		Hui = 0x20,
		/// <summary>
		/// 创园
		/// </summary>
		Cng = 0x30,
		/// <summary>
		/// 智园
		/// </summary>
		Zhi = 0x40
	}

	/// <summary>
	/// 麻将牌
	/// </summary>
	public class Tile
	{
		

		/// <summary>
		/// 每张牌唯一确定的id，0表示unknown
		/// </summary>
		public int id;

		/// <summary>
		/// 是否被选中
		/// </summary>
		private bool choosed;

		/// <summary>
		/// 初始化，默认unknown
		/// </summary>
		/// <param name="id"><see cref="id"/></param>
		public Tile(int id = 0)
		{
			this.id = id;
			this.choosed = false;
		}

		/// <summary>
		/// 判断为同一张牌(包括unique)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return obj.GetType() == typeof(Tile) &&
				((Tile)obj).id == id;
		}

		public override int GetHashCode()
		{
			return id;
		}

		/// <summary>
		/// 获取牌的种类
		/// </summary>
		/// <returns>种类</returns>
		public Special GetSpecial()
		{
			return (Special)(id | 0xf0000);
		}

		/// <summary>
		/// 判断两张牌牌面是否相同
		/// </summary>
		/// <param name="tile_1"></param>
		/// <param name="tile_2"></param>
		/// <returns></returns>
		public static bool operator ==(Tile tile_1, Tile tile_2)
		{
			return (tile_1.id | 0xff) == (tile_2.id | 0xff) || (tile_1.GetSpecial() == Special.Sign && tile_2.GetSpecial() == Special.Sign);
		}

		public static bool operator !=(Tile tile_1, Tile tile_2) => !(tile_1 == tile_2);

		/// <summary>
		/// 选中
		/// </summary>
		public void Choose()
		{
			this.choosed = true;
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
