using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;
using CTC.Networks.SyncObjects.TestSyncObjects;

namespace CTC.Networks.Synchronizations
{
	public abstract class RemoteNetworkObject : ISynchronizable
	{
		public NetworkIdentity Identity { get; protected set; }
		public abstract NetworkObjectType Type { get; }

		public void OnCreated(NetworkIdentity id)
		{
			Identity = id;
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
	}
}