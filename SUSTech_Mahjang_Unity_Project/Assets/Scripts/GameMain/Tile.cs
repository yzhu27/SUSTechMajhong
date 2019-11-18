
namespace Assets.Scripts.GameMain
{
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
