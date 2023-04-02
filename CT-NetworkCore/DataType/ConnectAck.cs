using CT.Network.Serialization;

namespace CT.Network.DataType
{
	public enum ConnectAck : byte
	{
		RejectUnknown = 0,

		Success,

		WrongEndpoint,
		WrongVersion,
		WrongPassword,

		Unauthorized,
		ThereIsNoSpace,
	}

	public static class ConnectAckExtension
	{
		public static void Put(this PacketWriter writer, ConnectAck value)
		{
			writer.Put((byte)value);
		}

		public static ConnectAck ReadConnectAck(this PacketReader reader)
		{
			return (ConnectAck)reader.ReadByte();
		}
	}
}
