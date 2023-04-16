using CT.Common.Serialization;

namespace CT.Common.Gameplay
{
	public enum MiniGameMapTheme : ushort
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

	public enum MiniGameMapType : ushort
	{
		None = 0,

		Map_Square_Europe = 50,

		Map_RedHood_0 = 110,
		Map_RedHood_1 = 111,
		Map_RedHood_2 = 112,
		Map_RedHood_3 = 113,

		Map_Dueoksini = 200,
	}

	public static class MiniGameMapTypesExtension
	{
		public static void Put(this PacketWriter writer, MiniGameMapTheme value)
		{
			writer.Put((ushort)value);
		}

		public static MiniGameMapTheme ReadMiniGameMapTheme(this PacketReader reader)
		{
			return (MiniGameMapTheme)reader.ReadUInt16();
		}

		public static void Put(this PacketWriter writer, MiniGameMapType value)
		{
			writer.Put((ushort)value);
		}

		public static MiniGameMapType ReadMiniGameMapType(this PacketReader reader)
		{
			return (MiniGameMapType)reader.ReadUInt16();
		}
	}
}
