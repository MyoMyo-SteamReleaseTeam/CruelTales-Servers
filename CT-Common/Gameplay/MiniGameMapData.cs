using System.Collections.Generic;
using System.Numerics;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;
using Newtonsoft.Json;

namespace CT.Common.Gameplay
{
	public class MiniGameMapData
	{
		public MiniGameIdentity MiniGameIdentity;
		public List<Vector2> SpawnPositions = new();
		public List<ColliderInfo> EnvironmentColliders = new();

		[JsonIgnore]
		public List<KaRigidBody> StaticRigidBodies { get; private set; } = new();

		public void Initialize()
		{
			foreach (ColliderInfo col in EnvironmentColliders)
			{
				StaticRigidBodies.Add(col.CreateRigidBody());
			}
		}
	}
}
