using CT.Network.Serialization;

namespace CT.Network.DataType
{
	public struct NetEntityID : IPacketSerializable
	{
		public byte ID;

		public int SerializeSize => sizeof(byte);

		public static implicit operator byte(NetEntityID value) => value.ID;
		public static explicit operator NetEntityID(byte value) => new NetEntityID(value);

		public NetEntityID(byte value)
		{
			ID = value;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put(ID);
		}

		public void Deserialize(PacketReader reader)
		{
			ID = reader.ReadByte();
		}
	}
}
