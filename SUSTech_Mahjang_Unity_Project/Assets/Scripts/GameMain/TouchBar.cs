using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameMain
{
	public class TouchBar
	{
		public bool eatButton { get; private set; }
		public bool touchButton { get; private set; }
		public bool rodButton { get; private set; }
		public bool winButton { get; private set; }
		public bool swapButton { get; private set; }

		public TouchBar()
		{
			eatButton = false;
			touchButton = false;
			rodButton = false;
			winButton = false;
		}

		#region Temp method for test

		public void SetEatButton(bool value) => eatButton = value;
		public void SetTouchButton(bool value) => touchButton = value;
		public void SetRodButton(bool value) => rodButton = value;
		public void SetWinButton(bool value) => winButton = value;
		public void SetSwapButton(bool value) => swapButton = value;

		#endregion

	}
}
