using System;
using System.Diagnostics.CodeAnalysis;
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
