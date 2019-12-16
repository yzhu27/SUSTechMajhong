using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameMain.Charactors
{
	public class EmptyCharactor : Charactor
	{
		public override uint id { get => 0; }
		public override string name { get => "Empty"; }
		public EmptyCharactor(Player player) : base(player) { }

	}
}
