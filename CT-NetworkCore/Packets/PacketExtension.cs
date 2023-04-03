using CT.Network.Serialization;

namespace CT.Packets
{
	public static class PacketExtension
	{
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
