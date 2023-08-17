using System.Collections.Generic;
using CTC.Networks.Synchronizations;
using CTC.Networks.SyncObjects.TestSyncObjects;

namespace CTC.Networks.Gameplay.ObjectManagements
{
	public class NetworkObjectPoolManager
	{
		Dictionary<NetworkObjectType, INetworkObjectPool> _netObjectPoolByType = new()
		{
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
