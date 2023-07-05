using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KaNet.Physics
{
	public readonly struct BoundingBox
	{
		public readonly Vector2 Min;
		public readonly Vector2 Max;

		public BoundingBox(Vector2 min, Vector2 max)
		{
			Min = min;
			Max = max;
		}

		public BoundingBox(Vector2 position, float width, float height)
		{
			Vector2 halfSize = new Vector2(width * 0.5f, height * 0.5f);
			Min = position - halfSize;
			Max = position + halfSize;
		}
	}
}
