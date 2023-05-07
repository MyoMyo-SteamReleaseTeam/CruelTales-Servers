using CT.Packets;

namespace CT.Common.Serialization
{
	public abstract class PacketBase : IPacketSerializable
	{
		public abstract PacketType PacketType { get; }
		public abstract int SerializeSize { get; }
		public abstract void Deserialize(PacketReader reader);
		public abstract void Serialize(PacketWriter writer);
		public static void Ignore(PacketReader reader) => throw new System.NotImplementedException();
	}
}
