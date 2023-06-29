using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatPhysics
{
	public static class RandomHelper
	{
		private static Random _random = new Random();

		public static bool RandomBollean()
		{
			return RandomInteger(0, 2) == 0;
		}

		public static Color RandomColor()
		{
			return Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255));
		}

		public static int RandomInteger(int min, int max)
		{
			return _random.Next(min, max);
		}

		public static float RandomSingle(float min, float max)
		{
			return FlatMath.Lerp(min, max, _random.NextSingle());
		}
	}
}
