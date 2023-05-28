using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct NetworkIdentity : IPacketSerializable, IEquatable<NetworkIdentity>
	{
		public byte Id;

		public int SerializeSize => sizeof(byte);
		public static int MaxValue => byte.MaxValue;

		public NetworkIdentity(int value) => Id = (byte)value;
		public NetworkIdentity(byte value) => Id = value;
		public NetworkIdentity(NetworkIdentity value) => this.Id = value.Id;

		public NetworkIdentity(IPacketReader reader)
		{
			Id = reader.ReadByte();
		}

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(Id);
		}

		public void Deserialize(IPacketReader reader)
		{
			Id = reader.ReadByte();
		}

		public static implicit operator byte(NetworkIdentity value) => value.Id;
		public static explicit operator NetworkIdentity(byte value) => new NetworkIdentity(value);

		public static NetworkIdentity operator++(NetworkIdentity value) => new NetworkIdentity(value.Id + 1);
		public static NetworkIdentity operator--(NetworkIdentity value) => new NetworkIdentity(value.Id - 1);
		public static bool operator ==(NetworkIdentity left, NetworkIdentity right) => left.Id == right.Id;
		public static bool operator !=(NetworkIdentity left, NetworkIdentity right) => left.Id != right.Id;
		public static bool operator ==(NetworkIdentity left, int right) => left.Id == right;
		public static bool operator !=(NetworkIdentity left, int right) => left.Id != right;
		public override int GetHashCode() => Id.GetHashCode();
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is not NetworkIdentity value)
				return false;
			return value == this;
		}
		public bool Equals(NetworkIdentity other) => this == other;
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(byte));
		public override string ToString() => Id.ToString();
	}
}
