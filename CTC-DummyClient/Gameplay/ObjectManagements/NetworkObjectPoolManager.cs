using System.Collections.Generic;
using CTC.Networks.Synchronizations;
using CTC.Networks.SyncObjects.TestSyncObjects;

namespace CTC.Networks.Gameplay.ObjectManagements
{
	public class NetworkObjectPoolManager
	{
		Dictionary<NetworkObjectType, INetworkObjectPool> _netObjectPoolByType = new()
		{
			{ NetworkObjectType.PlayerCharacter, new NetworkObjectPool<PlayerCharacter>(7) },
			{ NetworkObjectType.TestCube, new NetworkObjectPool<TestCube>(120) },
		};

		public RemoteNetworkObject Create(NetworkObjectType type)
		{
			return _netObjectPoolByType[type].Get();
		}

		public void Return(RemoteNetworkObject netObject)
		{
			_netObjectPoolByType[netObject.Type].Return(netObject);
		}
	}
}
