using System;
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
using CTS.Instance.Synchronizations;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class WorldManager
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(WorldManager));

		// Reference
		private GameplayInstance _gameplayInstance;
		[AllowNull] private GameManager _gameManager;
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

		public void Initialize()
		{
			_gameManager = _gameplayInstance.GameManager;
		}

		public void UpdateNetworkObjects(float deltaTime)
		{
			foreach (var netObj in _networkObjectById.ForwardValues)
			{
				if (!netObj.IsAlive)
					continue;

				netObj.OnUpdate(deltaTime);
			}
		}

		public void UpdatePhysics(float deltaTime)
		{
			foreach (var netObj in _networkObjectById.ForwardValues)
			{
				if (!netObj.IsAlive)
					continue;

				netObj.UpdatePhysics(deltaTime);
			}
		}

		public void UpdateWorldPartitions()
		{
			// Update every objects
			foreach (var netObj in _networkObjectById.ForwardValues)
			{
				if (!netObj.IsAlive)
					continue;

				// Update positions and logic
				netObj.UpdateWorldCell();
			}
		}

		public void UpdateRemoveObjects()
		{
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
			if (_networkObjectById.TryGetValue(id, out networkObject))
			{
				return true;
			}

			Debug.Assert(false);
			return false;
		}

		#region Life Cycle

		public T CreateObject<T>(Vector3 position = default) where T : MasterNetworkObject, new()
		{
			var netObj = _objectPoolManager.Create<T>();
			netObj.InitializeMasterProperties();
			netObj.InitializeRemoteProperties();
			netObj.Create(this, _visibilityManager, _gameManager, getNetworkIdentityCounter(), position);
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
		private ByteBuffer _reliableBuffer = new ByteBuffer(64 * 1024);
		private ByteBuffer _mtuBuffer = new ByteBuffer(GlobalNetwork.MTU * 16);

		public void SendSynchronization(NetworkPlayer player,
										PlayerVisibleTable visibleTable)
		{
			if (player.Session == null)
				return;

			_reliableBuffer.ResetWriter();

			// Serialize first spawn data
			if (visibleTable.SpawnObjects.Count != 0)
			{
				SerializeSpawnData(_reliableBuffer,
								   visibleTable.SpawnObjects,
								   PacketType.SC_Sync_MasterSpawn);
			}

			// Serialize first global spawn data
			if (visibleTable.GlobalSpawnObjects.Count != 0)
			{
				SerializeSpawnData(_reliableBuffer,
								   visibleTable.GlobalSpawnObjects,
								   PacketType.SC_Sync_MasterSpawn);
			}

			// Serialize enter data
			if (visibleTable.EnterObjects.Count != 0)
			{
				SerializeSpawnData(_reliableBuffer,
								   visibleTable.EnterObjects,
								   PacketType.SC_Sync_MasterEnter);
			}

			// Serialize reliable and unreliable trace object data
			if (visibleTable.TraceObjects.Count != 0)
			{
				// Reliable data
				SerializeReliableData(player, _reliableBuffer,
									  visibleTable.TraceObjects,
									  PacketType.SC_Sync_MasterReliable);

				// TODO : Fragment the packet by MTU
				_mtuBuffer.ResetWriter();

				// Serialize movement data
				SerializeMovementData(_mtuBuffer,
									  visibleTable.TraceObjects,
									  PacketType.SC_Sync_MasterMovement);

				// Unreliable data
				SerializeUnreliableData(player, _mtuBuffer,
										visibleTable.TraceObjects,
										PacketType.SC_Sync_MasterUnreliable);
			}

			// Serialize reliable and unreliable global trace object data
			if (visibleTable.GlobalTraceObjects.Count != 0)
			{
				// Reliable data
				SerializeReliableData(player, _reliableBuffer,
									  visibleTable.GlobalTraceObjects,
									  PacketType.SC_Sync_MasterReliable);

				// Unreliable data
				SerializeUnreliableData(player, _mtuBuffer,
										visibleTable.GlobalTraceObjects,
										PacketType.SC_Sync_MasterUnreliable);
			}

			// Leave and Despawn data should be serialize after reliable data
			// Serialize leave data
			if (visibleTable.LeaveObjects.Count != 0)
			{
				SerializeDespawnData(_reliableBuffer,
									 visibleTable.LeaveObjects,
									 PacketType.SC_Sync_MasterLeave);
			}

			// Serialize Desapwn data
			if (visibleTable.DespawnObjects.Count != 0)
			{
				SerializeDespawnData(_reliableBuffer,
									 visibleTable.DespawnObjects,
									 PacketType.SC_Sync_MasterDespawn);
			}

			// Serialize Global Desapwn data
			if (visibleTable.GlobalDespawnObjects.Count != 0)
			{
				SerializeDespawnData(_reliableBuffer,
									 visibleTable.GlobalDespawnObjects,
									 PacketType.SC_Sync_MasterDespawn);
			}

			// Send unreliable data
			if (_mtuBuffer.Size > 0)
			{
				player.Session?.SendUnreliable(_mtuBuffer);
			}

			// Send reliable data
			if (_reliableBuffer.Size > 0)
			{
				player.Session?.SendReliable(_reliableBuffer);
			}
		}

		public static void SerializeSpawnData(IPacketWriter writer,
											  Dictionary<NetworkIdentity, MasterNetworkObject> netObjs,
											  PacketType packetType)
		{
			writer.Put(packetType);
			writer.Put((byte)netObjs.Count);

			foreach (var spawnObj in netObjs.Values)
			{
				writer.Put(spawnObj.Type);
				writer.Put(spawnObj.Identity);
				spawnObj.Transform.SerializeSpawnData(writer);
				spawnObj.SerializeEveryProperty(writer);

				// TODO : remove
				// DEBUG
				if (packetType == PacketType.SC_Sync_MasterSpawn)
				{
					Console.WriteLine($"{spawnObj.Identity}:{spawnObj.Transform} SPAWN");
				}
			}
		}

		public static void SerializeDespawnData(IPacketWriter writer,
												Dictionary<NetworkIdentity, MasterNetworkObject> netObjs,
												PacketType packetType)
		{
			writer.Put(packetType);
			writer.Put((byte)netObjs.Count);

			foreach (var spawnObjId in netObjs.Keys)
			{
				writer.Put(spawnObjId);
			}
		}

		private const int OFFSET_SIZE = sizeof(PacketType) + sizeof(byte);
		public static void SerializeReliableData(NetworkPlayer player, IPacketWriter writer,
												 Dictionary<NetworkIdentity, MasterNetworkObject> netObjs,
												 PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			int syncCount = 0;
			foreach (var syncObj in netObjs.Values)
			{
				if (syncObj.IsDirtyReliable)
				{
					int preSize = writer.OffsetSize(NetworkIdentity.SIZE);
					int curSize = writer.Size;
					syncObj.SerializeSyncReliable(player, writer);

					// It's serialized
					if (writer.Size != curSize)
					{
						syncCount++;
						writer.PutTo(syncObj.Identity, preSize);
					}
					else
					{
						writer.SetSize(preSize);
					}
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
				writer.Put((byte)syncCount);
				writer.SetSize(serializeSize);
			}
		}

		public static void SerializeUnreliableData(NetworkPlayer player, IPacketWriter writer,
												   Dictionary<NetworkIdentity, MasterNetworkObject> netObjs,
												   PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			int syncCount = 0;
			foreach (var syncObj in netObjs.Values)
			{
				if (syncObj.IsDirtyUnreliable)
				{
					int preSize = writer.OffsetSize(NetworkIdentity.SIZE);
					int curSize = writer.Size;
					syncObj.SerializeSyncUnreliable(player, writer);

					// It's serialized
					if (writer.Size != curSize)
					{
						syncCount++;
						writer.PutTo(syncObj.Identity, preSize);
					}
					else
					{
						writer.SetSize(preSize);
					}
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
				writer.Put((byte)syncCount);
				writer.SetSize(serializeSize);
			}
		}

		public static void SerializeMovementData(IPacketWriter writer,
												 Dictionary<NetworkIdentity, MasterNetworkObject> netObjs,
												 PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			int syncCount = 0;
			foreach (var syncObj in netObjs.Values)
			{
				NetworkTransform transform = syncObj.Transform;
				if (transform.IsDirty)
				{
					syncCount++;
					writer.Put(syncObj.Identity);
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
				writer.Put((byte)syncCount);
				writer.SetSize(serializeSize);
			}
		}

		// TODO : 역직렬화 실패시 Network Player 내부의 UserSession으로 연결을 종료하도록 리펙토링 할 수 있음
		public bool OnRemoteReliable(UserId sender, IPacketReader reader)
		{
			if (!_gameManager.TryGetNetworkPlayer(sender, out var player))
			{
				return false;
			}

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				if (!_networkObjectById.TryGetValue(id, out var netObj))
				{
					return true;
				}

				if (!netObj.TryDeserializeSyncUnreliable(player, reader))
				{
					reader.IgnoreAll();
					return false;
				}
			}

			return true;
		}

		public bool OnRemoteUnreliable(UserId sender, IPacketReader reader)
		{
			if (!_gameManager.TryGetNetworkPlayer(sender, out var player))
			{
				return false;
			}

			int objCount = reader.ReadByte();
			for (int i = 0; i < objCount; i++)
			{
				NetworkIdentity id = new(reader);
				if (!_networkObjectById.TryGetValue(id, out var netObj))
				{
					return true;
				}

				if (!netObj.TryDeserializeSyncUnreliable(player, reader))
				{
					reader.IgnoreAll();
					return false;
				}
			}

			return true;
		}

		#endregion
	}
}
