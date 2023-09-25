using System;
using System.Numerics;
using Newtonsoft.Json;

namespace CT.Common.Gameplay.Infos
{
	[Serializable]
	public struct AreaInfo
	{
		public int Index;
		public Vector2 Position;
		public Vector2 Size;

		[JsonIgnore]
		public float Area => Size.X * Size.Y;

		public Vector2 GetRandomPosition()
		{
			Vector2 halfSize = Size * 0.5f;
			Vector2 min = Position - halfSize;
			Vector2 max = Position + halfSize;
			return RandomHelper.NextVector2(min, max);
		}

		public AreaInfo(int index, Vector2 position, Vector2 size)
		{
			Index = index;
			Position = position;
			Size = size;
		}
	}
}
