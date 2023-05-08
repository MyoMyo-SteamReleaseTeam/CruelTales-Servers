using System;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct GameInstanceGuid : IPacketSerializable
	{
		public ulong Guid;

		public int SerializeSize => sizeof(ulong);

		public static implicit operator ulong(GameInstanceGuid value) => value.Guid;
		public static explicit operator GameInstanceGuid(ulong value) => new GameInstanceGuid(value);

		public GameInstanceGuid(ulong value)
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
		public void Ignore(PacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(PacketReader reader) => reader.Ignore(sizeof(ulong));
	}
}
