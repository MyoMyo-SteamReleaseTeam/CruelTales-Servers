using CT.Network.Serialization;

namespace CT.Network.DataType
{

	public struct RoomGuid : IPacketSerializable
	{
		public ulong Guid;

		public int SerializeSize => sizeof(ulong);

		public static implicit operator ulong(RoomGuid value) => value.Guid;
		public static explicit operator RoomGuid(ulong value) => new RoomGuid(value);

		public RoomGuid(ulong value)
		{
			Guid = value;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put(Guid);
		}

		public void Deserialize(PacketReader reader)
		{
			Guid = reader.ReadUInt64();
		}

		public override string ToString() => Guid.ToString();
	}
}
