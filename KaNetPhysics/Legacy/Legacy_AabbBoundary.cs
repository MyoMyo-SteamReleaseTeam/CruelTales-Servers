using System.Numerics;

namespace KaNet.Physics.Legacy
{
	public readonly struct Legacy_AabbBoundary
	{
		public readonly Vector2 Min;
		public readonly Vector2 Max;

		public Legacy_AabbBoundary(Vector2 min, Vector2 max)
		{
			Min = min;
			Max = max;
		}

		public Legacy_AabbBoundary(float minX, float minY, float maxX, float maxY)
		{
			Min = new Vector2(minX, minY);
			Max = new Vector2(maxX, maxY);
		}
	}
}
