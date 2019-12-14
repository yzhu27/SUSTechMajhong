using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameMain
{
	public enum Action
	{
		Eat, Touch, Rod, Win, Swap
	}

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

		/// <summary>
		/// 更新行为可行性
		/// </summary>
		/// <param name="actions">
		///		从<see cref="MainPlayer.GetActionsOnPlay"/>
		///		或<see cref="MainPlayer.GetActionsOnResponse"/>
		///		获取的返回值
		/// </param>
		public void SetActions(HashSet<Action> actions)
		{
			eatButton = actions.Contains(Action.Eat);
			touchButton = actions.Contains(Action.Touch);
			rodButton = actions.Contains(Action.Rod);
			winButton = actions.Contains(Action.Win);
			swapButton = actions.Contains(Action.Swap);
		}

		

	}
}
