using System;

namespace CT.Tool.Collections
{
	public struct BitmaskByte
	{
		public byte Mask;

		/// <summary>전체 비트의 크기입니다.</summary>
		public const int BIT_SIZE = 8;
		public const byte ALL_BIT_SET = 0b_1111_1111;
		public const byte ALL_BIT_UNSET = 0b_0000_0000;

		public static implicit operator BitmaskByte(byte value) => new BitmaskByte(value);
		public static implicit operator byte(BitmaskByte value) => value.Mask;

		/// <summary>Value로 마스크를 생성합니다.</summary>
		/// <param name="value">1바이트 비트마스크입니다.</param>
		public BitmaskByte(byte value) => Mask = value;

		/// <summary>Byte수 만큼 비트 마스크를 생성합니다.</summary>
		/// <param name="memoryStride1D">차지할 메모리 공간의 byte수 입니다. Size = byte * 8</param>
		/// <param name="value">초기화 할 값</param>
		public BitmaskByte(bool value = false) => Clear(value);

		/// <summary>해당 비트를 참조합니다.</summary>
		/// <param name="x">bitmask 인덱스</param>
		/// <returns>값</returns>
		public bool this[int index]
		{
			get
			{
				return (Mask & 1 << index) != 0;
			}
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
		public void Clear(bool value = false) => Mask = (value ? ALL_BIT_SET : ALL_BIT_UNSET);

		/// <summary>모든 비트를 뒤집습니다.</summary>
		public void Flip() => Mask = (byte)~Mask;

		/// <summary>해당 index의 비트를 false로 설정합니다.</summary>
		/// <param name="x">인덱스 x</param>
		public void SetFalse(int index) => Mask &= (byte)~(1 << index);

		/// <summary>해당 index의 비트를 true로 설정합니다.</summary>
		/// <param name="x">인덱스</param>
		public void SetTrue(int index) => Mask |= (byte)(1 << index);

		/// <summary>모든 비트가 true라면 true를 반환합니다.</summary>
		public bool IsAllTrue() => Mask == ALL_BIT_SET;

		/// <summary>모든 비트가 false라면 true를 반환합니다.</summary>
		public bool IsAllFalse() => Mask == ALL_BIT_UNSET;

		/// <summary>인덱스가 유효한 범위인지 검사합니다.</summary>
		/// <param name="index">인덱스 x</param>
		/// <returns>유효한 인덱스라면 true를 반환합니다.</returns>
		public bool IsValidIndex(int index) => !(index >= BIT_SIZE || index < 0);

		public override string ToString()
		{
			return $"{Convert.ToString(Mask, 2).PadLeft(BIT_SIZE, '0')}";
		}
	}

	public struct Bitmask32
	{
		public uint Mask;

		/// <summary>전체 비트의 크기입니다.</summary>
		public const int BIT_SIZE = 8;
		public const uint ALL_BIT_SET = 0x_FFFFFFFF;
		public const uint ALL_BIT_UNSET = 0x_00000000;

		public static implicit operator Bitmask32(uint value) => new Bitmask32(value);
		public static implicit operator uint(Bitmask32 value) => value.Mask;

		/// <summary>Value로 마스크를 생성합니다.</summary>
		/// <param name="value">1바이트 비트마스크입니다.</param>
		public Bitmask32(uint value) => Mask = value;

		/// <summary>Byte수 만큼 비트 마스크를 생성합니다.</summary>
		/// <param name="memoryStride1D">차지할 메모리 공간의 byte수 입니다. Size = byte * 8</param>
		/// <param name="value">초기화 할 값</param>
		public Bitmask32(bool value = false) => Clear(value);

		/// <summary>해당 비트를 참조합니다.</summary>
		/// <param name="x">bitmask 인덱스</param>
		/// <returns>값</returns>
		public bool this[int index]
		{
			get
			{
				return (Mask & 1 << index) != 0;
			}
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
		public void Clear(bool value = false) => Mask = (value ? ALL_BIT_SET : ALL_BIT_UNSET);

		/// <summary>모든 비트를 뒤집습니다.</summary>
		public void Flip() => Mask = (uint)~Mask;

		/// <summary>해당 index의 비트를 false로 설정합니다.</summary>
		/// <param name="x">인덱스 x</param>
		public void SetFalse(int index) => Mask &= (uint)~(1 << index);

		/// <summary>해당 index의 비트를 true로 설정합니다.</summary>
		/// <param name="x">인덱스</param>
		public void SetTrue(int index) => Mask |= (uint)(1 << index);

		/// <summary>모든 비트가 true라면 true를 반환합니다.</summary>
		public bool IsAllTrue() => Mask == ALL_BIT_SET;

		/// <summary>모든 비트가 false라면 true를 반환합니다.</summary>
		public bool IsAllFalse() => Mask == ALL_BIT_UNSET;

		/// <summary>인덱스가 유효한 범위인지 검사합니다.</summary>
		/// <param name="index">인덱스 x</param>
		/// <returns>유효한 인덱스라면 true를 반환합니다.</returns>
		public bool IsValidIndex(int index) => !(index >= BIT_SIZE || index < 0);

		public override string ToString()
		{
			return $"{Convert.ToString(Mask, 2).PadLeft(BIT_SIZE, '0')}";
		}
	}
}
