using System;
using System.Collections.Generic;
using CT.Common.DataType;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay.ObjectManagements
{
	public interface INetworkObjectPool
	{
		public void Return(MasterNetworkObject networkObject);
		public MasterNetworkObject Get();
	}

	public class NetworkObjectPool<T> : INetworkObjectPool where T : MasterNetworkObject, new()
	{
		private static ILog _log = LogManager.GetLogger(typeof(NetworkObjectPool<T>));

		public int Count => _objectStack.Count;
		private Stack<MasterNetworkObject> _objectStack;
		public Type Type { get; private set; }
		private int _initialCapacity;

		public NetworkObjectPool(Type networkObjectType, int initialCapacity)
		{
			Type = networkObjectType;
			_initialCapacity = initialCapacity;
			_objectStack = new(_initialCapacity);
			for (int i = 0; i < _initialCapacity; i++)
			{
				var inst = new T();
				_objectStack.Push(inst);
			}
		}

		public MasterNetworkObject Get()
		{
			MasterNetworkObject inst;

			if (_objectStack.Count == 0)
			{
				_log.Warn($"More network object creation requests have occurred than the initial capacity. Type : {Type}");
				inst = new T();
			}
			else
			{
				inst = (T)_objectStack.Pop();
			}

			return (T)inst;
		}

		public void Return(MasterNetworkObject networkObject)
		{
			if (networkObject == null)
				return;

			_objectStack.Push(networkObject);
		}
	}
}
