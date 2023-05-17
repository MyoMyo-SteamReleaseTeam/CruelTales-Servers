using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Synchronizations
{
	public abstract class MasterNetworkObject : ISynchronizable, IUpdatable
	{
		/// <summary>네트워크 객체의 식별자입니다.</summary>
		public NetworkIdentity Identity { get; protected set; } = new NetworkIdentity();

		/// <summary>네트워크 객체의 Transform입니다.</summary>
		public NetworkTransform Transform { get; private set; } = new NetworkTransform();

		/// <summary>네트워크 객체의 오브젝트 타입입니다.</summary>
		public abstract NetworkObjectType Type { get; }

		/// <summary>네트워크 객체의 네트워크 가시성 타입입니다.</summary>
		public NetworkVisibility Visibility { get; protected set; } = NetworkVisibility.Distance;

		[AllowNull] private WorldManager _worldManager;
		[AllowNull] private WorldPartitioner _worldPartitioner;

		/// <summary>네트워크 객체가 활성화된 상태인지 여부입니다.</summary>
		public bool IsAlive { get; private set; } = false;

		/// <summary>위치가 고정된 네트워크 객체인지 여부입니다.</summary>
		public bool IsStatic { get; }

		public MasterNetworkObject() {}

		private Vector2Int _currentCellPos;

		/// <summary>네트워크 객체를 갱신합니다. 게임 로직에서 호출해서는 안됩니다.</summary>
		public void Update(float deltaTime)
		{
			Vector2Int previousPos = _currentCellPos;

			// 고정 물리 업데이트를 수행합니다.
			if (!IsStatic)
			{
				fixedUpdate(deltaTime);
			}

			_currentCellPos = WorldPartitioner.GetWorldCell(Transform.Position);
			_worldPartitioner.OnCellChanged(Identity, previousPos, _currentCellPos);
		}

		/// <summary>네트워크 객체의 고정 물리 업데이트입니다.</summary>
		private void fixedUpdate(float deltaTime)
		{
			Transform.Update(deltaTime);
		}

		/// <summary>객체가 삭제되었을 때 호출됩니다.</summary>
		public virtual void OnDestroyed() { }

		/// <summary>객체가 생성되었을 때 호출됩니다.</summary>
		public virtual void OnCreated() { }

		/// <summary>객체가 갱신되었을 때 호출됩니다.</summary>
		public virtual void OnUpdate(float deltaTime) { }

		/// <summary>객체를 초기화합니다.</summary>
		public void Create(WorldManager manager,
							   WorldPartitioner worldPartitioner,
							   NetworkIdentity id,
							   Vector3 position)
		{
			// Initialize
			Identity = id;
			IsAlive = true;

			// Bind reference
			_worldManager = manager;
			_worldPartitioner = worldPartitioner;

			// Set position
			Transform.SetPosition(position);
			_currentCellPos = WorldPartitioner.GetWorldCell(Transform.Position);
			_worldPartitioner.OnCreated(id, _currentCellPos);
		}

		/// <summary>객체를 삭제합니다. 다음 프레임에 삭제됩니다.</summary>
		public void Destroy()
		{
			IsAlive = false;
			_worldManager.AddDestroyStack(this);
			_worldPartitioner.OnDestroy(Identity, _currentCellPos);
		}

		/// <summary>객체를 해제합니다.</summary>
		public void Dispose()
		{
		}

		public abstract bool IsDirtyReliable { get; }
		public abstract bool IsDirtyUnreliable { get; }
		public abstract void SerializeSyncReliable(IPacketWriter writer);
		public abstract void SerializeSyncUnreliable(IPacketWriter writer);
		public abstract void SerializeEveryProperty(IPacketWriter writer);
		public abstract void DeserializeSyncReliable(IPacketReader reader);
		public abstract void DeserializeEveryProperty(IPacketReader reader);
		public abstract void DeserializeSyncUnreliable(IPacketReader reader);
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
		public abstract void IgnoreSyncReliable(IPacketReader reader);
		public abstract void IgnoreSyncUnreliable(IPacketReader reader);
		public override string ToString() => $"Type:{Type}/Id:{Identity}";
	}
}
