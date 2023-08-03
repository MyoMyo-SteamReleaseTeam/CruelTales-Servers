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

		public Vector2 LeftTop => new Vector2(Min.X, Max.Y);
		public Vector2 LeftBottom => Min;
		public Vector2 RightTop => Max;
		public Vector2 RightBottom => new Vector2(Max.X, Min.Y);

		public BoundingBox(Vector2 min, Vector2 max)
		{
			Min = min;
			Max = max;
		}

		public BoundingBox(Vector2 position, float width, float height)
		{
			Vector2 halfSize = new Vector2(width, height) * 0.5f;
			Min = position - halfSize;
			Max = position + halfSize;
		}
	}
}
