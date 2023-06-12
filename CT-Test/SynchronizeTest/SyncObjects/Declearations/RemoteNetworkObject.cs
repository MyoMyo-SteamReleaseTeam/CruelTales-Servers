using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTC.Networks.Synchronizations
{
	public abstract class RemoteNetworkObject : IRemoteSynchronizable
	{
		/// <summary>네트워크 객체의 Transform입니다.</summary>
		public NetworkTransform Transform { get; private set; } = new NetworkTransform();

		/// <summary>네트워크 객체의 식별자입니다.</summary>
		public NetworkIdentity Identity { get; protected set; }

		/// <summary>네트워크 객체의 오브젝트 타입입니다.</summary>
		public abstract NetworkObjectType Type { get; }

		/// <summary>네트워크 객체가 활성화된 상태인지 여부입니다.</summary>
		public bool IsAlive { get; private set; } = false;

		/// <summary>객체가 삭제되었을 때 호출됩니다.</summary>
		public virtual void OnDestroyed() { }

		/// <summary>객체가 생성되었을 때 호출됩니다.</summary>
		public virtual void OnSpawn() { }

		/// <summary>객체가 스폰 되었을 때 호출됩니다.</summary>
		public virtual void OnRespawn() { }

		/// <summary>객체가 갱신되었을 때 호출됩니다.</summary>
		public virtual void OnUpdate(float deltaTime) { }

		public void Created(NetworkIdentity id)
		{
			IsAlive = true;
			Identity = id;
		}

		/// <summary>객체를 삭제합니다. 다음 프레임에 삭제됩니다.</summary>
		public void Destroy()
		{
			IsAlive = false;
		}

		public abstract bool IsDirtyReliable { get; }
		public abstract bool IsDirtyUnreliable { get; }
		public abstract void SerializeSyncReliable(IPacketWriter writer);
		public abstract void SerializeSyncUnreliable(IPacketWriter writer);
		public abstract void SerializeEveryProperty(IPacketWriter writer);
		public abstract bool TryDeserializeSyncReliable(IPacketReader reader);
		public abstract bool TryDeserializeEveryProperty(IPacketReader reader);
		public abstract bool TryDeserializeSyncUnreliable(IPacketReader reader);
		public abstract void InitializeProperties();
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
		public abstract void IgnoreSyncReliable(IPacketReader reader);
		public abstract void IgnoreSyncUnreliable(IPacketReader reader);
	}
}