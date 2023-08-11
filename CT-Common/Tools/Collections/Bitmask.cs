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

	public struct Bitmask256 : IPacketSerializable
	{
		public Bitmask32 Mask_0;
		public Bitmask32 Mask_1;
		public Bitmask32 Mask_2;
		public Bitmask32 Mask_3;
		public Bitmask32 Mask_4;
		public Bitmask32 Mask_5;
		public Bitmask32 Mask_6;
		public Bitmask32 Mask_7;

		public const int SyncSize = sizeof(uint) * 8;
		public int SerializeSize => SyncSize;

		/// <summary>해당 비트를 참조합니다.</summary>
		/// <param name="x">bitmask 인덱스</param>
		/// <returns>값</returns>
		public bool this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				int bucketIndex = index / 32;
				int maskIndex = index - bucketIndex * 32;
				switch (bucketIndex)
				{
					case 0: return Mask_0[maskIndex];
					case 1: return Mask_1[maskIndex];
					case 2: return Mask_2[maskIndex];
					case 3: return Mask_3[maskIndex];
					case 4: return Mask_4[maskIndex];
					case 5: return Mask_5[maskIndex];
					case 6: return Mask_6[maskIndex];
					case 7: return Mask_7[maskIndex];
				}

				throw new ArgumentOutOfRangeException();
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				int bucketIndex = index / 32;
				int maskIndex = index - bucketIndex * 32;
				switch (bucketIndex)
				{
					case 0: Mask_0[maskIndex] = value; break;
					case 1: Mask_1[maskIndex] = value; break;
					case 2: Mask_2[maskIndex] = value; break;
					case 3: Mask_3[maskIndex] = value; break;
					case 4: Mask_4[maskIndex] = value; break;
					case 5: Mask_5[maskIndex] = value; break;
					case 6: Mask_6[maskIndex] = value; break;
					case 7: Mask_7[maskIndex] = value; break;
				}

				throw new ArgumentOutOfRangeException();
			}
		}

		public Bitmask32 GetMask(int bucketIndex)
		{
			switch (bucketIndex)
			{
				case 0: return Mask_0;
				case 1: return Mask_1;
				case 2: return Mask_2;
				case 3: return Mask_3;
				case 4: return Mask_4;
				case 5: return Mask_5;
				case 6: return Mask_6;
				case 7: return Mask_7;
			}

			throw new ArgumentOutOfRangeException();
		}

		/// <summary>모든 비트를 value로 초기화합니다.</summary>
		/// <param name="value">초기화할 값</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear(bool value = false)
		{
			Mask_0.Clear(value);
			Mask_1.Clear(value);
			Mask_2.Clear(value);
			Mask_3.Clear(value);
			Mask_4.Clear(value);
			Mask_5.Clear(value);
			Mask_6.Clear(value);
			Mask_7.Clear(value);
		}

		/// <summary>모든 비트를 뒤집습니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Flip()
		{
			Mask_0.Flip();
			Mask_1.Flip();
			Mask_2.Flip();
			Mask_3.Flip();
			Mask_4.Flip();
			Mask_5.Flip();
			Mask_6.Flip();
			Mask_7.Flip();
		}

		/// <summary>해당 index의 비트를 false로 설정합니다.</summary>
		/// <param name="x">인덱스 x</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFalse(int index) => this[index] = false;

		/// <summary>해당 index의 비트를 true로 설정합니다.</summary>
		/// <param name="x">인덱스</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrue(int index) => this[index] = true;

		/// <summary>모든 비트가 true라면 true를 반환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsAllTrue()
		{
			bool isAllTrue = true;
			isAllTrue &= Mask_0.IsAllTrue();
			isAllTrue &= Mask_1.IsAllTrue();
			isAllTrue &= Mask_2.IsAllTrue();
			isAllTrue &= Mask_3.IsAllTrue();
			isAllTrue &= Mask_4.IsAllTrue();
			isAllTrue &= Mask_5.IsAllTrue();
			isAllTrue &= Mask_6.IsAllTrue();
			isAllTrue &= Mask_7.IsAllTrue();
			return isAllTrue;
		}

		/// <summary>모든 비트가 false라면 true를 반환합니다.</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsAllFalse()
		{
			bool isAllFalse = true;
			isAllFalse &= Mask_0.IsAllFalse();
			isAllFalse &= Mask_1.IsAllFalse();
			isAllFalse &= Mask_2.IsAllFalse();
			isAllFalse &= Mask_3.IsAllFalse();
			isAllFalse &= Mask_4.IsAllFalse();
			isAllFalse &= Mask_5.IsAllFalse();
			isAllFalse &= Mask_6.IsAllFalse();
			isAllFalse &= Mask_7.IsAllFalse();
			return isAllFalse;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Ignore(IPacketReader reader) => IgnoreStatic(reader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IgnoreStatic(IPacketReader reader)
		{
			reader.Ignore(SyncSize);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Serialize(IPacketWriter writer)
		{
			Mask_0.Serialize(writer);
			Mask_1.Serialize(writer);
			Mask_2.Serialize(writer);
			Mask_3.Serialize(writer);
			Mask_4.Serialize(writer);
			Mask_5.Serialize(writer);
			Mask_6.Serialize(writer);
			Mask_7.Serialize(writer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryDeserialize(IPacketReader reader)
		{
			if (!reader.CanRead(SyncSize)) return false;
			Mask_0 = reader.ReadUInt32();
			Mask_1 = reader.ReadUInt32();
			Mask_2 = reader.ReadUInt32();
			Mask_3 = reader.ReadUInt32();
			Mask_4 = reader.ReadUInt32();
			Mask_5 = reader.ReadUInt32();
			Mask_6 = reader.ReadUInt32();
			Mask_7 = reader.ReadUInt32();
			return true;
		}
	}

	public static class BitmaskExtension
	{
		// BitmaskByte

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
		public static bool TryReadBitmaskByte(this IPacketReader reader, out BitmaskByte bitmaskByte)
		{
			if (!reader.TryReadByte(out byte value))
			{
				bitmaskByte = 0;
				return false;
			}

			bitmaskByte = value;
			return true;
		}

		// Bitmask32

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
