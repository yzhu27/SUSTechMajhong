using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
	static class Algorithm
	{
		public static void Shuffle<T>(List<T> list)
		{
			Random random = new Random();
			T temp;
			int rand;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				rand = random.Next(0, i);
				temp = list[i];
				list[i] = list[rand];
				list[rand] = temp;
			}
		}
	}
}
