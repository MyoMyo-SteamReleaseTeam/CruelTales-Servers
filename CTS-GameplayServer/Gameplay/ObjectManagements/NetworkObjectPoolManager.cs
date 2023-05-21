using System;
using System.Collections.Generic;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay.ObjectManagements
{
	public class NetworkObjectPoolManager
	{
		Dictionary<Type, INetworkObjectPool> _netObjectPoolByType = new()
		{
			{ typeof(PlayerCharacter), new NetworkObjectPool<PlayerCharacter>(7) },
			{ typeof(TestCube), new NetworkObjectPool<TestCube>(120) },
		};

		public T Create<T>() where T : MasterNetworkObject, new()
		{
			return (T)_netObjectPoolByType[typeof(T)].Get();
		}

		public void Return(MasterNetworkObject netObject)
		{
			_netObjectPoolByType[netObject.GetType()].Return(netObject);
		}
	}
}
