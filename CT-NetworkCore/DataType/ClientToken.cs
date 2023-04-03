using CT.Network.Serialization;

namespace CT.Network.DataType
{
	public struct ClientToken : IPacketSerializable
	{
		public ulong Token;

		public ClientToken(ulong token)
		{
			Token = token;
		}

		public int SerializeSize => sizeof(ulong);

		public void Serialize(PacketWriter writer)
		{
			writer.Put(Token);
		}

		public void Deserialize(PacketReader reader)
		{
			Token = reader.ReadUInt64();
		}

		public override string ToString()
		{
			return $"Token:{Token}";
		}
	}
}
