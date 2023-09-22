using System;
using System.Collections.Generic;
using CT.Networks;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;
using KaNet.Physics;

namespace CTS.Instance.Gameplay.ObjectManagements
{
	public class NetworkObjectPoolManager
	{
		Dictionary<Type, INetworkObjectPool> _netObjectPoolByType;

		public NetworkObjectPoolManager(WorldManager worldManager,
										WorldVisibilityManager visibilityManager,
										GameplayManager gameManager,
										KaPhysicsWorld physicsWorld)
		{
			_netObjectPoolByType = new()
			{
				{ typeof(CameraController), new NetworkObjectPool<CameraController>(1 * GlobalNetwork.SYSTEM_MAX_USER, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(GameplayController), new NetworkObjectPool<GameplayController>(1, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(CustomLobby_SceneController), new NetworkObjectPool<CustomLobby_SceneController>(2, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(Dueoksini_MiniGameController), new NetworkObjectPool<Dueoksini_MiniGameController>(2, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(MiniGameControllerBase), new NetworkObjectPool<MiniGameControllerBase>(2, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(RedHood_MiniGameController), new NetworkObjectPool<RedHood_MiniGameController>(2, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(SceneControllerBase), new NetworkObjectPool<SceneControllerBase>(2, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(NormalCharacter), new NetworkObjectPool<NormalCharacter>(1 * GlobalNetwork.SYSTEM_MAX_USER, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(PlayerCharacter), new NetworkObjectPool<PlayerCharacter>(1 * GlobalNetwork.SYSTEM_MAX_USER, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(RedHoodCharacter), new NetworkObjectPool<RedHoodCharacter>(1 * GlobalNetwork.SYSTEM_MAX_USER, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(WolfCharacter), new NetworkObjectPool<WolfCharacter>(1 * GlobalNetwork.SYSTEM_MAX_USER, worldManager, visibilityManager, gameManager, physicsWorld) },
				{ typeof(TestCube), new NetworkObjectPool<TestCube>(120, worldManager, visibilityManager, gameManager, physicsWorld) },
				
			};
		}

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