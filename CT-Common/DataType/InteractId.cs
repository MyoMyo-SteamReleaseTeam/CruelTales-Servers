using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct InteractId : IPacketSerializable
	{
		public byte Id;

		public InteractId(byte value)
		{
			Id = value;
		}

		public int SerializeSize => sizeof(byte);

		public void Serialize(PacketWriter writer)
		{
			writer.Put(Id);
		}

		public void Deserialize(PacketReader reader)
		{
			Id = reader.ReadByte();
		}

		public override string ToString()
		{
			return Id.ToString();
		}
	}
}
