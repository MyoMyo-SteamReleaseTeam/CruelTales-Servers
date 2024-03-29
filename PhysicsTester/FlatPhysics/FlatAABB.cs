﻿using System.Numerics;

namespace FlatPhysics
{
	public readonly struct FlatAABB
	{
		public readonly Vector2 Min;
		public readonly Vector2 Max;

		public FlatAABB(Vector2 min, Vector2 max)
		{
			this.Min = min;
			this.Max = max;
		}

        public FlatAABB(float minX, float minY, float maxX, float maxY)
        {
			this.Min = new Vector2(minX, minY);
			this.Max = new Vector2(maxX, maxY);
        }
    }
}
