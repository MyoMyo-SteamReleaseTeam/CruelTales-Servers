using System.Collections.Generic;
using System.Diagnostics;
using CTS.Instance.Synchronizations;
using KaNet.Physics;
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

		// References
		private GameplayManager _gameplayManager;
		private WorldManager _worldManager;
		private WorldVisibilityManager _visibilityManager;
		private KaPhysicsWorld _physicsWorld;

		public NetworkObjectPool(int initialCapacity, 
								 WorldManager worldManager,
								 WorldVisibilityManager visibilityManager,
								 GameplayManager gameManager,
								 KaPhysicsWorld physicsWorld)
		{
			_worldManager = worldManager;
			_visibilityManager = visibilityManager;
			_gameplayManager = gameManager;
			_physicsWorld = physicsWorld;

			_initialCapacity = initialCapacity;
			_objectStack = new(_initialCapacity);
			for (int i = 0; i < _initialCapacity; i++)
			{
				var inst = new T();
				inst.BindReferences(_worldManager, _visibilityManager, _gameplayManager, _physicsWorld);
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

				var inst = new T();
				inst.BindReferences(_worldManager, _visibilityManager, _gameplayManager, _physicsWorld);
				inst.Constructor();
				return inst;
			}
			else
			{
				T netObj = (T)_objectStack.Pop();
				netObj.InitializeMasterProperties();
				netObj.InitializeRemoteProperties();
				return netObj;
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
