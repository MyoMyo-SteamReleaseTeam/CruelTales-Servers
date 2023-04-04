using CT.Network.Serialization;

namespace CT.Network.DataType
{
	public struct ClientId : IPacketSerializable
	{
		public ulong Id;

		public ClientId(ulong value)
		{
			Id = value;
		}

		public int SerializeSize => sizeof(ulong);

		public void Serialize(PacketWriter writer)
		{
			writer.Put(Id);
		}

		public void Deserialize(PacketReader reader)
		{
			Id = reader.ReadUInt64();
		}

		public override string ToString()
		{
			return $"Id:{Id}";
		}
	}
}
