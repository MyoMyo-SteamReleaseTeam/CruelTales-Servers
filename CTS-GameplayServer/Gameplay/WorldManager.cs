using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Tools.Collections;
using CT.Networks;
using CT.Packets;
using CTS.Instance.Coroutines;
using CTS.Instance.Gameplay.ObjectManagements;
using CTS.Instance.Synchronizations;
using KaNet.Physics;
using log4net;

namespace CTS.Instance.Gameplay
{
	public class WorldManager
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(WorldManager));

		// Reference
		private GameplayInstance _gameplayInstance;
		private GameplayManager _gameplayManager;
		private InstanceInitializeOption _option;

		// Network Object Management
		private BidirectionalMap<NetworkIdentity, MasterNetworkObject> _networkObjectById = new();
		private NetworkObjectPoolManager _objectPoolManager;

		/// <summary>프레임이 끝나면 삭제될 객체 목록입니다.</summary>
		private Queue<MasterNetworkObject> _destroyObjectQueue;

		/// <summary>프레임이 끝나면 생성될 객체 목록입니다.</summary>
		private Queue<MasterNetworkObject> _createObjectQueue;

		private NetworkIdentity _idCounter;

		// Visibility
		private WorldVisibilityManager _visibilityManager;

		// Physics World
		private KaPhysicsWorld _physicsWorld;
		private float _deltaAccumulator = 0;
		private float _stepTime = 0.03f;
		
		// Coroutine
		private CoroutineRuntime _coroutineRuntime;

		// Getter
		public int Count => _networkObjectById.Count;

		public WorldManager(GameplayInstance gameplayInstance,
							GameplayManager gameplayManager,
							InstanceInitializeOption option)
		{
			// Reference
			_gameplayInstance = gameplayInstance;
			_gameplayManager = gameplayManager;
			_option = option;

			// Network Object Management
			_destroyObjectQueue = new(option.WorldMaximumObjectCount);
			_createObjectQueue = new(option.WorldMaximumObjectCount);

			// Partitioner
			_visibilityManager = new(gameplayInstance, this, option);

			// Physics world
			_physicsWorld = new KaPhysicsWorld();
			_stepTime = _gameplayManager.ServerOption.PhysicsStepTime;

			// Object Pool Manager
			_objectPoolManager = new(this, _visibilityManager, gameplayManager, _physicsWorld);

			// Coroutine
			_coroutineRuntime = new(option.CoroutineCapacity);
		}

		public void Reset()
		{
			Clear();
			_coroutineRuntime.Reset();
			_visibilityManager.Reset();
		}

		public void SetMiniGameMapData(MiniGameMapData mapData)
		{
			_physicsWorld.SetStaticRigidBodies(mapData.StaticRigidBodies);
		}

		public void ReleaseMiniGameMapData()
		{
			_physicsWorld.ReleaseStaticRigidBodies();
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

		#region Update funtions

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateNetworkObjects(float deltaTime)
		{
			foreach (var netObj in _networkObjectById.ForwardValues)
			{
				if (!netObj.IsAlive)
					continue;

				netObj.OnUpdate(deltaTime);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateCoroutine(float deltaTime)
		{
			_coroutineRuntime.Flush(deltaTime);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateVisibilityAndSendData()
		{
			this._visibilityManager.UpdateVisibilityAndSendData();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FixedUpdate(float deltaTime)
		{
			_deltaAccumulator += deltaTime;
			if (_deltaAccumulator > _stepTime * 5)
				_deltaAccumulator = _stepTime * 5;

			while (_deltaAccumulator >= _stepTime)
			{
				_deltaAccumulator -= _stepTime;
				_physicsWorld.Step(_stepTime);

				foreach (var netObj in _networkObjectById.ForwardValues)
				{
					if (!netObj.IsAlive)
						continue;

					netObj.OnFixedUpdate(deltaTime);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDirtys()
		{
			foreach (var netObj in _networkObjectById.ForwardValues)
			{
				netObj.ClearDirtyReliable();
				netObj.ClearDirtyUnreliable();
				netObj.RigidBody.ClearDirty();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateObjectLifeCycle()
		{
			while (_createObjectQueue.Count > 0)
			{
				var createdObj = _createObjectQueue.Dequeue();
				createdObj.InitializeAfterFrame();
				_networkObjectById.Add(createdObj.Identity, createdObj);
			}

			while (_destroyObjectQueue.Count > 0)
			{
				var destroyObj = _destroyObjectQueue.Dequeue();
				_objectPoolManager.Return(destroyObj);
				if (!_networkObjectById.TryRemove(destroyObj))
				{
					_log.Error($"There is no network object to remove. " +
						$"Object : [{destroyObj.GetType().Name}: {destroyObj.Identity}]");
					Debug.Assert(false);
					return;
				}

				destroyObj.OnDestroyed();
				destroyObj.Dispose();
			}
		}

		#endregion

		#region Coroutine

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StartCoroutine(CoroutineActionVoid coroutineAction)
		{
			_coroutineRuntime.Start(coroutineAction);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StartCoroutine(CoroutineActionArg coroutineAction)
		{
			_coroutineRuntime.Start(coroutineAction);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StartCoroutine(CoroutineActionArgs2 coroutineAction)
		{
			_coroutineRuntime.Start(coroutineAction);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StartCoroutine(CoroutineActionArgs3 coroutineAction)
		{
			_coroutineRuntime.Start(coroutineAction);
		}

		#endregion

		#region Life Cycle

		public T CreateObject<T>(Vector2 position = default) where T : MasterNetworkObject, new()
		{
			var netObj = _objectPoolManager.Create<T>();
			netObj.InitializeMasterProperties();
			netObj.InitializeRemoteProperties();
			netObj.Initialize(getNetworkIdentityCounter(),
							  position, 
							  rotation: 0);
			_createObjectQueue.Enqueue(netObj);

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

		public void AddDestroyEqueue(MasterNetworkObject networkObject)
		{
			_destroyObjectQueue.Enqueue(networkObject);
		}

		public void Clear()
		{
			_idCounter = new NetworkIdentity(0);

			foreach (MasterNetworkObject netObj in _networkObjectById.ForwardValues)
			{
				netObj.Destroy();
			}

			while (_createObjectQueue.TryDequeue(out var netObj))
			{
				netObj.Destroy();
			}
		}

		public void ClearWithoutDontDestroy()
		{

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
				SerializeCreationData(_reliableBuffer,
									  visibleTable.SpawnObjects,
									  PacketType.SC_Sync_MasterSpawn);
				// Reliable data
				SerializeReliableData(player, _reliableBuffer,
									  visibleTable.SpawnObjects,
									  PacketType.SC_Sync_MasterReliable);
			}

			// Serialize first global spawn data
			if (visibleTable.GlobalSpawnObjects.Count != 0)
			{
				SerializeCreationData(_reliableBuffer,
									  visibleTable.GlobalSpawnObjects,
									  PacketType.SC_Sync_MasterSpawn);
				// Reliable data
				SerializeReliableData(player, _reliableBuffer,
									  visibleTable.GlobalSpawnObjects,
									  PacketType.SC_Sync_MasterReliable);
			}

			// Serialize enter data
			if (visibleTable.EnterObjects.Count != 0)
			{
				SerializeCreationData(_reliableBuffer,
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
				SerializePhysicsEventData(_mtuBuffer,
										  visibleTable.TraceObjects,
										  PacketType.SC_Sync_MasterPhysics);
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
				SerializeDestructionData(_reliableBuffer,
										 visibleTable.LeaveObjects,
										 PacketType.SC_Sync_MasterLeave);
			}

			// Serialize Desapwn data
			if (visibleTable.DespawnObjects.Count != 0)
			{
				SerializeDestructionData(_reliableBuffer,
										 visibleTable.DespawnObjects,
										 PacketType.SC_Sync_MasterDespawn);
			}

			// Serialize Global Desapwn data
			if (visibleTable.GlobalDespawnObjects.Count != 0)
			{
				SerializeDestructionData(_reliableBuffer,
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

		public static void SerializeCreationData(IPacketWriter writer,
												 Dictionary<NetworkIdentity, MasterNetworkObject> netObjs,
												 PacketType packetType)
		{
			writer.Put(packetType);
			writer.Put((byte)netObjs.Count);

			foreach (var spawnObj in netObjs.Values)
			{
				writer.Put(spawnObj.Type);
				writer.Put(spawnObj.Identity);
				spawnObj.RigidBody.SerializeInitialSyncData(writer);
				spawnObj.SerializeEveryProperty(writer);
			}
		}

		public static void SerializeDestructionData(IPacketWriter writer,
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

		public static void SerializePhysicsEventData(IPacketWriter writer,
													 Dictionary<NetworkIdentity, MasterNetworkObject> netObjs,
													 PacketType packetType)
		{
			int originSize = writer.Size;
			writer.OffsetSize(OFFSET_SIZE);

			// Serialize data
			int syncCount = 0;
			foreach (var syncObj in netObjs.Values)
			{
				NetRigidBody rigidBody = syncObj.RigidBody;
				if (rigidBody.IsDirty)
				{
					writer.Put(syncObj.Identity);
					rigidBody.SerializeEventSyncData(writer);
					syncCount++;
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
			if (!_gameplayManager.TryGetNetworkPlayer(sender, out var player))
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

				if (!netObj.TryDeserializeSyncReliable(player, reader))
				{
					reader.IgnoreAll();
					return false;
				}
			}

			return true;
		}

		public bool OnRemoteUnreliable(UserId sender, IPacketReader reader)
		{
			if (!_gameplayManager.TryGetNetworkPlayer(sender, out var player))
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
