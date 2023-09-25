using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.DataType.Primitives;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct UserId : IPacketSerializable, IEquatable<UserId>, IComparable<UserId>
	{
		public ulong Id;
		public int SerializeSize => sizeof(ulong);

		public UserId(ulong value) => Id = value;
		public static implicit operator ulong(UserId value) => value.Id;
		public static implicit operator UserId(ulong value) => new UserId(value);
		public void Serialize(IPacketWriter writer) => writer.Put(Id);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadUInt64(out Id);
		public static bool operator ==(UserId left, UserId right) => left.Id == right.Id;
		public static bool operator !=(UserId left, UserId right) => left.Id != right.Id;
		public override int GetHashCode() => Id.GetHashCode();
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is not UserId value)
				return false;
			return value == this;
		}
		public bool Equals(UserId other) => this == other;
		public int CompareTo(UserId other) => Id.CompareTo(other.Id);
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(ulong));
		public override string ToString() => Id.ToString();
	}
}
