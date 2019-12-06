using System.Collections.Generic;

using Assets.Scripts.GameMain;

namespace Assets.Scripts.Util
{
	public class TileFactory
	{
		/// <summary>
		/// All tiles in factory
		/// </summary>
		private static Dictionary<int, Tile> tiles;

		/// <summary>
		/// Use this function instead of <c>new Tile()</c>
		/// </summary>
		/// <param name="id">id of <see cref="Tile"/></param>
		/// <returns></returns>
		public Tile GetTile(int id)
		{
			if (tiles.TryGetValue(id, out Tile tile))
			{
				return tile;
			}
			else
			{
				return new Tile(id);
			}
		}
	}
}
