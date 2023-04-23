using CT.Common.DataType;
using CT.Common.Serialization;
using CT.Common.Synchronizations;

namespace CTS.Instance.Synchronizations
{
	public abstract class MasterNetworkObject : IMasterSynchronizable
	{
		public NetworkIdentity Identity { get; protected set; }
		public abstract NetworkObjectType Type { get; }
		public abstract bool IsDirty { get; }
		public abstract void SerializeSyncReliable(PacketWriter writer);
		public abstract void SerializeSyncUnreliable(PacketWriter writer);
		public abstract void SerializeEveryProperty(PacketWriter writer);
		public abstract void ClearDirtyReliable();
		public abstract void ClearDirtyUnreliable();
	}

	public abstract class RemoteNetworkObject : IRemoteSynchronizable
	{
		public NetworkIdentity Identity { get; protected set; }
		public abstract NetworkObjectType Type { get; }
		public abstract void DeserializeSyncReliable(PacketReader reader);
		public abstract void DeserializeEveryProperty(PacketReader reader);
		public abstract void DeserializeSyncUnreliable(PacketReader reader);
	}
}
