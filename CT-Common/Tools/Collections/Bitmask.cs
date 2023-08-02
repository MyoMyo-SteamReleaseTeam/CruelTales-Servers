using System;
using System.Runtime.CompilerServices;
using CT.Common.Serialization;

namespace CT.Common.Tools.Collections
{
	public struct BitmaskByte : IPacketSerializable
	{
		public byte Mask;

		/// <summary>전체 비트의 크기입니다.</summary>
		public const int BIT_SIZE = 8;
		public const byte ALL_BIT_SET = 0b_1111_1111;
		public const byte ALL_BIT_UNSET = 0b_0000_0000;

		public int SerializeSize => sizeof(byte);

		public static implicit operator BitmaskByte(byte value) => new BitmaskByte(value);
		public static implicit operator byte(BitmaskByte value) => value.Mask;

		/// <summary>Value로 마스크를 생성합니다.</summary>
		/// <param name="value">1바이트 비트마스크입니다.</param>
		public BitmaskByte(byte value) => Mask = value;

		/// <summary>Byte수 만큼 비트 마스크를 생성합니다.</summary>
		/// <param name="memoryStride1D">차지할 메모리 공간의 byte수 입니다. Size = byte * 8</param>
		/// <param name="value">초기화 할 값</param>
		public BitmaskByte(bool value = false) => Mask = value ? ALL_BIT_SET : ALL_BIT_UNSET;

		/// <summary>해당 비트를 참조합니다.</summary>
		/// <param name="x">bitmask 인덱스</param>
		/// <returns>값</returns>
		public bool this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (Mask & 1 << index) != 0;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (value)
				{
					Mask |= (byte)(1 << index);
				}
				else
				{
					Mask &= (byte)~(1 << index);
				}
			}
		}

		/// <summary>모든 비트를 value로 초기화합니다.</summary>
		/// <param name="value">초기화할 값</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear(bool value = false) => Mask = value ? ALL_BIT_SET : ALL_BIT_UNSET;

		/// <summary>모든 비트를 뒤집습니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Flip() => Mask = (byte)~Mask;

		/// <summary>해당 index의 비트를 false로 설정합니다.</summary>
		/// <param name="x">인덱스 x</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFalse(int index) => Mask &= (byte)~(1 << index);

		/// <summary>해당 index의 비트를 true로 설정합니다.</summary>
		/// <param name="x">인덱스</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrue(int index) => Mask |= (byte)(1 << index);

		/// <summary>모든 비트가 true라면 true를 반환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsAllTrue() => Mask == ALL_BIT_SET;

		/// <summary>모든 비트가 false라면 true를 반환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsAllFalse() => Mask == ALL_BIT_UNSET;

		/// <summary>하나의 비트라도 ture라면 true를 반환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool AnyTrue() => Mask != ALL_BIT_UNSET;

		/// <summary>하나의 비트라도 false라면 false를 반환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool AnyFalse() => Mask != ALL_BIT_SET;

		/// <summary>인덱스가 유효한 범위인지 검사합니다.</summary>
		/// <param name="index">인덱스 x</param>
		/// <returns>유효한 인덱스라면 true를 반환합니다.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsValidIndex(int index) => !(index >= BIT_SIZE || index < 0);

		public override string ToString() => $"{Convert.ToString(Mask, 2).PadLeft(BIT_SIZE, '0')}";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Serialize(IPacketWriter writer) => writer.Put(Mask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryDeserialize(IPacketReader reader)
		{
			if (reader.TryReadByte(out var value))
			{
				Mask = value;
				return true;
			}

			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(byte));
	}

	public struct Bitmask32 : IPacketSerializable
	{
		public uint Mask;

		/// <summary>전체 비트의 크기입니다.</summary>
		public const int BIT_SIZE = 32;
		public const uint ALL_BIT_SET = 0x_FFFFFFFF;
		public const uint ALL_BIT_UNSET = 0x_00000000;

		public int SerializeSize => sizeof(uint);

		public static implicit operator Bitmask32(uint value) => new Bitmask32(value);
		public static implicit operator uint(Bitmask32 value) => value.Mask;

		/// <summary>Value로 마스크를 생성합니다.</summary>
		/// <param name="value">1바이트 비트마스크입니다.</param>
		public Bitmask32(uint value) => Mask = value;

		/// <summary>Byte수 만큼 비트 마스크를 생성합니다.</summary>
		/// <param name="memoryStride1D">차지할 메모리 공간의 byte수 입니다. Size = byte * 8</param>
		/// <param name="value">초기화 할 값</param>
		public Bitmask32(bool value = false)
		{
			Mask = value ? ALL_BIT_SET : ALL_BIT_UNSET;
		}

		/// <summary>해당 비트를 참조합니다.</summary>
		/// <param name="x">bitmask 인덱스</param>
		/// <returns>값</returns>
		public bool this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (Mask & 1 << index) != 0;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (value)
				{
					Mask |= (uint)(1 << index);
				}
				else
				{
					Mask &= (uint)~(1 << index);
				}
			}
		}

		/// <summary>모든 비트를 value로 초기화합니다.</summary>
		/// <param name="value">초기화할 값</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear(bool value = false) => Mask = value ? ALL_BIT_SET : ALL_BIT_UNSET;

		/// <summary>모든 비트를 뒤집습니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Flip() => Mask = ~Mask;

		/// <summary>해당 index의 비트를 false로 설정합니다.</summary>
		/// <param name="x">인덱스 x</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFalse(int index) => Mask &= (uint)~(1 << index);

		/// <summary>해당 index의 비트를 true로 설정합니다.</summary>
		/// <param name="x">인덱스</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrue(int index) => Mask |= (uint)(1 << index);

		/// <summary>모든 비트가 true라면 true를 반환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsAllTrue() => Mask == ALL_BIT_SET;

		/// <summary>모든 비트가 false라면 true를 반환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsAllFalse() => Mask == ALL_BIT_UNSET;

		/// <summary>인덱스가 유효한 범위인지 검사합니다.</summary>
		/// <param name="index">인덱스 x</param>
		/// <returns>유효한 인덱스라면 true를 반환합니다.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsValidIndex(int index) => !(index >= BIT_SIZE || index < 0);

		public override string ToString() => $"{Convert.ToString(Mask, 2).PadLeft(BIT_SIZE, '0')}";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Serialize(IPacketWriter writer) => writer.Put(Mask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryDeserialize(IPacketReader reader)
		{
			if (reader.TryReadUInt32(out var value))
			{
				Mask = value;
				return true;
			}

			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IgnoreStatic(IPacketReader reader) => reader.Ignore(sizeof(uint));
	}

	public static class BitmaskExtension
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Put(this IPacketWriter writer, BitmaskByte value)
		{
			writer.Put(value.Mask);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutTo(this IPacketWriter writer, BitmaskByte value, int position)
		{
			writer.PutTo(value.Mask, position);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BitmaskByte ReadBitmaskByte(this IPacketReader reader)
		{
			return new BitmaskByte(reader.ReadByte());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Put(this IPacketWriter writer, Bitmask32 value)
		{
			writer.Put(value.Mask);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Bitmask32 ReadBitmask32(this IPacketReader reader)
		{
			return new Bitmask32(reader.ReadUInt32());
		}
	}
}
