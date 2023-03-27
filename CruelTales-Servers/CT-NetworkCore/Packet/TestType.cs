using CT.Network.Serialization;

namespace CT.Network.Packet
{
	public enum TestType : byte
	{

	}

	public static class TestTypeExtension
	{
		public static void Put(this PacketWriter writer, TestType value)
		{
			writer.Put((byte)value);
		}

		public static TestType ReadTestType(this PacketReader reader)
		{
			return (TestType)reader.ReadByte();
		}
	}
}
