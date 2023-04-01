using System.Numerics;

namespace CT.Tools
{
	public static class KMath
	{
		public static int Clamp(int value, int maxValue, int minValue)
		{
			if (value > maxValue)
			{
				return maxValue;
			}

			if (value < minValue)
			{
				return minValue;
			}

			return value;
		}

		/// <summary>값을 스냅합니다.</summary>
		/// <param name="value">스냅할 값입니다.</param>
		/// <param name="snap">스냅 크기입니다.</param>
		/// <returns>스냅된 값입니다.</returns>
		public static float SnapBy(float value, float snap)
		{
			return (int)(value / snap) * snap;
		}

		/// <summary>값을 스냅합니다.</summary>
		/// <param name="value">스냅할 값입니다.</param>
		/// <param name="snap">스냅 크기입니다.</param>
		/// <returns>스냅된 값입니다.</returns>
		public static Vector2 SnapBy(Vector2 value, float snap)
		{
			float x = SnapBy(value.X, snap);
			float y = SnapBy(value.Y, snap);
			return new Vector2(x, y);
		}

		public static float Remap(float inValue, float inMin, float inMax, float outMin, float outMax)
		{
			return outMin + (inValue - inMin) / (inMax - inMin) * (outMax - outMin);
		}
	}
}
