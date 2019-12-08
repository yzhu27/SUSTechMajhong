using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GameMain;

namespace Assets.Scripts.Util.Charactor
{
	/// <summary>
	/// 白板角色
	/// </summary>
	class DemoCharactor0 : Charactor
	{
		public DemoCharactor0(Player player) : base(player)
		{
		}

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
