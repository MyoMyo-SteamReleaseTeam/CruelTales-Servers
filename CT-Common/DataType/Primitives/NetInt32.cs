using System;
using CT.Common.Serialization;

namespace CT.Common.DataType.Primitives
{
	public struct NetInt32 : IPacketSerializable, IEquatable<NetInt32>
	{
		private int _value;
		public int SerializeSize => sizeof(int);
		public static implicit operator int(NetInt32 value) => value._value;
		public static implicit operator NetInt32(int value) => new NetInt32(value);
#if NET
		public NetInt32() => _value = 0;
#endif
		public NetInt32(int value) => _value = value;
		public void Serialize(PacketWriter writer) => writer.Put(_value);
		public void Deserialize(PacketReader reader) => _value = reader.ReadInt32();
		public int CompareTo(int other) => _value.CompareTo(other);
		public bool Equals(NetInt32 other) => _value == other._value;
	}
}
