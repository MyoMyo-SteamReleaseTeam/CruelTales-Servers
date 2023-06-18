using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public enum GameMapTheme : ushort
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

		Square_Loading = 20,

		Square_Europe = 50,

		MiniGame_RedHood_0 = 110,
		MiniGame_RedHood_1 = 111,
		MiniGame_RedHood_2 = 112,
		MiniGame_RedHood_3 = 113,

		MiniGame_Dueoksini_0 = 200,
	}

	public static class GameMapTypesExtension
	{
		public static void Put(this IPacketWriter writer, GameMapTheme value)
		{
			writer.Put((ushort)value);
		}

		public static GameMapTheme ReadGameMapTheme(this IPacketReader reader)
		{
			return (GameMapTheme)reader.ReadUInt16();
		}

		public static void Put(this IPacketWriter writer, GameMapType value)
		{
			writer.Put((ushort)value);
		}

		public static GameMapType ReadGameMapType(this IPacketReader reader)
		{
			return (GameMapType)reader.ReadUInt16();
		}
	}
}
