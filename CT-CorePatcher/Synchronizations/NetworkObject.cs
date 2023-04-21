using CT.Common.Serialization;

namespace CT.CorePatcher.Synchronizations
{
	public enum NetworkObjectType
	{
		None = 0,
		TestEntity,
	}

	public abstract class NetworkObject
	{
		public abstract void SerializeSyncReliable(PacketWriter writer);
	}

	public abstract class RemoteNetworkObject
	{
		public abstract void DeserializeSyncReliable(PacketReader reader);
	}
}
