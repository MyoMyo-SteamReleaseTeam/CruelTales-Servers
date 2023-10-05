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
		public Dictionary<Faction, List<Vector2>> SpawnPositionTable = new();
		public Dictionary<InteractorType, List<InteractorInfo>> InteractorTable = new();
		public Dictionary<ushort, Vector2> SectionDirectionTable = new();
		public List<ColliderInfo> EnvironmentColliders = new();
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
