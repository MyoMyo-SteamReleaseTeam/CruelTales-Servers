using System.Collections.Generic;
using System.Diagnostics;
using CTC.Networks.Synchronizations;
using log4net;

namespace CTC.Networks.Gameplay.ObjectManagements
{
	public interface INetworkObjectPool
	{
		public void Return(RemoteNetworkObject networkObject);
		public RemoteNetworkObject Get();
	}

	public class NetworkObjectPool<T> : INetworkObjectPool where T : RemoteNetworkObject, new()
	{
		private static ILog _log = LogManager.GetLogger(typeof(NetworkObjectPool<T>));

		public int Count => _objectStack.Count;
		private Stack<RemoteNetworkObject> _objectStack;
		private int _initialCapacity;

		public NetworkObjectPool(int initialCapacity)
		{
			_initialCapacity = initialCapacity;
			_objectStack = new(_initialCapacity);
			for (int i = 0; i < _initialCapacity; i++)
			{
				var inst = new T();
				_objectStack.Push(inst);
			}
		}

		public RemoteNetworkObject Get()
		{
			if (_objectStack.Count == 0)
			{
				_log.Warn($"More network object creation requests have occurred than the initial capacity. Type : {typeof(T)}");
				Debug.Assert(false);
				return new T();
			}
			else
			{
				return (T)_objectStack.Pop();
			}
		}

		public void Return(RemoteNetworkObject networkObject)
		{
			if (networkObject == null)
				return;

			_objectStack.Push(networkObject);
		}
	}
}
