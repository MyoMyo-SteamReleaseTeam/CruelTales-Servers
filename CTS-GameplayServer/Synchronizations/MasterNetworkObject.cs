using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.Gameplay;
using CTS.Instance.Gameplay.ObjectManagements;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Synchronizations
{
	[Flags]
	public enum NetworkVisibility : byte
	{
		/// <summary>모든 대상에게 보입니다.</summary>
		Static = 0b_0000_0001,

		/// <summary>소유자에게만 보입니다.</summary>
		OwnerOnly = 0b_0000_0010,

		/// <summary>가까운 거리에서만 보입니다.</summary>
		Distance = 0b_0000_0100,
	}

	public abstract class MasterNetworkObject : ISynchronizable, IUpdatable
	{
		public NetworkTransform Transform { get; private set; } = new NetworkTransform();
		public NetworkIdentity Identity { get; protected set; } = new NetworkIdentity();
		public abstract NetworkObjectType Type { get; }

		[AllowNull] private GameWorldManager _worldManager;
		[AllowNull] private WorldPartitioner _worldPartitioner;

		public bool IsAlived { get; private set; } = false;
		public MasterNetworkObject() {}

		public void FixedUpdate(float deltaTime)
		{
			Transform.Update(deltaTime);
			// TODO : Calculate world partition position
		}

		public virtual void Update(float deltaTime) { }
		public virtual void OnDestroy() { }
		public virtual void OnCreated() { }

		public void Initialize(GameWorldManager manager,
							   WorldPartitioner worldPartitioner,
							   NetworkIdentity id,
							   Vector3 position)
		{
			IsAlived = true;
			_worldManager = manager;
			_worldPartitioner = worldPartitioner;
			Identity = id;
			Transform.SetPosition(position);
		}

		public void Destroy()
		{
			IsAlived = false;
			_worldManager.AddDestroyStack(this);
		}

		public abstract bool IsDirtyReliable { get; }
		public abstract bool IsDirtyUnreliable { get; }
		public abstract void SerializeSyncReliable(PacketWriter writer);
		public abstract void SerializeSyncUnreliable(PacketWriter writer);
		public abstract void SerializeEveryProperty(PacketWriter writer);
		public abstract void DeserializeSyncReliable(PacketReader reader);
		public abstract void DeserializeEveryProperty(PacketReader reader);
		public abstract void DeserializeSyncUnreliable(PacketReader reader);
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
		public abstract void IgnoreSyncReliable(PacketReader reader);
		public abstract void IgnoreSyncUnreliable(PacketReader reader);
		public override string ToString() => $"Type:{Type}/Id:{Identity}";
	}
}
