using CT.Common.Serialization;

namespace CT.Common.DataType
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
		public static void Put(this IPacketWriter writer, AckJoinMatch value)
		{
			writer.Put((byte)value);
		}

		public static AckJoinMatch ReadAckJoinMatch(this IPacketReader reader)
		{
			return (AckJoinMatch)reader.ReadByte();
		}
	}
}
