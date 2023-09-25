using System.Collections.Generic;
using System.Numerics;
using CT.Common.Gameplay.Infos;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;
using Newtonsoft.Json;

namespace CT.Common.Gameplay
{
	public class GameSceneMapData
	{
		public GameSceneIdentity GameSceneIdentity;
		public List<Vector2> SpawnPositions = new();
		public List<ColliderInfo> EnvironmentColliders = new();
		public Dictionary<InteractorType, List<InteractorInfo>> InteractorTable = new();
		public List<AreaInfo> AreaInfos = new();
		public List<PivotInfo> PivotInfos = new();

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
