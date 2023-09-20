using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.Coroutines;
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

		/// <summary>게임 컨트롤러 입니다.</summary>
		[AllowNull] public GameplayController GameplayController
			=> GameplayManager.GameplayController;

		/// <summary>게임 세션 메니져입니다.</summary>
		[AllowNull] public RoomSessionManager RoomSessionManager
			=> GameplayController.RoomSessionManager;

		/// <summary>네트워크 객체가 속해있는 World 입니다.</summary>
		[AllowNull] public WorldManager WorldManager { get; private set; }

		/// <summary>네트워크 가시성 매니져입니다.</summary>
		[AllowNull] private WorldVisibilityManager _worldVisibilityManager;

		/// <summary>물리 계산 월드입니다.</summary>
		[AllowNull] public KaPhysicsWorld PhysicsWorld { get; private set; }

		/// <summary>네트워크 객체의 식별자입니다.</summary>
		public NetworkIdentity Identity { get; protected set; } = new NetworkIdentity();

		/// <summary>객체를 소유한 유저의 ID입니다.</summary>
		public UserId Owner { get; protected set; } = new UserId(0);

		/// <summary>유저의 소속입니다.</summary>
		public Faction Faction { get; protected set; } = Faction.System;

		/// <summary>물리 강체 입니다.</summary>
		public readonly NetRigidBody RigidBody;
		protected readonly KaRigidBody _physicsRigidBody;

		/// <summary>네트워크 객체의 오브젝트 타입입니다.</summary>
		public abstract NetworkObjectType Type { get; }

		/// <summary>네트워크 객체가 보일 조건을 결정합니다.</summary>
		public abstract VisibilityType Visibility { get; }

		/// <summary>네트워크 객체가 보일 대상을 결정합니다.</summary>
		public VisibilityAuthority VisibilityAuthority { get; protected set; }
		public abstract VisibilityAuthority InitialVisibilityAuthority { get; }

		/// <summary>네트워크 객체가 활성화된 상태인지 여부입니다.</summary>
		public bool IsAlive { get; private set; } = false;

		/// <summary>객체의 위치입니다.</summary>
		public Vector2 Position => RigidBody.Position;

		public Vector2Int CurrentCellPos { get; private set; }

		/// <summary>미니게임이 변경되더라도 객체가 유지되는지 여부입니다.</summary>
		public bool IsPersistent { get; set; }

		/// <summary>코루틴 호출 카운터입니다.</summary>
		private int _coroutineCallCounter = 0;

		/// <summary>실행 대기중인 코루틴입니다.</summary>
		private HashSet<CoroutineIdentity> _wattingCoroutines;

		public MasterNetworkObject()
		{
			_physicsRigidBody = EntityRigidBodyDB.CreateRigidBodyBy(Type);
			_physicsRigidBody.BindAction(onCollisionWith);
			RigidBody = new NetRigidBody(_physicsRigidBody);
			_wattingCoroutines = new HashSet<CoroutineIdentity>(16);
		}

		public void BindReferences(WorldManager worldManager,
								   WorldVisibilityManager worldPartitioner,
								   GameplayManager gameManager,
								   KaPhysicsWorld physicsWorld)
		{
			WorldManager = worldManager;
			_worldVisibilityManager = worldPartitioner;
			GameplayManager = gameManager;
			PhysicsWorld = physicsWorld;
		}

		/// <summary>생성된 객체를 초기화합니다.</summary>
		public void Initialize(NetworkIdentity id,
							   Vector2 position,
							   float rotation)
		{
			// Initialize
			Identity = id;
			IsAlive = true;
			VisibilityAuthority = InitialVisibilityAuthority;

			// Initialize physics
			_physicsRigidBody.Initialize(Identity);
			_physicsRigidBody.MoveTo(position);
			_physicsRigidBody.RotateTo(rotation);
			_physicsRigidBody.LinearVelocity = Vector2.Zero;
			_physicsRigidBody.ForceVelocity = Vector2.Zero;
			_physicsRigidBody.ForceFriction = 0;
			PhysicsWorld.AddRigidBody(_physicsRigidBody);
		}

		public void InitializeAfterFrame()
		{
			OnCreated();

			// Add to trace visibility
			if (Visibility == VisibilityType.View)
			{
				CurrentCellPos = WorldVisibilityManager.GetWorldCell(_physicsRigidBody.Position);
			}
			_worldVisibilityManager.OnCreated(this);
		}

		/// <summary>객체를 해제합니다.</summary>
		public void Dispose()
		{
			Debug.Assert(PhysicsWorld != null);
			PhysicsWorld.RemoveRigidBody(_physicsRigidBody);
			_wattingCoroutines.Clear();
			_coroutineCallCounter = 0;
		}

		/// <summary>객체를 삭제합니다. 다음 프레임에 삭제됩니다.</summary>
		public void Destroy()
		{
			if (!IsAlive)
				return;

			IsAlive = false;
			WorldManager.AddDestroyEqueue(this);
			_worldVisibilityManager.OnDestroy(this);
		}

		/// <summary>객체를 강제로 삭제합니다. WorldManager에서는 사라지지 않습니다.</summary>
		public void ForceDestroy()
		{
			if (!IsAlive)
				return;

			IsAlive = false;
			//_worldVisibilityManager.OnDestroy(this);

			OnDestroyed();
			Dispose();
		}

		private void onCollisionWith(int id)
		{
			if (WorldManager.TryGetNetworkObject(new(id), out var networkObject))
			{
				OnCollisionWith(networkObject);
			}
		}

		/// <summary>특정 객체와 충돌했을 때 발생합니다.</summary>
		public virtual void OnCollisionWith(MasterNetworkObject collideObject) { }

		/// <summary>객체의 사용자 정의 생성자입니다. 단 한 번만 호출됩니다.</summary>
		public virtual void Constructor() { }

		/// <summary>물리를 갱신합니다. 게임 로직에서 호출해서는 안됩니다.</summary>
		public virtual void OnFixedUpdate(float stepTime) { }

		/// <summary>객체가 삭제되었을 때 호출됩니다.</summary>
		public virtual void OnDestroyed() { }

		/// <summary>객체가 생성되었을 때 호출됩니다.</summary>
		public virtual void OnCreated() { }

		/// <summary>객체가 갱신되었을 때 호출됩니다.</summary>
		public virtual void OnUpdate(float deltaTime) { }

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

		#region Coroutine

		public CoroutineIdentity StartCoroutine(Action cachedAction, float delay)
		{
			CoroutineIdentity coroutineId = new(Identity, _coroutineCallCounter);
			CoroutineActionVoid coroutineAction = new CoroutineActionVoid
			(
				callerObject: this,
				identity: coroutineId,
				action: cachedAction,
				delay: delay
			);
			WorldManager.StartCoroutine(coroutineAction);
			_wattingCoroutines.Add(coroutineId);
			_coroutineCallCounter++;
			return coroutineId;
		}

		public CoroutineIdentity StartCoroutine(Action<Arg> cachedAction, Arg argument, float delay)
		{
			CoroutineIdentity coroutineId = new(Identity, _coroutineCallCounter);
			CoroutineActionArg coroutineAction = new CoroutineActionArg
			(
				callerObject: this,
				identity: coroutineId,
				action: cachedAction,
				arg: argument,
				delay: delay
			);
			WorldManager.StartCoroutine(coroutineAction);
			_wattingCoroutines.Add(coroutineId);
			_coroutineCallCounter++;
			return coroutineId;
		}

		public CoroutineIdentity StartCoroutine(Action<Arg, Arg> cachedAction, Arg argument0, Arg argument1, float delay)
		{
			CoroutineIdentity coroutineId = new(Identity, _coroutineCallCounter);
			CoroutineActionArgs2 coroutineAction = new CoroutineActionArgs2
			(
				callerObject: this,
				identity: coroutineId,
				action: cachedAction,
				arg0: argument0,
				arg1: argument1,
				delay: delay
			);
			WorldManager.StartCoroutine(coroutineAction);
			_wattingCoroutines.Add(coroutineId);
			_coroutineCallCounter++;
			return coroutineId;
		}

		public CoroutineIdentity StartCoroutine(Action<Arg, Arg, Arg> cachedAction,
												Arg argument0, Arg argument1, Arg argument2, float delay)
		{
			CoroutineIdentity coroutineId = new(Identity, _coroutineCallCounter);
			CoroutineActionArgs3 coroutineAction = new CoroutineActionArgs3
			(
				callerObject: this,
				identity: coroutineId,
				action: cachedAction,
				arg0: argument0,
				arg1: argument1,
				arg2: argument2,
				delay: delay
			);
			WorldManager.StartCoroutine(coroutineAction);
			_wattingCoroutines.Add(coroutineId);
			_coroutineCallCounter++;
			return coroutineId;
		}

		public CoroutineIdentity StartCoroutine(Action<Arg, Arg, Arg, Arg> cachedAction,
												Arg argument0, Arg argument1, Arg argument2, Arg argument3, float delay)
		{
			CoroutineIdentity coroutineId = new(Identity, _coroutineCallCounter);
			CoroutineActionArgs4 coroutineAction = new CoroutineActionArgs4
			(
				callerObject: this,
				identity: coroutineId,
				action: cachedAction,
				arg0: argument0,
				arg1: argument1,
				arg2: argument2,
				arg3: argument3,
				delay: delay
			);
			WorldManager.StartCoroutine(coroutineAction);
			_wattingCoroutines.Add(coroutineId);
			_coroutineCallCounter++;
			return coroutineId;
		}

		public CoroutineIdentity StartCoroutine(Action<Arg, Arg, Arg, Arg, Arg> cachedAction,
												Arg argument0, Arg argument1, Arg argument2, Arg argument3, Arg argument4, float delay)
		{
			CoroutineIdentity coroutineId = new(Identity, _coroutineCallCounter);
			CoroutineActionArgs5 coroutineAction = new CoroutineActionArgs5
			(
				callerObject: this,
				identity: coroutineId,
				action: cachedAction,
				arg0: argument0,
				arg1: argument1,
				arg2: argument2,
				arg3: argument3,
				arg4: argument4,
				delay: delay
			);
			WorldManager.StartCoroutine(coroutineAction);
			_wattingCoroutines.Add(coroutineId);
			_coroutineCallCounter++;
			return coroutineId;
		}

		public void CancelCoroutine(CoroutineIdentity coroutineIdentity)
		{
			_wattingCoroutines.Remove(coroutineIdentity);
		}

		public bool TryPopWaittingCoroutine(CoroutineIdentity coroutineIdentity)
		{
			return _wattingCoroutines.Remove(coroutineIdentity);
		}

		#endregion

		protected bool _isDirtyReliable;
		protected bool _isDirtyUnreliable;
		public bool IsDirtyReliable => _isDirtyReliable;
		public bool IsDirtyUnreliable => _isDirtyUnreliable;
		public abstract void SerializeSyncReliable(NetworkPlayer player, IPacketWriter writer);
		public abstract void SerializeSyncUnreliable(NetworkPlayer player, IPacketWriter writer);
		public abstract void SerializeEveryProperty(IPacketWriter writer);
		public abstract bool TryDeserializeSyncReliable(NetworkPlayer player, IPacketReader reader);
		public abstract bool TryDeserializeSyncUnreliable(NetworkPlayer player, IPacketReader reader);
		public void MarkDirtyReliable() => _isDirtyReliable = true;
		public void MarkDirtyUnreliable() => _isDirtyUnreliable = true;
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
		public abstract void InitializeMasterProperties();
		public abstract void InitializeRemoteProperties();
		public abstract void IgnoreSyncReliable(IPacketReader reader);
		public abstract void IgnoreSyncUnreliable(IPacketReader reader);
		public override string ToString() => $"Type:{Type}/Id:{Identity}";
		public void BindOwner(IDirtyable owner) => throw new System.NotImplementedException();
	}
}