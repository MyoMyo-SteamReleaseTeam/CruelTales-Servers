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
		public static void Put(this PacketWriter writer, NetEntityType value)
		{
			writer.Put((byte)value);
		}

		public static NetEntityType ReadNetEntityType(this PacketReader reader)
		{
			return (NetEntityType)reader.ReadByte();
		}
	}
}
