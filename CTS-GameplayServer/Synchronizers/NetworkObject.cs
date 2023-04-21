using CT.Common.Serialization;

namespace CTS.Instance.Synchronizations
{
	public abstract class NetworkObject
	{
		public abstract void SerializeSyncReliable(PacketWriter writer);
	}
}
