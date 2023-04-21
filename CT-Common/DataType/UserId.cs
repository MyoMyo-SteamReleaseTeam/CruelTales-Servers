using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct UserId : IPacketSerializable
	{
		public ulong Id;

		public UserId(ulong value)
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
			return Id.ToString();
		}
	}
}
