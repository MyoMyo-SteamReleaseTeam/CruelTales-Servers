using System.Collections.Generic;
using System.Numerics;
using KaNet.Physics;

namespace CT.Common.Gameplay
{
	public class MiniGameMapData
	{
		public GameMapType MapType;
		public GameMapTheme Theme;
		public List<Vector2> SpawnPosition = new();
		public List<ColliderInfo> ColliderInfoList = new();
	}
}
