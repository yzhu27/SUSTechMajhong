using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GameMain;

namespace Assets.Scripts.Util.Charactor
{
	/// <summary>
	/// 
	/// </summary>
	class DemoCharactor1 : Charactor
	{
		public DemoCharactor1(Player player) : base(player) { }

		public override bool CanAddRod()
		{
			throw new NotImplementedException();
		}

		public override bool CanEat(Tile last)
		{
			throw new NotImplementedException();
		}

		public override bool CanRod(Tile last)
		{
			throw new NotImplementedException();
		}

		public override bool CanSelfRod()
		{
			throw new NotImplementedException();
		}

		public override bool CanTouch(Tile last)
		{
			throw new NotImplementedException();
		}

		public override bool CanWin()
		{
			throw new NotImplementedException();
		}
	}
}
