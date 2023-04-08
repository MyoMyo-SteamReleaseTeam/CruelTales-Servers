using CT.Packets;

namespace CT.Network.Serialization
{
	public static class PacketExtension
	{
		public static PacketType PeekPacketType(this PacketReader reader)
		{
			return (PacketType)reader.PeekUInt16();
		}

		public static PacketType ReadPacketType(this PacketReader reader)
		{
			return (PacketType)reader.ReadUInt16();
		}

		public static void Put(this PacketWriter writer, PacketType packetType)
		{
			writer.Put((ushort)packetType);
		}
	}
}
