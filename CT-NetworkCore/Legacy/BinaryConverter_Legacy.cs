using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace CT.Network.Legacy
{
	[StructLayout(LayoutKind.Explicit)]
	public readonly ref struct MemoryLayoutUInt32
	{
		[FieldOffset(0)]
		public readonly uint LayoutValue;

		[FieldOffset(0)]
		public readonly float Value;

		public MemoryLayoutUInt32(float value)
		{
			Value = value;
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly ref struct MemoryLayoutUInt64
	{
		[FieldOffset(0)]
		public readonly ulong LayoutValue;

		[FieldOffset(0)]
		public readonly double Value;

		public MemoryLayoutUInt64(double value)
		{
			Value = value;
		}
	}

	/// <summary>원시 타입을 byte 배열에 인코딩 하거나 디코딩합니다.</summary>
	public static class BinaryConverter_Legacy
	{
		public static bool IsLittleEndian() => BitConverter.IsLittleEndian;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteBool(in ArraySegment<byte> dest, int offset, bool data)
		{
			dest[offset] = (byte)(data ? 1 : 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteSByte(in ArraySegment<byte> dest, int offset, sbyte data)
		{
			dest[offset] = (byte)data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteByte(in ArraySegment<byte> dest, int offset, byte data)
		{
			dest[offset] = data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt16(in ArraySegment<byte> dest, int offset, short data)
		{
#if BIGENDIAN
			dest[offset + 1] = (byte)(data);
			dest[offset    ] = (byte)(data >> 8);
#else
			dest[offset] = (byte)data;
			dest[offset + 1] = (byte)(data >> 8);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt16(in ArraySegment<byte> dest, int offset, ushort data)
		{
#if BIGENDIAN
			dest[offset + 1] = (byte)(data);
			dest[offset    ] = (byte)(data >> 8);
#else
			dest[offset] = (byte)data;
			dest[offset + 1] = (byte)(data >> 8);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt32(in ArraySegment<byte> dest, int offset, int data)
		{
#if BIGENDIAN
			dest[offset + 3] = (byte)(data >> 0);
			dest[offset + 2] = (byte)(data >> 8);
			dest[offset + 1] = (byte)(data >> 16);
			dest[offset] = (byte)(data >> 24);
#else
			dest[offset] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt32(in ArraySegment<byte> dest, int offset, uint data)
		{
#if BIGENDIAN
			dest[offset + 3] = (byte)(data >> 0);
			dest[offset + 2] = (byte)(data >> 8);
			dest[offset + 1] = (byte)(data >> 16);
			dest[offset] = (byte)(data >> 24);
#else
			dest[offset] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt64(in ArraySegment<byte> dest, int offset, long data)
		{
#if BIGENDIAN
			dest[offset + 7] = (byte)(data);
			dest[offset + 6] = (byte)(data >> 8);
			dest[offset + 5] = (byte)(data >> 16);
			dest[offset + 4] = (byte)(data >> 24);
			dest[offset + 3] = (byte)(data >> 32);
			dest[offset + 2] = (byte)(data >> 40);
			dest[offset + 1] = (byte)(data >> 48);
			dest[offset    ] = (byte)(data >> 56);
#else
			dest[offset] = (byte)data;
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			dest[offset + 4] = (byte)(data >> 32);
			dest[offset + 5] = (byte)(data >> 40);
			dest[offset + 6] = (byte)(data >> 48);
			dest[offset + 7] = (byte)(data >> 56);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt64(in ArraySegment<byte> dest, int offset, ulong data)
		{
#if BIGENDIAN
			dest[offset + 7] = (byte)(data);
			dest[offset + 6] = (byte)(data >> 8);
			dest[offset + 5] = (byte)(data >> 16);
			dest[offset + 4] = (byte)(data >> 24);
			dest[offset + 3] = (byte)(data >> 32);
			dest[offset + 2] = (byte)(data >> 40);
			dest[offset + 1] = (byte)(data >> 48);
			dest[offset    ] = (byte)(data >> 56);
#else
			dest[offset] = (byte)data;
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			dest[offset + 4] = (byte)(data >> 32);
			dest[offset + 5] = (byte)(data >> 40);
			dest[offset + 6] = (byte)(data >> 48);
			dest[offset + 7] = (byte)(data >> 56);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteFloat(in ArraySegment<byte> dest, int offset, float data)
		{
			MemoryLayoutUInt32 layout = new MemoryLayoutUInt32(data);
			WriteUInt32(dest, offset, layout.LayoutValue);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteDouble(in ArraySegment<byte> dest, int offset, double data)
		{
			MemoryLayoutUInt64 layout = new MemoryLayoutUInt64(data);
			WriteUInt64(dest, offset, layout.LayoutValue);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteString(in ArraySegment<byte> dest, int offset, string data)
		{
			Debug.Assert(dest.Array != null);
			int byteSize = Encoding.UTF8.GetBytes(data, 0, data.Length, dest.Array, dest.Offset + offset + 2);
			WriteUInt16(dest, offset, (ushort)byteSize);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteBytes(in ArraySegment<byte> dest, int offset, byte[] data)
		{
			Debug.Assert(dest.Array != null);
			int dataLength = data.Length;
			WriteUInt16(dest, offset, (ushort)dataLength);
			Buffer.BlockCopy(data, 0, dest.Array, dest.Offset + offset + 2, dataLength);
		}

		// Decoding
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadBool(in ArraySegment<byte> src, int offset, out bool data)
		{
			data = src[offset] == 0 ? false : true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadSByte(in ArraySegment<byte> src, int offset, out sbyte data)
		{
			data = (sbyte)src[offset];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadByte(in ArraySegment<byte> src, int offset, out byte data)
		{
			data = src[offset];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadInt16(in ArraySegment<byte> src, int offset, out short data)
		{
			data = 0;
			data |= (short)(src[offset + 0] << 0);
			data |= (short)(src[offset + 1] << 8);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadUInt16(in ArraySegment<byte> src, int offset, out ushort data)
		{
			data = 0;
			data |= (ushort)(src[offset + 0] << 0);
			data |= (ushort)(src[offset + 1] << 8);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadInt32(in ArraySegment<byte> src, int offset, out int data)
		{
			data = 0;
			data |= src[offset + 0] << 0;
			data |= src[offset + 1] << 8;
			data |= src[offset + 2] << 16;
			data |= src[offset + 3] << 24;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadUInt32(in ArraySegment<byte> src, int offset, out uint data)
		{
			data = 0;
			data |= (uint)src[offset + 0] << 0;
			data |= (uint)src[offset + 1] << 8;
			data |= (uint)src[offset + 2] << 16;
			data |= (uint)src[offset + 3] << 24;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadInt64(in ArraySegment<byte> src, int offset, out long data)
		{
			data = 0;
			data |= (long)src[offset + 0] << 0;
			data |= (long)src[offset + 1] << 8;
			data |= (long)src[offset + 2] << 16;
			data |= (long)src[offset + 3] << 24;
			data |= (long)src[offset + 4] << 32;
			data |= (long)src[offset + 5] << 40;
			data |= (long)src[offset + 6] << 48;
			data |= (long)src[offset + 7] << 56;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadUInt64(in ArraySegment<byte> src, int offset, out ulong data)
		{
			data = 0;
			data |= (ulong)src[offset + 0] << 0;
			data |= (ulong)src[offset + 1] << 8;
			data |= (ulong)src[offset + 2] << 16;
			data |= (ulong)src[offset + 3] << 24;
			data |= (ulong)src[offset + 4] << 32;
			data |= (ulong)src[offset + 5] << 40;
			data |= (ulong)src[offset + 6] << 48;
			data |= (ulong)src[offset + 7] << 56;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadFloat(in ArraySegment<byte> src, int offset, out float data)
		{
			Debug.Assert(src.Array != null);

			data = BitConverter.ToSingle(src.Array, src.Offset + offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReadDouble(in ArraySegment<byte> src, int offset, out double data)
		{
			Debug.Assert(src.Array != null);

			data = BitConverter.ToDouble(src.Array, src.Offset + offset);
		}

		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static int ReadString(in ArraySegment<byte> src, int offset, out string data)
		//{
		//	Debug.Assert(src.Array != null);
		//	DecodeUInt16(src, offset, out var dataLength);
		//	data = Encoding.UTF8.GetString(src.Array, src.Offset + offset + STRING_DATA_LENGTH_COUNT_BYTE, dataLength);
		//	return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		//}

		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static int ReadBytes(in ArraySegment<byte> src, int offset, out byte[] data)
		//{
		//	Debug.Assert(src.Array != null);

		//	DecodeUInt16(src, offset, out var dataLength);
		//	data = new byte[dataLength];
		//	Buffer.BlockCopy(src.Array, src.Offset + offset + STRING_DATA_LENGTH_COUNT_BYTE, data, 0, dataLength);
		//	return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		//}
	}
}