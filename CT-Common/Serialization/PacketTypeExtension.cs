using CT.Packets;

namespace CT.Common.Serialization
{
	public static class PacketTypeExtension
	{
		public const string SIZE_TYPE = "byte";
		public const int SIZE = 1;

		public static PacketType PeekPacketType(this IPacketReader reader)
		{
			return (PacketType)reader.PeekByte();
		}

		public static PacketType ReadPacketType(this IPacketReader reader)
		{
			return (PacketType)reader.ReadByte();
		}

		public static void Put(this IPacketWriter writer, PacketType packetType)
		{
			writer.Put((byte)packetType);
		}
	}
}
