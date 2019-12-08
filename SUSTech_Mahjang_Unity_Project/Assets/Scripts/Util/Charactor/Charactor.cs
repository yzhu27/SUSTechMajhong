using System.Collections.Generic;

using Assets.Scripts.GameMain;
using Assets.Scripts.Util;

namespace Assets.Scripts.Util.Charactor
{
	public abstract class Charactor
	{
		protected Player self;

		public Charactor(Player player)
		{
			self = player;
		}

		// On win check
		public abstract bool CanWin();
		// On responce
		public abstract bool CanTouch(Tile last);
		public abstract bool CanEat(Tile last);
		public abstract bool CanRod(Tile last);
		// On play
		public abstract bool CanSelfRod();
		public abstract bool CanAddRod();

	}
}

