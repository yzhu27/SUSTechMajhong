using System.Collections.Generic;
using System;

using Assets.Scripts.Util;

namespace Assets.Scripts.GameMain.Charactors
{
	public abstract class Charactor
	{
		public abstract uint id { get; }
		public abstract string name { get; }

		protected Player self;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="player"></param>
		public Charactor(Player player)
		{
			self = player;
		}

		/// <summary>
		/// after other player played a tile,
		/// check if and resopnse can do.
		/// </summary>
		/// <param name="last">tile played by other player</param>
		/// <param name="cache">a cache to avoid repeat calculations</param>
		/// <returns></returns>
		public HashSet<Action> GetAvailableResponces(Tile last, Dictionary<Action, HashSet<Tile>> cache)
		{
			HashSet<Action> actions = new HashSet<Action>();
			cache.Clear();

			var eat = EatableTiles(last, null, self.hand);
			var touch = TouchableTiles(last, null, self.hand);
			var rod = RodableTiles(last, null, self.hand);
			bool win = CanResponceWin(last);

			if (eat != null)
			{
				actions.Add(Action.Eat);
				cache.Add(Action.Eat, eat);
			}
			if (touch != null)
			{
				actions.Add(Action.Touch);
				cache.Add(Action.Touch, touch);
			}
			if (rod != null)
			{
				actions.Add(Action.Rod);
				cache.Add(Action.Rod, rod);
			}
			if (win)
			{
				actions.Add(Action.Win);
			}

			return actions;
		}

		/// <summary>
		/// after draw, check actions can do
		/// </summary>
		/// <param name="cache">a cache to avoid repeat calculations</param>
		/// <returns></returns>
		public HashSet<Action> GetAvailableActions(Dictionary<Action, HashSet<Tile>> cache)
		{
			HashSet<Action> actions = new HashSet<Action>();
			cache.Clear();

			var rod = SelfRodableTiles(null, self.hand);
			bool win = CanWin();
			bool swap = CanSwap();

			if (rod != null)
			{
				actions.Add(Action.Rod);
				cache.Add(Action.Rod, rod);
			}
			if (win)
			{
				actions.Add(Action.Win);
			}
			if (swap)
			{
				actions.Add(Action.Swap);
			}

			return actions;
		}

		public HashSet<Tile> GetAvailableResponceTilesByCache(Action action, Tile last, List<Tile> fix, HashSet<Tile> cache)
		{
			var temp = new List<Tile>(cache);
			temp.Sort();

			switch (action)
			{
				case Action.Eat:
					return EatableTiles(last, fix, temp);
				case Action.Touch:
					return TouchableTiles(last, fix, temp);
				case Action.Rod:
					return RodableTiles(last, fix, temp);
				default:
					throw new ArgumentException(action.ToString() + " is not permitted in responce stage");
			}
		}

		public HashSet<Tile> GetAvailableActionTilesByCache(Action action, List<Tile> fix, HashSet<Tile> cache)
		{
			switch (action)
			{
				case Action.Rod:
					var temp = new List<Tile>(cache);
					temp.Sort();
					return SelfRodableTiles(fix, temp);
				default:
					throw new ArgumentException(action.ToString() + " is not permitted in main stage");
			}
		}

		#region On responce

		private HashSet<Tile> EatableTiles(Tile last, List<Tile> fix, List<Tile> hand)
		{
			if (fix == null || fix.Count == 0)
				return Rule.GetEatableTiles(last, hand);
			else if (fix.Count == 1)
				return Rule.GetEatableTiles(last, fix[0], hand);
			else if (fix.Count == 2)
				return null;
			else
				throw new ArgumentException("too many fix files to eat");
		}

		private HashSet<Tile> TouchableTiles(Tile last, List<Tile> fix, List<Tile> hand)
		{
			if (fix == null || fix.Count == 0)
				return Rule.GetTouchableTiles(last, hand);
			else if (fix.Count == 1)
				return Rule.GetTouchableTiles(last, fix[0], hand);
			else if (fix.Count == 2)
				return null;
			else
				throw new ArgumentException("too many fix files to touch");
		}

		private HashSet<Tile> RodableTiles(Tile last, List<Tile> fix, List<Tile> hand)
		{
			if (fix == null || fix.Count == 0)
				return Rule.GetRodableTiles(last, hand);
			else if (fix.Count == 1)
				return Rule.GetRodableTiles(last, fix[0], hand);
			else if (fix.Count == 2)
				return Rule.GetRodableTiles(last, fix[0], fix[1], hand);
			else if (fix.Count == 3)
				return null;
			else
				throw new ArgumentException("too many fix files to touch");
		}

		private bool CanResponceWin(Tile last)
		{
			var temp = new List<Tile>(self.hand);
			temp.Add(last);
			temp.Sort();
			return Rule.BasicCanWin(self.onDesk, temp);
		}

		#endregion

		#region On play

		private HashSet<Tile> SelfRodableTiles(List<Tile> fix, List<Tile> hand)
		{
			return Rule.GetSelfRodableTiles(self.onDesk, fix, hand);
		}

		private bool CanWin()
		{
			return Rule.BasicCanWin(self.onDesk, self.hand);
		}

		private bool CanSwap()
		{
			return self.hiden.Count > 0;
		}

		#endregion

	}
}

