using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Tools;
using CT.Common.Tools.Collections;
using CTS.Instance.Gameplay.ObjectManagements;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class PlayerVisibleTable
	{
		// Reference
		private WorldPartitioner _partitioner;

		/// <summary>무시할 가시성 특성입니다.</summary>
		public NetworkVisibility IgnoreVisibility = NetworkVisibility.None;

		[AllowNull] private NetworkPlayer _networkPlayer;

		// Sync set
		private HashSet<NetworkIdentity> _spawnObjects;
		private HashSet<NetworkIdentity> _traceObjects;
		private HashSet<NetworkIdentity> _despawnObjects;

		// View boundary
		private Vector2 _viewInSize;
		private Vector2 _viewOutSize;

		public PlayerVisibleTable(WorldPartitioner worldPartitioner, InstanceInitializeOption option)
		{
			// Reference
			_partitioner = worldPartitioner;

			// Sync set
			_spawnObjects = new HashSet<NetworkIdentity>(option.SpawnObjectCapacity);
			_traceObjects = new HashSet<NetworkIdentity>(option.TraceObjectCapacity);
			_despawnObjects = new HashSet<NetworkIdentity>(option.DespawnObjectCapacity);

			// View boundary
			_viewInSize = option.ViewInSize;
			_viewOutSize = option.ViewOutSize;
		}

		public void BindPlayer(NetworkPlayer networkPlayer)
		{
			_networkPlayer = networkPlayer;
		}

		public void RemovePlayer()
		{
			_networkPlayer = null;
		}

		public void UpdateAndSend()
		{
			var hashSet = _partitioner.GetCell(_networkPlayer.Transform.Position);
			// TODO update visibility
		}
	}

	public class WorldManager : IUpdatable
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(WorldManager));

		// Reference
		private GameplayInstance _gameplayInstance;
		private InstanceInitializeOption _option;

		// Network Object Management
		private BidirectionalMap<NetworkIdentity, MasterNetworkObject> _worldObjectById = new();
		private NetworkObjectPoolManager _objectPoolManager;
		/// <summary>프레임이 끝나면 삭제될 객체 목록입니다.</summary>
		private Stack<MasterNetworkObject> _destroyObjectStack;
		private ushort _idCounter = 1;

		// Partitioner
		private WorldPartitioner _worldPartition;

		// Visible Table
		private ObjectPool<PlayerVisibleTable> _playerVisibleTablePool;
		private List<PlayerVisibleTable> _playerVisibleTableList;

		// Session
		private BidirectionalMap<UserSession, NetworkPlayer> _networkPlayerByUserSession;

		public WorldManager(GameplayInstance gameplayInstance, InstanceInitializeOption option)
		{
			// Reference
			_gameplayInstance = gameplayInstance;
			_option = option;

			// Network Object Management
			_objectPoolManager = new();
			_destroyObjectStack = new(option.DestroyObjectStackCapacity);

			// Partitioner
			_worldPartition = new(option.PartitionCellCapacity);

			// Visible Table
			_playerVisibleTablePool = new(() => new PlayerVisibleTable(_worldPartition, _option),
										  option.SystemMaxUser);
			_playerVisibleTableList = new(option.SystemMaxUser);

			// Session
			_networkPlayerByUserSession = new(option.SystemMaxUser);
		}

		public void Update(float deltaTime)
		{
			// Update every objects
			foreach (var netObj in _worldObjectById.ForwardValues)
			{
				if (!netObj.IsAlive)
					return;

				// Update positions and logic
				netObj.Update(deltaTime);
			}

			// Remove objects
			while (_destroyObjectStack.Count > 0)
			{
				destroyObject(_destroyObjectStack.Pop());
			}
		}

		public void UpdateSerialize()
		{

		}

		#region Session

		public NetworkPlayer CreateNetworkPlayer(UserSession userSession)
		{
			var playerVisibleTable = _playerVisibleTablePool.Get();
			var playerEntity = this.CreateObject<NetworkPlayer>();
			playerVisibleTable.BindPlayer(playerEntity);
			playerEntity.BindUserSession(userSession, playerVisibleTable);
			_playerVisibleTableList.Add(playerVisibleTable);
			_networkPlayerByUserSession.Add(userSession, playerEntity);
			return playerEntity;
		}

		public void DestroyNetworkPlayer(UserSession userSession)
		{
			if (!_networkPlayerByUserSession.TryGetValue(userSession, out var player))
			{
				_log.Error($"[{_gameplayInstance}] There is no {userSession}'s player in the world!");
				return;
			}
			var visibleTable = player.VisibleTable;
			player.RemoveUserSession();
			_networkPlayerByUserSession.TryRemove(player);
			if (visibleTable == null)
			{
				_log.Error($"[{_gameplayInstance}] There is no {userSession}'s visible table!");
				return;
			}
			visibleTable.RemovePlayer();
			_playerVisibleTableList.Remove(visibleTable);
		}

		#endregion

		#region Life Cycle

		public T CreateObject<T>(Vector3 position = default) where T : MasterNetworkObject, new()
		{
			var netObj = _objectPoolManager.Create<T>();
			_worldObjectById.Add(netObj.Identity, netObj);
			netObj.Create(this, _worldPartition, getNetworkIdentityCounter(), position);
			netObj.OnCreated();
			return netObj;

			NetworkIdentity getNetworkIdentityCounter()
			{
				for (int i = 0; i < ushort.MaxValue; i++)
				{
					if (_idCounter == 0)
						_idCounter++;

					var newId = new NetworkIdentity(_idCounter++);
					if (_worldObjectById.ContainsForward(newId))
						continue;

					return newId;
				}

				throw new IndexOutOfRangeException($"There are no more network identity");
			}
		}

		public void AddDestroyStack(MasterNetworkObject networkObject)
		{
			_destroyObjectStack.Push(networkObject);
		}

		public void Clear()
		{
			_idCounter = 1;
			_destroyObjectStack.Clear();
			var ids = _worldObjectById.ForwardKeys;
			int removeCount = _worldObjectById.Count;
			Span<NetworkIdentity> removeIds = stackalloc NetworkIdentity[removeCount];
			for (int i = 0; i < removeCount; i++)
			{
				destroyObject(_worldObjectById.GetValue(removeIds[i]));
			}
		}

		private void destroyObject(MasterNetworkObject netObject)
		{
			if (!_worldObjectById.TryRemove(netObject))
			{
				_log.Error($"There is no network object to remove. Object : [{netObject}]");
				Debug.Assert(false);
				return;
			}

			netObject.Dispose();
			netObject.OnDestroyed();
			_objectPoolManager.Return(netObject);
		}

		#endregion

		#region Synchronizaion

		public void OnDeserializeSyncReliable(IPacketReader reader)
		{
			while (reader.CanRead(1))
			{
				NetworkIdentity id = new NetworkIdentity();
				id.Deserialize(reader);
				if (_worldObjectById.TryGetValue(id, out var netObj))
				{
					netObj.DeserializeSyncReliable(reader);
				}
				else
				{
					_log.Warn($"{nameof(OnDeserializeSyncReliable)} ignored!");
					reader.IgnoreAll();
				}
			}
		}

		public void OnDeserializeSyncUnreliable(IPacketReader reader)
		{
			while (reader.CanRead(1))
			{
				NetworkIdentity id = new NetworkIdentity();
				id.Deserialize(reader);
				if (_worldObjectById.TryGetValue(id, out var netObj))
				{
					netObj.DeserializeSyncUnreliable(reader);
				}
				else
				{
					_log.Warn($"{nameof(OnDeserializeSyncUnreliable)} ignored!");
				}
			}
		}

		#endregion
	}
}
