using System;
using System.Numerics;

namespace CT.Common.Gameplay.Infos
{
	[Serializable]
	public struct PivotInfo
	{
		public int Index;
		public Vector2 Position;

		public PivotInfo(int index, Vector2 position)
		{
			Index = index;
			Position = position;
		}
	}
}
