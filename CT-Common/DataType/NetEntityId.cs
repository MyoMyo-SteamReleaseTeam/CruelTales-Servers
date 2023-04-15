using CT.Common.Serialization;

namespace CT.Common.DataType
{
	public struct NetEntityId : IPacketSerializable
	{
		public byte ID;

		public int SerializeSize => sizeof(byte);

		public static implicit operator byte(NetEntityId value) => value.ID;
		public static explicit operator NetEntityId(byte value) => new NetEntityId(value);

		public NetEntityId(byte value)
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
