using CT.Network.Serialization;

namespace CT.Network.DataType
{
	public enum AckJoinMatch : byte
	{
		RejectUnknown = 0,

		Success,

		WrongEndpoint,
		WrongVersion,
		WrongPassword,

		Unauthorized,
		ThereIsNoSpace,
	}

	public static class AckJoinMatchExtension
	{
		public static void Put(this PacketWriter writer, AckJoinMatch value)
		{
			writer.Put((byte)value);
		}

		public static AckJoinMatch ReadAckJoinMatch(this PacketReader reader)
		{
			return (AckJoinMatch)reader.ReadByte();
		}
	}
}
