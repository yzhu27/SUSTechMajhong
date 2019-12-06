using System.Collections.Generic;

namespace Assets.Scripts.GameMain
{
	public class TileFactory
	{
		/// <summary>
		/// All tiles in factory
		/// </summary>
		private Dictionary<int, Tile> tiles;

		public TileFactory() => tiles = new Dictionary<int, Tile>();

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
				tile = new Tile(id);
				tiles.Add(id, tile);
				return tile;
			}
		}
	}
}
