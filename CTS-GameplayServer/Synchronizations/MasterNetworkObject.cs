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
	public abstract class MasterNetworkObject : IMasterSynchronizable
	{
		/// <summary>게임 매니져 입니다.</summary>
		[AllowNull] protected GameManager _gameManager;

		/// <summary>네트워크 객체가 속해있는 World 입니다.</summary>
		[AllowNull] protected WorldManager _worldManager;

		/// <summary>네트워크 가시성 매니져입니다.</summary>
		[AllowNull] private WorldVisibilityManager _worldPartitioner;

		/// <summary>네트워크 객체의 식별자입니다.</summary>
		public NetworkIdentity Identity { get; protected set; } = new NetworkIdentity();

		public UserId Owner { get; protected set; } = new UserId(0);
		public Faction Faction { get; protected set; } = Faction.System;

		/// <summary>네트워크 객체의 Transform입니다.</summary>
		public NetworkTransform Transform { get; private set; } = new NetworkTransform();

		/// <summary>네트워크 객체의 오브젝트 타입입니다.</summary>
		public abstract NetworkObjectType Type { get; }

		/// <summary>네트워크 객체가 보일 조건을 결정합니다.</summary>
		public abstract VisibilityType Visibility { get; }

		/// <summary>네트워크 객체가 보일 대상을 결정합니다.</summary>
		public abstract VisibilityAuthority VisibilityAuthority { get; }

		/// <summary>네트워크 객체가 활성화된 상태인지 여부입니다.</summary>
		public bool IsAlive { get; private set; } = false;

		/// <summary>위치가 고정된 네트워크 객체인지 여부입니다.</summary>
		public bool IsStatic { get; }

		public MasterNetworkObject() {}

		public Vector2Int CurrentCellPos { get; private set; }

		/// <summary>네트워크 객체를 갱신합니다. 게임 로직에서 호출해서는 안됩니다.</summary>
		public void Update(float deltaTime)
		{
			// 고정 물리 업데이트를 수행합니다.
			if (!IsStatic)
			{
				fixedUpdate(deltaTime);
			}

			OnUpdate(deltaTime);
		}

		/// <summary>월드에서의 Cell 위치를 갱신합니다.</summary>
		public void UpdateWorldCell()
		{
			if (Visibility == VisibilityType.View)
			{
				Vector2Int previousPos = CurrentCellPos;
				CurrentCellPos = WorldVisibilityManager.GetWorldCell(Transform.Position);
				_worldPartitioner.OnCellChanged(this, previousPos, CurrentCellPos);
			}
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
						   WorldVisibilityManager worldPartitioner,
						   GameManager gameManager,
						   NetworkIdentity id,
						   Vector3 position)
		{
			// Initialize
			Identity = id;
			IsAlive = true;

			// Bind reference
			_worldManager = manager;
			_gameManager = gameManager;

			// Set position
			Transform.Initialize(position);
			if (Visibility == VisibilityType.View)
			{
				_worldPartitioner = worldPartitioner;
				CurrentCellPos = WorldVisibilityManager.GetWorldCell(Transform.Position);
				_worldPartitioner.OnCreated(this);
			}
		}

		/// <summary>객체를 삭제합니다. 다음 프레임에 삭제됩니다.</summary>
		public void Destroy()
		{
			IsAlive = false;
			_worldManager.AddDestroyStack(this);

			if (Visibility == VisibilityType.View)
			{
				_worldPartitioner.OnDestroy(this);
			}
		}

		/// <summary>객체를 해제합니다.</summary>
		public void Dispose()
		{
		}

		#region Visibility authority

		public bool IsValidVisibilityAuthority(NetworkPlayer networkPlayer)
		{
			switch (VisibilityAuthority)
			{
				case VisibilityAuthority.All:
					return true;

				case VisibilityAuthority.Owner:
					return networkPlayer.UserId == Owner;

				case VisibilityAuthority.Faction:
					return networkPlayer.Faction == Faction;

				default:
					return false;
			}
		}

		public virtual bool IsOwner(NetworkPlayer networkPlayer)
		{
			return Owner == networkPlayer.UserId;
		}

		public virtual bool IsSameFaction(NetworkPlayer networkPlayer)
		{
			return Faction == networkPlayer.Faction;
		}

		#endregion

		public abstract bool IsDirtyReliable { get; }
		public abstract bool IsDirtyUnreliable { get; }
		public abstract void SerializeSyncReliable(IPacketWriter writer);
		public abstract void SerializeSyncUnreliable(IPacketWriter writer);
		public abstract void SerializeEveryProperty(IPacketWriter writer);
		public abstract bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader);
		public abstract bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader);
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
		public abstract void InitializeProperties();
		public abstract void IgnoreSyncReliable(IPacketReader reader);
		public abstract void IgnoreSyncUnreliable(IPacketReader reader);
		public override string ToString() => $"Type:{Type}/Id:{Identity}";
	}
}
