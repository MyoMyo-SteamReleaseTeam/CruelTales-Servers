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
			{ typeof(TestCube), new NetworkObjectPool<TestCube>(120) },
			{ typeof(PlayerCharacter), new NetworkObjectPool<PlayerCharacter>(1 * GlobalNetwork.SYSTEM_MAX_USER) },
			{ typeof(ZTest_Value8), new NetworkObjectPool<ZTest_Value8>(16) },
			{ typeof(ZTest_Value16), new NetworkObjectPool<ZTest_Value16>(16) },
			{ typeof(ZTest_Value32), new NetworkObjectPool<ZTest_Value32>(16) },
			
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