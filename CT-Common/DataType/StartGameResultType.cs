using CT.Common.Serialization;

namespace CT.Common.DataType
{
	public enum StartGameResultType : byte
	{
		None = 0,
		Success,
		NoEnoughPlayer,
		TooManyPlayer,
		SomePlayerNotReady,
		YouAreNotHost,
		FatalError,
	}

	public static class StartGameResultTypeExtension
	{
		public static void Put(this IPacketWriter writer, StartGameResultType value)
		{
			writer.Put((byte)value);
		}

		public static bool TryReadStartGameResultType(this IPacketReader reader, out StartGameResultType value)
		{
			if (!reader.TryReadByte(out var enumValue))
			{
				value = default;
				return false;
			}

			value = (StartGameResultType)enumValue;
			return true;
		}
	}
}
