using CT.Common.Serialization;

namespace CT.Common.DataType
{
	public enum NetEntityType : byte
	{
		None,
		Player,
	}

	public static class NetEntityTypeExtension
	{
		public static void Put(this IPacketWriter writer, NetEntityType value)
		{
			writer.Put((byte)value);
		}

		public static NetEntityType ReadNetEntityType(this IPacketReader reader)
		{
			return (NetEntityType)reader.ReadByte();
		}
	}
}
