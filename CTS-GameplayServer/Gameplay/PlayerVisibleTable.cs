using System.Collections.Generic;
using CT.Common.DataType;

namespace CTS.Instance.Gameplay
{
	public class PlayerVisibleTable
	{
		public HashSet<NetworkIdentity> SpawnObjects { get; private set; }
		public HashSet<NetworkIdentity> TraceObjects { get; private set; }
		public HashSet<NetworkIdentity> DespawnObjects { get; private set; }

		public PlayerVisibleTable(InstanceInitializeOption option)
		{
			SpawnObjects = new HashSet<NetworkIdentity>(option.SpawnObjectCapacity);
			TraceObjects = new HashSet<NetworkIdentity>(option.TraceObjectCapacity);
			DespawnObjects = new HashSet<NetworkIdentity>(option.DespawnObjectCapacity);
		}

		public void Clear()
		{
			SpawnObjects.Clear();
			TraceObjects.Clear();
			DespawnObjects.Clear();
		}
	}
}
