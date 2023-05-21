﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;
using CT.Networks;
using CT.Packets;
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
		private BidirectionalMap<NetworkIdentity, MasterNetworkObject> _networkObjectById = new();
		private NetworkObjectPoolManager _objectPoolManager;
		/// <summary>프레임이 끝나면 삭제될 객체 목록입니다.</summary>
		private Stack<MasterNetworkObject> _destroyObjectStack;
		private NetworkIdentity _idCounter;

		// Visibility
		private WorldVisibilityManager _visibilityManager;

		public WorldManager(GameplayInstance gameplayInstance, InstanceInitializeOption option)
		{
			// Reference
			_gameplayInstance = gameplayInstance;
			_option = option;

			// Network Object Management
			_objectPoolManager = new();
			_destroyObjectStack = new(option.DestroyObjectStackCapacity);

			// Partitioner
			_visibilityManager = new(gameplayInstance, this, option);
		}

		public void Update(float deltaTime)
		{
			// Update every objects
			foreach (var netObj in _networkObjectById.ForwardValues)
			{
				if (!netObj.IsAlive)
					continue;

				// Update positions and logic
				netObj.Update(deltaTime);
			}

			// Remove objects
			while (_destroyObjectStack.Count > 0)
			{
				destroyObject(_destroyObjectStack.Pop());
			}
		}

		public void UpdateVisibilityAndSendData()
		{
			this._visibilityManager.UpdateVisibilityAndSendData();
		}

		public void ClearDirtys()
		{
			foreach (var netObj in _networkObjectById.ForwardValues)
			{
				netObj.ClearDirtyReliable();
				netObj.ClearDirtyUnreliable();
				netObj.Transform.ClearDirty();
			}
		}

		public void OnPlayerEnter(NetworkPlayer player)
		{
			this._visibilityManager.CreatePlayerVisibleTable(player);
		}

		public void OnPlayerLeave(NetworkPlayer player)
		{
			this._visibilityManager.DestroyNetworkPlayer(player);
		}

		public bool TryGetNetworkObject(NetworkIdentity id,
									    [MaybeNullWhen(false)]
										out MasterNetworkObject networkObject)
		{
			return _networkObjectById.TryGetValue(id, out networkObject);
		}

		#region Life Cycle

		public T CreateObject<T>(Vector3 position = default) where T : MasterNetworkObject, new()
		{
			var netObj = _objectPoolManager.Create<T>();
			netObj.Create(this, _visibilityManager, getNetworkIdentityCounter(), position);
			_networkObjectById.Add(netObj.Identity, netObj);
			netObj.OnCreated();
			return netObj;

			NetworkIdentity getNetworkIdentityCounter()
			{
				for (int i = 0; i < NetworkIdentity.MaxValue; i++)
				{
					_idCounter++;

					if (_networkObjectById.ContainsForward(_idCounter))
						continue;

					return _idCounter;
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
			_idCounter = new NetworkIdentity(0);
			_destroyObjectStack.Clear();
			var ids = _networkObjectById.ForwardKeys;
			int removeCount = _networkObjectById.Count;
			Span<NetworkIdentity> removeIds = stackalloc NetworkIdentity[removeCount];
			for (int i = 0; i < removeCount; i++)
			{
				destroyObject(_networkObjectById.GetValue(removeIds[i]));
			}
		}

		private void destroyObject(MasterNetworkObject netObject)
		{
			_objectPoolManager.Return(netObject);
			if (!_networkObjectById.TryRemove(netObject))
			{
				_log.Error($"There is no network object to remove. Object : [{netObject.Identity}]");
				Debug.Assert(false);
				return;
			}

			netObject.Dispose();
			netObject.OnDestroyed();
		}

		#endregion

		#region Synchronizaion

		// TODO : Need to pool packet
		private ByteBuffer _reliableBuffer = new ByteBuffer(16 * 1024);
		private ByteBuffer _mtuBuffer = new ByteBuffer(GlobalNetwork.MTU);

		public void SendSynchronization(UserSession userSession,
											   PlayerVisibleTable visibleTable)
		{
			_reliableBuffer.ResetWriter();

			// Serialize first spawn data
			if (visibleTable.SpawnObjects.Count != 0)
			{
				SerializeSpawnData(_reliableBuffer,
								   visibleTable.SpawnObjects,
								   PacketType.SC_Sync_MasterSpawn);
			}

			// Serialize respawn data
			if (visibleTable.RespawnObjects.Count != 0)
			{
				SerializeSpawnData(_reliableBuffer,
								   visibleTable.RespawnObjects,
								   PacketType.SC_Sync_MasterRespawn);
			}

			// Serialize despawn data
			if (visibleTable.DespawnObjects.Count != 0)
			{
				SerializeDespawnData(_reliableBuffer,
									 visibleTable.DespawnObjects,
									 PacketType.SC_Sync_MasterDespawn);
			}

			// Serialize reliable and unreliable trace object data
			if (visibleTable.TraceObjects.Count != 0)
			{
				// Reliable data
				SerializeReliableData(_reliableBuffer,
									  visibleTable.TraceObjects,
									  PacketType.SC_Sync_MasterReliable);

				// TODO : Fragment the packet by MTU
				_mtuBuffer.ResetWriter();

				// Unreliable data
				SerializeUnreliableData(_mtuBuffer,
										visibleTable.TraceObjects,
										PacketType.SC_Sync_MasterUnreliable);

				// Serialize movement data
				SerializeMovementData(_mtuBuffer,
									  visibleTable.TraceObjects,
									  PacketType.SC_Sync_MasterMovement);

				// Send unreliable data
				if (_mtuBuffer.Size > 0)
				{
					userSession.SendReliable(_mtuBuffer);
				}
			}

			// Send reliable data
			if (_reliableBuffer.Size > 0)
			{
				userSession.SendReliable(_reliableBuffer);
			}
		}

		public void SerializeSpawnData(IPacketWriter writer,
									   HashSet<MasterNetworkObject> netObjs,
									   PacketType packetType)
		{
			writer.Put(packetType);
			writer.Put((byte)netObjs.Count);

			foreach (var spawnObj in netObjs)
			{
				writer.Put(spawnObj.Type);
				writer.Put(spawnObj.Identity);
				spawnObj.Transform.SerializeSpawnData(writer);
				spawnObj.SerializeEveryProperty(writer);
			}
		}

		public void SerializeDespawnData(IPacketWriter writer,
										 HashSet<MasterNetworkObject> netObjs,
										 PacketType packetType)
		{
			writer.Put(packetType);
			writer.Put((byte)netObjs.Count);

			foreach (var spawnObj in netObjs)
			{
				writer.Put(spawnObj.Identity);
			}
		}

		private const int OFFSET_SIZE = sizeof(PacketType) + sizeof(byte);
		public void SerializeReliableData(IPacketWriter writer,
										  HashSet<MasterNetworkObject> netObjs,
										  PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			foreach (var spawnObj in netObjs)
			{
				if (spawnObj.IsDirtyReliable)
				{
					spawnObj.SerializeSyncReliable(writer);
				}
			}

			// Revert size if there is no serialized data
			if (originSize == writer.Size - OFFSET_SIZE)
			{
				writer.SetSize(originSize);
			}
			else
			{
				int serializeSize = writer.Size;
				writer.SetSize(originSize);
				writer.Put(packetType);
				writer.Put((byte)netObjs.Count);
				writer.SetSize(serializeSize);
			}
		}

		public void SerializeUnreliableData(IPacketWriter writer,
											HashSet<MasterNetworkObject> netObjs,
											PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			foreach (var spawnObj in netObjs)
			{
				if (spawnObj.IsDirtyUnreliable)
				{
					spawnObj.SerializeSyncUnreliable(writer);
				}
			}

			// Revert size if there is no serialized data
			if (originSize == writer.Size - OFFSET_SIZE)
			{
				writer.SetSize(originSize);
			}
			else
			{
				int serializeSize = writer.Size;
				writer.SetSize(originSize);
				writer.Put(packetType);
				writer.Put((byte)netObjs.Count);
				writer.SetSize(serializeSize);
			}
		}

		public void SerializeMovementData(IPacketWriter writer,
										  HashSet<MasterNetworkObject> netObjs,
										  PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			foreach (var spawnObj in netObjs)
			{
				NetworkTransform transform = spawnObj.Transform;
				if (transform.IsDirty)
				{
					transform.Serialize(writer);
				}
			}

			// Revert size if there is no serialized data
			if (originSize == writer.Size - OFFSET_SIZE)
			{
				writer.SetSize(originSize);
			}
			else
			{
				int serializeSize = writer.Size;
				writer.SetSize(originSize);
				writer.Put(packetType);
				writer.Put((byte)netObjs.Count);
				writer.SetSize(serializeSize);
			}
		}

		public void OnDeserializeSyncReliable(IPacketReader reader)
		{
			while (reader.CanRead(1))
			{
				NetworkIdentity id = new NetworkIdentity();
				id.Deserialize(reader);
				if (_networkObjectById.TryGetValue(id, out var netObj))
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
				if (_networkObjectById.TryGetValue(id, out var netObj))
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
