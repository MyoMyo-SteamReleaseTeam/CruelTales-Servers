using System.Collections.Generic;
using System.Numerics;

namespace CT.Common.Gameplay
{
	public struct StaticCollider
	{
		public ColliderType Type;
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 Scale;
	}

	public enum ColliderType
	{
		None = 0,
		Box,
	}

	public class MiniGameMapData
	{
		public GameMapType MapType;
		public GameMapTheme Theme;
		public List<Vector3> SpawnPosition = new();
	}
}
