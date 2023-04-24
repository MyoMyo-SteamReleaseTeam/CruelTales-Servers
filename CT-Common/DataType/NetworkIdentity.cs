using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct NetworkIdentity : IPacketSerializable
	{
		public ushort Id;

		public int SerializeSize => sizeof(ushort);

		public NetworkIdentity(int value)
		{
			Id = (ushort)value;
		}

		public NetworkIdentity(ushort value)
		{
			Id = value;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put(Id);
		}

		public void Deserialize(PacketReader reader)
		{
			Id = reader.ReadUInt16();
		}

		public static void Ignore(PacketReader reader)
		{
			reader.Ignore(sizeof(ushort));
		}

		public static bool operator ==(NetworkIdentity left, NetworkIdentity right) => left.Id == right.Id;
		public static bool operator !=(NetworkIdentity left, NetworkIdentity right) => left.Id != right.Id;
		public override int GetHashCode() => Id.GetHashCode();
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is not NetworkIdentity value)
				return false;
			return value == this;
		}
	}
}
