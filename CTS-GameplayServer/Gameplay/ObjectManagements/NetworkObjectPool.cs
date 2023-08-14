using System.Collections.Generic;
using System.Diagnostics;
using CTS.Instance.Synchronizations;
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
		private int _initialCapacity;

		public NetworkObjectPool(int initialCapacity)
		{
			_initialCapacity = initialCapacity;
			_objectStack = new(_initialCapacity);
			for (int i = 0; i < _initialCapacity; i++)
			{
				var inst = new T();
				inst.Constructor();
				_objectStack.Push(inst);
			}
		}

		public MasterNetworkObject Get()
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

		public void Return(MasterNetworkObject networkObject)
		{
			if (networkObject == null)
				return;

			_objectStack.Push(networkObject);
		}
	}
}
