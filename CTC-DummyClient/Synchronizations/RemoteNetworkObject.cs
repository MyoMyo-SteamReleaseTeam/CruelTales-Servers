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
		public abstract void SerializeSyncReliable(PacketWriter writer);
		public abstract void SerializeSyncUnreliable(PacketWriter writer);
		public abstract void SerializeEveryProperty(PacketWriter writer);
		public abstract void DeserializeSyncReliable(PacketReader reader);
		public abstract void DeserializeEveryProperty(PacketReader reader);
		public abstract void DeserializeSyncUnreliable(PacketReader reader);
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
		public static void IgnoreSyncReliable(PacketReader reader) { }
		public static void IgnoreSyncUnreliable(PacketReader reader) { }
	}
}