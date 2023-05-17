using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CT.Networks;
using CTS.Instance.Gameplay.ObjectManagements;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class PlayerVisibleTable
	{
		/// <summary>무시할 가시성 특성입니다.</summary>
		public NetworkVisibility IgnoreVisibility = NetworkVisibility.None;

		private NetworkPlayer _networkPlayer;

		public PlayerVisibleTable(NetworkPlayer networkPlayer)
		{
			networkPlayer = networkPlayer;
		}
	}

	public class WorldManager : IUpdatable
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(WorldManager));

		// Network Object Management
		private BidirectionalMap<NetworkIdentity, MasterNetworkObject> _worldObjectById = new();
		private WorldPartitioner _worldPartition;
		private NetworkObjectPoolManager _objectPoolManager;
		/// <summary>프레임이 끝나면 삭제될 객체 목록입니다.</summary>
		private Stack<MasterNetworkObject> _destroyObjectStack = new(16);
		private ushort _idCounter = 1;

		public WorldManager()
		{
			_worldPartition = new WorldPartitioner(12);
			_objectPoolManager = new NetworkObjectPoolManager();
		}

		public void Update(float deltaTime)
		{
			// Update every objects
			foreach (var netObj in _worldObjectById.ForwardValues)
			{
				if (!netObj.IsAlive)
					return;

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
	}
}
