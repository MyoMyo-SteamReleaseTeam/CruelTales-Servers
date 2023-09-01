using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public enum GameModeType : ushort
	{
		None = 0,
		Lobby,
		RedHood,
		Dueoksini,
	}

	public enum GameMapThemeType : ushort
	{
		None = 0,

		Europe = 100,
		Europe_France = 101,

		EastAsia = 200,
		EastAsia_Korea = 201,
		EastAsia_China = 202,
		EastAsia_Japan = 203,

		MiddleEast = 300,
		MiddleEast_Egypt = 301,
	}

	public enum GameMapType : ushort
	{
		None = 0,

		Square_Europe = 50,

		RedHood_0 = 110,
		RedHood_1 = 111,
		RedHood_2 = 112,
		RedHood_3 = 113,

		Dueoksini_0 = 200,
	}

	public static class GameMapTypesExtension
	{
		public static void Put(this IPacketWriter writer, GameModeType value)
		{
			writer.Put((ushort)value);
		}

		public static bool TryReadGameModeType(this IPacketReader reader, out GameModeType value)
		{
			if (!reader.TryReadUInt16(out ushort read))
			{
				value = GameModeType.None;
				return false;
			}

			value = (GameModeType)read;
			return true;
		}

		public static void Put(this IPacketWriter writer, GameMapThemeType value)
		{
			writer.Put((ushort)value);
		}

		public static bool TryReadGameMapTheme(this IPacketReader reader, out GameMapThemeType value)
		{
			if (!reader.TryReadUInt16(out ushort read))
			{
				value = GameMapThemeType.None;
				return false;
			}

			value = (GameMapThemeType)read;
			return true;
		}

		public static void Put(this IPacketWriter writer, GameMapType value)
		{
			writer.Put((ushort)value);
		}

		public static bool TryReadGameMapType(this IPacketReader reader, out GameMapType value)
		{
			if (!reader.TryReadUInt16(out ushort read))
			{
				value = GameMapType.None;
				return false;
			}

			value = (GameMapType)read;
			return true;
		}
	}
}
