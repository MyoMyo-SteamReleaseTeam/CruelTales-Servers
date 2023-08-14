using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.Data;
using CTS.Instance.Gameplay;
using CTS.Instance.SyncObjects;
using KaNet.Physics;
using KaNet.Physics.RigidBodies;

namespace CTS.Instance.Synchronizations
{
	public abstract class MasterNetworkObject : IMasterSynchronizable
	{
		/// <summary>게임 매니져 입니다.</summary>
		[AllowNull] public GameplayManager GameplayManager { get; private set; }

		/// <summary>네트워크 객체가 속해있는 World 입니다.</summary>
		[AllowNull] public WorldManager WorldManager { get; private set; }

		/// <summary>네트워크 가시성 매니져입니다.</summary>
		[AllowNull] private WorldVisibilityManager _worldVisibilityManager;

		/// <summary>네트워크 객체의 식별자입니다.</summary>
		public NetworkIdentity Identity { get; protected set; } = new NetworkIdentity();

		/// <summary>객체를 소유한 유저의 ID입니다.</summary>
		public UserId Owner { get; protected set; } = new UserId(0);

		/// <summary>유저의 소속입니다.</summary>
		public Faction Faction { get; protected set; } = Faction.System;

		/// <summary>물리 강체 입니다.</summary>
		public readonly NetRigidBody RigidBody;
		private readonly KaRigidBody _physicsRigidBody;

		/// <summary>물리 계산 월드입니다.</summary>
		private KaPhysicsWorld? _physicsWorld;

		/// <summary>네트워크 객체의 오브젝트 타입입니다.</summary>
		public abstract NetworkObjectType Type { get; }

		/// <summary>네트워크 객체가 보일 조건을 결정합니다.</summary>
		public abstract VisibilityType Visibility { get; }

		/// <summary>네트워크 객체가 보일 대상을 결정합니다.</summary>
		public VisibilityAuthority VisibilityAuthority { get; protected set; }
		public abstract VisibilityAuthority InitialVisibilityAuthority { get; }

		/// <summary>네트워크 객체가 활성화된 상태인지 여부입니다.</summary>
		public bool IsAlive { get; private set; } = false;

		public Vector2Int CurrentCellPos { get; private set; }

		public MasterNetworkObject()
		{
			_physicsRigidBody = EntityRigidBodyDB.CreateRigidBodyBy(Type);
			RigidBody = new NetRigidBody(_physicsRigidBody);
		}

		/// <summary>객체의 생성자입니다. 단 한 번만 호출됩니다.</summary>
		public virtual void Constructor() { }

		/// <summary>물리를 갱신합니다. 게임 로직에서 호출해서는 안됩니다.</summary>
		public virtual void OnFixedUpdate(float stepTime) {}

		/// <summary>월드에서의 Cell 위치를 갱신합니다.</summary>
		public void UpdateWorldCell()
		{
			if (Visibility == VisibilityType.View)
			{
				Vector2Int previousPos = CurrentCellPos;
				CurrentCellPos = WorldVisibilityManager.GetWorldCell(_physicsRigidBody.Position);
				_worldVisibilityManager.OnCellChanged(this, previousPos, CurrentCellPos);
			}
		}

		/// <summary>객체가 삭제되었을 때 호출됩니다.</summary>
		public virtual void OnDestroyed() { }

		/// <summary>객체가 생성되었을 때 호출됩니다.</summary>
		public virtual void OnCreated() { }

		/// <summary>객체가 갱신되었을 때 호출됩니다.</summary>
		public virtual void OnUpdate(float deltaTime) { }

		/// <summary>생성된 객체를 초기화합니다.</summary>
		public void Initialize(WorldManager worldManager,
							   WorldVisibilityManager worldPartitioner,
							   GameplayManager gameManager,
							   KaPhysicsWorld physicsWorld,
							   NetworkIdentity id,
							   Vector2 position,
							   float rotation)
		{
			// Bind reference
			WorldManager = worldManager;
			_worldVisibilityManager = worldPartitioner;
			GameplayManager = gameManager;
			_physicsWorld = physicsWorld;

			// Initialize
			Identity = id;
			IsAlive = true;

			VisibilityAuthority = InitialVisibilityAuthority;

			// Initialize physics
			RigidBody.Reset();
			RigidBody.MoveTo(position);
			RigidBody.RotateTo(rotation);
			_physicsWorld.AddRigidBody(_physicsRigidBody);
		}

		public void InitializeAfterFrame()
		{
			OnCreated();

			// Add to trace visibility
			if (Visibility == VisibilityType.View)
			{
				CurrentCellPos = WorldVisibilityManager.GetWorldCell(RigidBody.Position);
			}
			_worldVisibilityManager.OnCreated(this);
		}

		/// <summary>객체를 해제합니다.</summary>
		public void Dispose()
		{
			Debug.Assert(_physicsWorld != null);
			_physicsWorld.RemoveRigidBody(_physicsRigidBody);
		}

		/// <summary>객체를 삭제합니다. 다음 프레임에 삭제됩니다.</summary>
		public void Destroy()
		{
			IsAlive = false;
			WorldManager.AddDestroyEqueue(this);

			if (Visibility == VisibilityType.View)
			{
				_worldVisibilityManager.OnDestroy(this);
			}
		}

		#region Visibility authority

		public bool IsValidVisibilityAuthority(NetworkPlayer networkPlayer)
		{
			if (this.Visibility == VisibilityType.View && !networkPlayer.CanSeeViewObject)
			{
				return false;
			}

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
		public abstract void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer);
		public abstract void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer);
		public abstract void SerializeEveryProperty(IPacketWriter writer);
		public abstract bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader);
		public abstract bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader);
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
		public abstract void InitializeMasterProperties();
		public abstract void InitializeRemoteProperties();
		public abstract void IgnoreSyncReliable(IPacketReader reader);
		public abstract void IgnoreSyncUnreliable(IPacketReader reader);
		public override string ToString() => $"Type:{Type}/Id:{Identity}";
	}
}
