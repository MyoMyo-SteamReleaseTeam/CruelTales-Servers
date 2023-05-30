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

		public static bool TryReadAckJoinMatch(this IPacketReader reader, out AckJoinMatch value)
		{
			if (!reader.TryReadByte(out var enumValue))
			{
				value = default;
				return false;
			}

			value = (AckJoinMatch)enumValue;
			return true;
		}
	}
}
