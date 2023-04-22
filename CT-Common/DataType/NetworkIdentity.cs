using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct NetworkIdentity : IPacketSerializable
	{
		public byte Id;

		public int SerializeSize => sizeof(byte);

		public NetworkIdentity(int value)
		{
			Id = (byte)value;
		}

		public NetworkIdentity(byte value)
		{
			Id = value;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put(Id);
		}

		public void Deserialize(PacketReader reader)
		{
			Id = reader.ReadByte();
		}
	}
}
