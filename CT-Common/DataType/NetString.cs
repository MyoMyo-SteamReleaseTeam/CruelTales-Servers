﻿#if NET7_0_OR_GREATER
using System.Diagnostics;
#endif

using System;
using System.Runtime.CompilerServices;
using System.Text;
using CT.Common.Serialization;

namespace CT.Common.DataType
{
	/// <summary>65536이하 길이의 string 입니다.</summary>
	[Serializable]
	public struct NetString : IPacketSerializable
	{
#if NET7_0_OR_GREATER
		public string Value = "";
#else
		public string Value;
#endif
		public const int MAX_BYTE_LENGTH = 65536;

		public int ByteSize
		{
			get
			{
				if (string.IsNullOrEmpty(Value))
					return 0;

				return Encoding.UTF8.GetByteCount(Value);
			}
		}
		public int SerializeSize => ByteSize + 2;
		public static implicit operator string(NetString value) => value.Value;
		public static implicit operator NetString(string value) => new NetString(value ?? string.Empty);

#if NET7_0_OR_GREATER
		public NetString()
		{
			Value = string.Empty;
		}
#endif

		public NetString(string value)
		{
#if NET7_0_OR_GREATER
			Debug.Assert(ByteSize <= MAX_BYTE_LENGTH);
#endif
			Value = value;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put(this);
		}

		public void Deserialize(PacketReader reader)
		{
			Value = reader.ReadNetString();
		}

		public override string ToString() => Value;

		public override bool Equals(object? obj)
		{
			string value = "";

			if (obj == null)
				return false;
			else if (obj is string)
				value = (string)obj;
			else if (obj is NetString)
				value = (NetString)obj;
			else if (obj is NetStringShort)
				value = (NetStringShort)obj;
			else
				return false;

			return Value == value;
		}

		public override int GetHashCode() => Value.GetHashCode();

		public static bool operator ==(NetString lhs, string rhs) => lhs.Value == rhs;
		public static bool operator !=(NetString lhs, string rhs) => lhs.Value != rhs;
		public static bool operator ==(string lhs, NetString rhs) => lhs == rhs.Value;
		public static bool operator !=(string lhs, NetString rhs) => lhs != rhs.Value;
		public static bool operator ==(NetString lhs, NetString rhs) => lhs.Value == rhs.Value;
		public static bool operator !=(NetString lhs, NetString rhs) => lhs.Value != rhs.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Serialize(in NetString value, PacketWriter writer)
		{
			writer.Put(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Deserialize(ref NetString value, PacketReader reader)
		{
			value = reader.ReadNetString();
		}

		public static void IgnoreStatic(PacketReader reader)
		{
			ushort count = reader.ReadUInt16();
			reader.Ignore(count);
		}

		public void Ignore(PacketReader reader) => IgnoreStatic(reader);
	}

	/// <summary>256이하 길이의 string 입니다.</summary>
	[Serializable]
	public struct NetStringShort : IPacketSerializable
	{
#if NET7_0_OR_GREATER
		public string Value = "";
#else
		public string Value;
#endif
		public const int MAX_BYTE_LENGTH = 256;

		public int ByteSize
		{
			get
			{
				if (string.IsNullOrEmpty(Value))
					return 0;

				return Encoding.UTF8.GetByteCount(Value);
			}
		}
		public int SerializeSize => ByteSize + 1;

		public static implicit operator string(NetStringShort value) => value.Value;
		public static implicit operator NetStringShort(string value) => new NetStringShort(value ?? string.Empty);

#if NET7_0_OR_GREATER
		public NetStringShort()
		{
			Value = string.Empty;
		}
#endif

		public NetStringShort(string value)
		{
#if NET7_0_OR_GREATER
			Debug.Assert(ByteSize <= MAX_BYTE_LENGTH);
#endif
			Value = value;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Put(this);
		}

		public void Deserialize(PacketReader reader)
		{
			Value = reader.ReadNetStringShort();
		}

		public override string ToString() => Value;

		public override bool Equals(object? obj)
		{
			string value = "";

			if (obj == null)
				return false;
			else if (obj is string)
				value = (string)obj;
			else if (obj is NetString)
				value = (NetString)obj;
			else if (obj is NetStringShort)
				value = (NetStringShort)obj;
			else
				return false;

			return Value == value;
		}

		public override int GetHashCode() => Value.GetHashCode();

		public static bool operator ==(NetStringShort lhs, string rhs) => lhs.Value == rhs;
		public static bool operator !=(NetStringShort lhs, string rhs) => lhs.Value != rhs;
		public static bool operator ==(string lhs, NetStringShort rhs) => lhs == rhs.Value;
		public static bool operator !=(string lhs, NetStringShort rhs) => lhs != rhs.Value;
		public static bool operator ==(NetStringShort lhs, NetStringShort rhs) => lhs.Value == rhs.Value;
		public static bool operator !=(NetStringShort lhs, NetStringShort rhs) => lhs.Value != rhs.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Serialize(in NetStringShort value, PacketWriter writer)
		{
			writer.Put(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Deserialize(ref NetStringShort value, PacketReader reader)
		{
			value = reader.ReadNetStringShort();
		}

		public void Ignore(PacketReader reader) => IgnoreStatic(reader);

		public static void IgnoreStatic(PacketReader reader)
		{
			byte count = reader.ReadByte();
			reader.Ignore(count);
		}
	}
}
