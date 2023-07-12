using System;
using System.Numerics;
using Sirenix.OdinInspector;

namespace KaNet.Physics
{
	[Serializable]
	public readonly struct BoundingBox
	{
		[ShowInInspector]
		public readonly Vector2 Min;

		[ShowInInspector]
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
