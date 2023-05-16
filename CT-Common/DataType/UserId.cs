using System;
using System.Diagnostics.CodeAnalysis;
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

		public void Serialize(IPacketWriter writer)
		{
			writer.Put(Id);
		}

		public void Deserialize(IPacketReader reader)
		{
			Id = reader.ReadUInt64();
		}

		public override string ToString()
		{
			return Id.ToString();
		}

		public static bool operator ==(UserId left, UserId right) => left.Id == right.Id;
		public static bool operator !=(UserId left, UserId right) => left.Id != right.Id;
		public override int GetHashCode() => Id.GetHashCode();
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is not UserId value)
				return false;
			return value == this;
		}
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(ulong));
	}
}
