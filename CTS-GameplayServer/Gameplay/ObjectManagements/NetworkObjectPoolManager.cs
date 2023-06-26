using System;
using System.Collections.Generic;
using CT.Networks;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Gameplay.ObjectManagements
{
	public class NetworkObjectPoolManager
	{
		Dictionary<Type, INetworkObjectPool> _netObjectPoolByType = new()
		{
			{ typeof(GameplayController), new NetworkObjectPool<GameplayController>(1) },
			{ typeof(PlayerCharacter), new NetworkObjectPool<PlayerCharacter>(1 * GlobalNetwork.SYSTEM_MAX_USER) },
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