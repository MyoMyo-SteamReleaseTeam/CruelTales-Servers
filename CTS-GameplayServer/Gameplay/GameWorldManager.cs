using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CT.Common.Tools.Collections;
using CTS.Instance.Gameplay.ObjectManagements;
using CTS.Instance.Networks;
using CTS.Instance.Synchronizations;
using CTS.Instance.SyncObjects;
using log4net;

namespace CTS.Instance.Gameplay
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SynchronizeJob
	{
		public SyncOperation Operation;
		public fixed byte Data[2048];

		public void CopyFrom(SyncOperation operation, PacketReader reader)
		{
			Operation = operation;
		}
	}

	public class GameWorldManager : IUpdatable//, IJobHandler<SynchronizeJob>
	{
		// Log
		private static ILog _log = LogManager.GetLogger(typeof(GameWorldManager));

		// Network Object Management
		private BidirectionalMap<NetworkIdentity, MasterNetworkObject> _worldObjectById = new();
		private WorldPartitioner _worldPartition;
		private NetworkObjectPoolManager _objectPoolManager;
		/// <summary>프레임이 끝나면 삭제될 객체 목록입니다.</summary>
		private Stack<MasterNetworkObject> _destroyObjectStack = new(16);
		private ushort _idCounter = 1;

		public GameWorldManager()
		{
			_worldPartition = new WorldPartitioner(12);
			_objectPoolManager = new NetworkObjectPoolManager();
		}

		public void Update(float deltaTime)
		{
			// Update every objects
			foreach (var netObj in _worldObjectById.ForwardValues)
			{
				netObj.FixedUpdate(deltaTime);
				netObj.Update(deltaTime);
			}

			// Remove objects
			while (_destroyObjectStack.Count > 0)
			{
				destroyObject(_destroyObjectStack.Pop());
			}
		}

		public void UpdateDeserialize()
		{

		}

		public void UpdateSerialize()
		{

		}

		public T CreateObject<T>(Vector3 position = default) where T : MasterNetworkObject, new()
		{
			var netObj = _objectPoolManager.Create<T>();
			_worldObjectById.Add(netObj.Identity, netObj);
			netObj.Initialize(this, _worldPartition, getNetworkIdentityCounter(), position);
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
				return;
			}

			netObject.OnDestroy();
			_objectPoolManager.Return(netObject);
		}

		public void AddPlayer(UserSession session)
		{
			var playerEntity = CreateObject<NetworkPlayer>();
			playerEntity.BindUser(session);
		}
	}
}
