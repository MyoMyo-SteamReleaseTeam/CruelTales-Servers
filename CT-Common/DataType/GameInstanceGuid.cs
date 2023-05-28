using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct GameInstanceGuid : IPacketSerializable, IEquatable<GameInstanceGuid>
	{
		public ulong Guid;
		public int SerializeSize => sizeof(ulong);

		public static implicit operator ulong(GameInstanceGuid value) => value.Guid;
		public static explicit operator GameInstanceGuid(ulong value) => new GameInstanceGuid(value);
		public GameInstanceGuid(ulong value) => Guid = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Guid);
		public void Deserialize(IPacketReader reader) => Guid = reader.ReadUInt64();
		public static bool operator==(GameInstanceGuid lhs, GameInstanceGuid rhs) => lhs.Guid == rhs.Guid;
		public static bool operator!=(GameInstanceGuid lhs, GameInstanceGuid rhs) => lhs.Guid != rhs.Guid;
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(ulong));
		public bool Equals(GameInstanceGuid other) => this == other;
		public override int GetHashCode() =>Guid.GetHashCode();
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is not GameInstanceGuid other)
				return false;
			return this == other;
		}
		public override string ToString() => Guid.ToString();
	}
}
