using System;
using CT.Common.Serialization;

namespace CT.Common.DataType.Primitives
{
	public struct NetByte : IPacketSerializable, IEquatable<NetByte>
	{
		public byte Value;
		public const int SERIALIZE_SIZE = sizeof(byte);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator byte(NetByte value) => value.Value;
		public static implicit operator NetByte(byte value) => new NetByte(value);
#if NET
		public NetByte() => Value = 0;
#endif
		public NetByte(byte value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadByte(out Value);
		public int CompareTo(byte other) => Value.CompareTo(other);
		public bool Equals(NetByte other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetSByte : IPacketSerializable, IEquatable<NetSByte>
	{
		public sbyte Value;
		public const int SERIALIZE_SIZE = sizeof(sbyte);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator sbyte(NetSByte value) => value.Value;
		public static implicit operator NetSByte(sbyte value) => new NetSByte(value);
#if NET
		public NetSByte() => Value = 0;
#endif
		public NetSByte(sbyte value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadSByte(out Value);
		public int CompareTo(sbyte other) => Value.CompareTo(other);
		public bool Equals(NetSByte other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetInt16 : IPacketSerializable, IEquatable<NetInt16>
	{
		public short Value;
		public const int SERIALIZE_SIZE = sizeof(short);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator short(NetInt16 value) => value.Value;
		public static implicit operator NetInt16(short value) => new NetInt16(value);
#if NET
		public NetInt16() => Value = 0;
#endif
		public NetInt16(short value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadInt16(out Value);
		public int CompareTo(short other) => Value.CompareTo(other);
		public bool Equals(NetInt16 other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetUInt16 : IPacketSerializable, IEquatable<NetUInt16>
	{
		public ushort Value;
		public const int SERIALIZE_SIZE = sizeof(ushort);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator ushort(NetUInt16 value) => value.Value;
		public static implicit operator NetUInt16(ushort value) => new NetUInt16(value);
#if NET
		public NetUInt16() => Value = 0;
#endif
		public NetUInt16(ushort value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadUInt16(out Value);
		public int CompareTo(ushort other) => Value.CompareTo(other);
		public bool Equals(NetUInt16 other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetInt32 : IPacketSerializable, IEquatable<NetInt32>
	{
		public int Value;
		public const int SERIALIZE_SIZE = sizeof(int);
		public int SerializeSize => SERIALIZE_SIZE;
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
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetUInt32 : IPacketSerializable, IEquatable<NetUInt32>
	{
		public uint Value;
		public const int SERIALIZE_SIZE = sizeof(uint);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator uint(NetUInt32 value) => value.Value;
		public static implicit operator NetUInt32(uint value) => new NetUInt32(value);
#if NET
		public NetUInt32() => Value = 0;
#endif
		public NetUInt32(uint value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadUInt32(out Value);
		public int CompareTo(uint other) => Value.CompareTo(other);
		public bool Equals(NetUInt32 other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetInt64 : IPacketSerializable, IEquatable<NetInt64>
	{
		public long Value;
		public const int SERIALIZE_SIZE = sizeof(long);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator long(NetInt64 value) => value.Value;
		public static implicit operator NetInt64(long value) => new NetInt64(value);
#if NET
		public NetInt64() => Value = 0;
#endif
		public NetInt64(long value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadInt64(out Value);
		public int CompareTo(long other) => Value.CompareTo(other);
		public bool Equals(NetInt64 other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetUInt64 : IPacketSerializable, IEquatable<NetUInt64>
	{
		public ulong Value;
		public const int SERIALIZE_SIZE = sizeof(ulong);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator ulong(NetUInt64 value) => value.Value;
		public static implicit operator NetUInt64(ulong value) => new NetUInt64(value);
#if NET
		public NetUInt64() => Value = 0;
#endif
		public NetUInt64(ulong value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadUInt64(out Value);
		public int CompareTo(ulong other) => Value.CompareTo(other);
		public bool Equals(NetUInt64 other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetSingle : IPacketSerializable, IEquatable<NetSingle>
	{
		public float Value;
		public const int SERIALIZE_SIZE = sizeof(float);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator float(NetSingle value) => value.Value;
		public static implicit operator NetSingle(float value) => new NetSingle(value);
#if NET
		public NetSingle() => Value = 0;
#endif
		public NetSingle(float value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadSingle(out Value);
		public int CompareTo(float other) => Value.CompareTo(other);
		public bool Equals(NetSingle other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}

	public struct NetDouble : IPacketSerializable, IEquatable<NetDouble>
	{
		public double Value;
		public const int SERIALIZE_SIZE = sizeof(double);
		public int SerializeSize => SERIALIZE_SIZE;
		public static implicit operator double(NetDouble value) => value.Value;
		public static implicit operator NetDouble(double value) => new NetDouble(value);
#if NET
		public NetDouble() => Value = 0;
#endif
		public NetDouble(double value) => Value = value;
		public void Serialize(IPacketWriter writer) => writer.Put(Value);
		public bool TryDeserialize(IPacketReader reader) => reader.TryReadDouble(out Value);
		public int CompareTo(double other) => Value.CompareTo(other);
		public bool Equals(NetDouble other) => Value == other.Value;
		public void Ignore(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(SERIALIZE_SIZE);
	}
}
