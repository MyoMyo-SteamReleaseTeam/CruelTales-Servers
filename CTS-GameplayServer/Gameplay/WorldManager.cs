using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Tools;
using CT.Common.Tools.Collections;
using CTS.Instance.Gameplay.ObjectManagements;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.Gameplay
{
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
		private Dictionary<UserSession, PlayerVisibleTable> _playerVisibleBySession;

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
			_playerVisibleTablePool = new(() => new PlayerVisibleTable(_option), option.SystemMaxUser);
			_playerVisibleBySession = new(option.SystemMaxUser);
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
			foreach (var p in _playerVisibleBySession)
			{
				// TODO : Visible 테이블 갱신 (위치 및 카메라 컬링, 조건 검사)

				// TODO : 데이터 전송 (Life cycle -> Reliable -> Unreliable)

				// TODO : Visible 테이블 갱신 (스폰 -> 추적, 디스폰 -> 제거)
			}
		}

		#region Session

		public PlayerVisibleTable CreatePlayerVisibleTable(UserSession userSession)
		{
			var playerVisibleTable = _playerVisibleTablePool.Get();
			_playerVisibleBySession.Add(userSession, playerVisibleTable);
			return playerVisibleTable;
		}

		public void DestroyNetworkPlayer(UserSession userSession)
		{
			if (!_playerVisibleBySession.TryGetValue(userSession, out var visibleTable))
			{
				_log.Error($"[{_gameplayInstance}] There is no {userSession}'s visible table!");
				return;
			}
			visibleTable.Clear();
			_playerVisibleTablePool.Return(visibleTable);
			_playerVisibleBySession.Remove(userSession);
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
