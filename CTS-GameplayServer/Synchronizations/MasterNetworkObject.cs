using CT.Common.DataType;
using CT.Common.Gameplay;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTS.Instance.SyncObjects;

namespace CTS.Instance.Synchronizations
{
	public abstract class MasterNetworkObject : ISynchronizable, IUpdatable
	{
		public NetworkTransform Transform { get; private set; }

		public NetworkIdentity Identity { get; protected set; }
		public abstract NetworkObjectType Type { get; }

		public MasterNetworkObject()
		{
			Transform = new NetworkTransform();
		}

		public virtual void Update(float deltaTime)
		{
			Transform.Update(deltaTime);
		}

		public void OnCreated(NetworkIdentity id)
		{
			Identity = id;
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
		public static void IgnoreSyncReliable(PacketReader reader) => throw new System.NotImplementedException();
		public static void IgnoreSyncUnreliable(PacketReader reader) => throw new System.NotImplementedException();
	}
}
