using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Common.Tools;
using CT.Common.Tools.Collections;
using CTS.Instance.Networks;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class GameplayManager
	{
		// Log
		private readonly static ILog _log = LogManager.GetLogger(typeof(GameplayManager));

		// Reference
		public ServerOption ServerOption { get; private set; }
		private GameplayInstance _gameplayInstance;
		public RoomOption RoomOption { get; private set; }
		public WorldManager WorldManager { get; private set; }

		// Options
		public InstanceInitializeOption Option { get; private set; }

		// Manage Players
		private BidirectionalMap<UserId, NetworkPlayer> _networkPlayerByUserId;
		public IList<NetworkPlayer> NetworkPlayers { get; private set; }
		[AllowNull] private ObjectPool<NetworkPlayer> _networkPlayerPool;
		public int PlayerCount => _networkPlayerByUserId.Count;

		// Objects
		public GameplayController? GameplayController { get; private set; }

		public GameplayManager(GameplayInstance gameplayInstance,
							   ServerOption serverOption,
							   InstanceInitializeOption option)
		{
			// Reference
			ServerOption = serverOption;
			_gameplayInstance = gameplayInstance;
			RoomOption = _gameplayInstance.RoomOption;
			Option = option;

			// Manage Players
			_networkPlayerByUserId = new(Option.SystemMaxUser);
			NetworkPlayers = new List<NetworkPlayer>(Option.SystemMaxUser);

			// Initialize world
			WorldManager = new WorldManager(_gameplayInstance, this, Option);

			// Initialize pool
			_networkPlayerPool = new(() => new NetworkPlayer(this, WorldManager, Option),
									 Option.SystemMaxUser);
		}

		public void Reset()
		{
			WorldManager.Reset();
			GameplayController = WorldManager.CreateObject<GameplayController>();
		}

		public void Update(float deltaTime)
		{
			// Update world network logic of objects
			WorldManager.UpdateNetworkObjects(deltaTime);

			// Update coroutines
			WorldManager.UpdateCoroutine(deltaTime);

			// Update network player
			foreach (var player in _networkPlayerByUserId.ForwardValues)
			{
				player.Update(deltaTime);
			}

			// Send sync data to each user
			WorldManager.UpdateVisibilityAndSendData();

			// Update network objects physics
			WorldManager.FixedUpdate(deltaTime);

			// Update world partitions
			WorldManager.UpdateWorldPartitions();

			// Reset dirtys
			WorldManager.ClearDirtys();

			// Update objects life cycle
			WorldManager.UpdateObjectLifeCycle();
		}

		public void OnUserEnterGame(UserSession userSession)
		{
			var player = _networkPlayerPool.Get();
			player.OnCreated(userSession);
			_networkPlayerByUserId.Add(userSession.UserId, player);
			NetworkPlayers.Add(player);
			WorldManager.OnPlayerEnter(player);
			GameplayController?.OnPlayerEnter(player);
		}

		public void OnUserLeaveGame(UserSession userSession)
		{
			if (!_networkPlayerByUserId.TryGetValue(userSession.UserId, out var player))
			{
				_log.Error($"[{_gameplayInstance}] There is no {userSession}'s network player!");
				return;
			}

			GameplayController?.OnPlayerLeave(player);
			WorldManager.OnPlayerLeave(player);
			NetworkPlayers.Remove(player);
			_networkPlayerByUserId.TryRemove(player);
			player.OnDestroyed();
			_log.Debug($"[{_gameplayInstance}] Session {userSession} leave the game");

			if (PlayerCount <= 0)
			{
				Reset();
			}
		}

		public bool TryGetNetworkPlayer(UserId user, [MaybeNullWhen(false)] out NetworkPlayer player)
		{
			return _networkPlayerByUserId.TryGetForward(user, out player);
		}

		public bool IsConnectedPlayer(UserId user)
		{
			return _networkPlayerByUserId.Contains(user);
		}
	}
}
