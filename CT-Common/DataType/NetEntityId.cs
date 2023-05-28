using System;
using System.Diagnostics.CodeAnalysis;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	[Serializable]
	public struct NetEntityId : IPacketSerializable, IEquatable<NetEntityId>
	{
		public byte ID;
		public int SerializeSize => sizeof(byte);

		public static implicit operator byte(NetEntityId value) => value.ID;
		public static explicit operator NetEntityId(byte value) => new NetEntityId(value);
		public NetEntityId(int value) => ID = (byte)value;
		public NetEntityId(byte value) => ID = value;
		public void Serialize(IPacketWriter writer) => writer.Put(ID);
		public void Deserialize(IPacketReader reader) => ID = reader.ReadByte();
		public static bool operator ==(NetEntityId lhs, NetEntityId rhs) => lhs.ID == rhs.ID;
		public static bool operator !=(NetEntityId lhs, NetEntityId rhs) => lhs.ID != rhs.ID;
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(byte));
		public bool Equals(NetEntityId other) => this == other;
		public override int GetHashCode() => ID.GetHashCode();
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is not NetEntityId other)
				return false;
			return this == other;
		}
		public override string ToString() => ID.ToString();
	}
}
