using System;
using CT.Common.Serialization;

namespace CT.Common.DataType.Primitives
{
	public struct NetInt32 : IPacketSerializable, IEquatable<NetInt32>
	{
		public int Value;
		public int SerializeSize => sizeof(int);
		public static implicit operator int(NetInt32 value) => value.Value;
		public static implicit operator NetInt32(int value) => new NetInt32(value);
#if NET
		public NetInt32() => Value = 0;
#endif
		public NetInt32(int value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadInt32(out Value);
		public int CompareTo(int other) => Value.CompareTo(other);
		public bool Equals(NetInt32 other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(int));
	}
}
