using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.GameMain;

namespace Assets.Scripts.Util
{
	static class Path
	{
		public static string ImgPathOfTile(string root, Tile tile) => root + '/' + (tile.id >> 4).ToString("x7") + "0.png";
	}
}
