using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using CT.Network.Exceptions;

namespace CT.Network.Legacy.Rerference
{
	/// <summary>원시 타입을 byte 배열에 인코딩 하거나 디코딩합니다.</summary>
	public static class DataConverter
	{
		/// <summary>문자열의 길이를 표현할 수 있는 최대 바이트 수 입니다.</summary>
		public static readonly int STRING_DATA_LENGTH_COUNT_BYTE = 2;
		public static readonly int MAX_STRING_DATA_LENGTH = 800;

		public static bool IsLittleEndian() => BitConverter.IsLittleEndian;

		//
		// 
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeBool(in ArraySegment<byte> dest, int offset, bool data)
		{
			dest[offset] = (byte)(data ? 1 : 0);
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeInt8(in ArraySegment<byte> dest, int offset, sbyte data)
		{
			dest[offset] = (byte)data;
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeUInt8(in ArraySegment<byte> dest, int offset, byte data)
		{
			dest[offset] = data;
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeInt16(in ArraySegment<byte> dest, int offset, short data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			return 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeUInt16(in ArraySegment<byte> dest, int offset, ushort data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			return 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeInt32(in ArraySegment<byte> dest, int offset, int data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeUInt32(in ArraySegment<byte> dest, int offset, uint data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeInt64(in ArraySegment<byte> dest, int offset, long data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			dest[offset + 4] = (byte)(data >> 32);
			dest[offset + 5] = (byte)(data >> 40);
			dest[offset + 6] = (byte)(data >> 48);
			dest[offset + 7] = (byte)(data >> 56);
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeUInt64(in ArraySegment<byte> dest, int offset, ulong data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			dest[offset + 4] = (byte)(data >> 32);
			dest[offset + 5] = (byte)(data >> 40);
			dest[offset + 6] = (byte)(data >> 48);
			dest[offset + 7] = (byte)(data >> 56);
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeFloat(in ArraySegment<byte> dest, int offset, float data)
		{
			Debug.Assert(dest.Array != null);

			var rawData = BitConverter.GetBytes(data);
			Buffer.BlockCopy(rawData, 0, dest.Array, dest.Offset + offset, 4);
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeDouble(in ArraySegment<byte> dest, int offset, double data)
		{
			Debug.Assert(dest.Array != null);

			var rawData = BitConverter.GetBytes(data);
			Buffer.BlockCopy(rawData, 0, dest.Array, dest.Offset + offset, 8);
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeString(in ArraySegment<byte> dest, int offset, string data)
		{
			Debug.Assert(dest.Array != null);
			int dataLength = Encoding.UTF8.GetBytes(data, 0, data.Length, dest.Array, dest.Offset + offset + STRING_DATA_LENGTH_COUNT_BYTE);
			if (dataLength >= MAX_STRING_DATA_LENGTH)
			{
				throw new TooLongSteamDataException(dataLength, MAX_STRING_DATA_LENGTH);
			}
			EncodeUInt16(dest, offset, (ushort)dataLength);
			return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeBytes(in ArraySegment<byte> dest, int offset, byte[] data)
		{
			Debug.Assert(dest.Array != null);

			int dataLength = data.Length;
			Buffer.BlockCopy(data, 0, dest.Array, dest.Offset + offset + STRING_DATA_LENGTH_COUNT_BYTE, dataLength);
			if (dataLength >= MAX_STRING_DATA_LENGTH)
			{
				throw new TooLongSteamDataException(dataLength, MAX_STRING_DATA_LENGTH);
			}
			EncodeUInt16(dest, offset, (ushort)dataLength);
			return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		}

		// Decoding

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeBool(in ArraySegment<byte> src, int offset, out bool data)
		{
			data = src[offset] == 0 ? false : true;
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeInt8(in ArraySegment<byte> src, int offset, out sbyte data)
		{
			data = (sbyte)src[offset];
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeUInt8(in ArraySegment<byte> src, int offset, out byte data)
		{
			data = src[offset];
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeInt16(in ArraySegment<byte> src, int offset, out short data)
		{
			data = 0;
			data |= (short)(src[offset + 0] << 0);
			data |= (short)(src[offset + 1] << 8);
			return 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeUInt16(in ArraySegment<byte> src, int offset, out ushort data)
		{
			data = 0;
			data |= (ushort)(src[offset + 0] << 0);
			data |= (ushort)(src[offset + 1] << 8);
			return 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeInt32(in ArraySegment<byte> src, int offset, out int data)
		{
			data = 0;
			data |= src[offset + 0] << 0;
			data |= src[offset + 1] << 8;
			data |= src[offset + 2] << 16;
			data |= src[offset + 3] << 24;
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeUInt32(in ArraySegment<byte> src, int offset, out uint data)
		{
			data = 0;
			data |= (uint)src[offset + 0] << 0;
			data |= (uint)src[offset + 1] << 8;
			data |= (uint)src[offset + 2] << 16;
			data |= (uint)src[offset + 3] << 24;
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeInt64(in ArraySegment<byte> src, int offset, out long data)
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
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeUInt64(in ArraySegment<byte> src, int offset, out ulong data)
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
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeFloat(in ArraySegment<byte> src, int offset, out float data)
		{
			Debug.Assert(src.Array != null);

			data = BitConverter.ToSingle(src.Array, src.Offset + offset);
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeDouble(in ArraySegment<byte> src, int offset, out double data)
		{
			Debug.Assert(src.Array != null);

			data = BitConverter.ToDouble(src.Array, src.Offset + offset);
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeString(in ArraySegment<byte> src, int offset, out string data)
		{
			Debug.Assert(src.Array != null);

			DecodeUInt16(src, offset, out var dataLength);
			data = Encoding.UTF8.GetString(src.Array, src.Offset + offset + STRING_DATA_LENGTH_COUNT_BYTE, dataLength);
			return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeBytes(in ArraySegment<byte> src, int offset, out byte[] data)
		{
			Debug.Assert(src.Array != null);

			DecodeUInt16(src, offset, out var dataLength);
			data = new byte[dataLength];
			Buffer.BlockCopy(src.Array, src.Offset + offset + STRING_DATA_LENGTH_COUNT_BYTE, data, 0, dataLength);
			return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		}

		// Encoding
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeBool(byte[] dest, int offset, bool data)
		{
			dest[offset] = (byte)(data ? 1 : 0);
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeInt8(byte[] dest, int offset, sbyte data)
		{
			dest[offset] = (byte)data;
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeUInt8(byte[] dest, int offset, byte data)
		{
			dest[offset] = data;
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeInt16(byte[] dest, int offset, short data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			return 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeUInt16(byte[] dest, int offset, ushort data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			return 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeInt32(byte[] dest, int offset, int data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeUInt32(byte[] dest, int offset, uint data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeInt64(byte[] dest, int offset, long data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			dest[offset + 4] = (byte)(data >> 32);
			dest[offset + 5] = (byte)(data >> 40);
			dest[offset + 6] = (byte)(data >> 48);
			dest[offset + 7] = (byte)(data >> 56);
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeUInt64(byte[] dest, int offset, ulong data)
		{
			dest[offset + 0] = (byte)(data >> 0);
			dest[offset + 1] = (byte)(data >> 8);
			dest[offset + 2] = (byte)(data >> 16);
			dest[offset + 3] = (byte)(data >> 24);
			dest[offset + 4] = (byte)(data >> 32);
			dest[offset + 5] = (byte)(data >> 40);
			dest[offset + 6] = (byte)(data >> 48);
			dest[offset + 7] = (byte)(data >> 56);
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeFloat(byte[] dest, int offset, float data)
		{
			BitConverter.GetBytes(data).CopyTo(dest, offset);
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeDouble(byte[] dest, int offset, double data)
		{
			BitConverter.GetBytes(data).CopyTo(dest, offset);
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeString(byte[] dest, int offset, string data)
		{
			int dataLength = Encoding.UTF8.GetBytes(data, 0, data.Length, dest, offset + STRING_DATA_LENGTH_COUNT_BYTE);
			if (dataLength >= MAX_STRING_DATA_LENGTH)
			{
				throw new TooLongSteamDataException(dataLength, MAX_STRING_DATA_LENGTH);
			}
			EncodeUInt16(dest, offset, (ushort)dataLength);
			return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int EncodeBytes(byte[] dest, int offset, byte[] data)
		{
			int dataLength = data.Length;
			Buffer.BlockCopy(data, 0, dest, offset + STRING_DATA_LENGTH_COUNT_BYTE, dataLength);
			if (dataLength >= MAX_STRING_DATA_LENGTH)
			{
				throw new TooLongSteamDataException(dataLength, MAX_STRING_DATA_LENGTH);
			}
			EncodeUInt16(dest, offset, (ushort)dataLength);
			return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		}

		// Decoding
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeBool(byte[] src, int offset, out bool data)
		{
			data = src[offset] == 0 ? false : true;
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeInt8(byte[] src, int offset, out sbyte data)
		{
			data = (sbyte)src[offset];
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeUInt8(byte[] src, int offset, out byte data)
		{
			data = src[offset];
			return 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeInt16(byte[] src, int offset, out short data)
		{
			data = 0;
			data |= (short)(src[offset + 0] << 0);
			data |= (short)(src[offset + 1] << 8);
			return 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeUInt16(byte[] src, int offset, out ushort data)
		{
			data = 0;
			data |= (ushort)(src[offset + 0] << 0);
			data |= (ushort)(src[offset + 1] << 8);
			return 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeInt32(byte[] src, int offset, out int data)
		{
			data = 0;
			data |= src[offset + 0] << 0;
			data |= src[offset + 1] << 8;
			data |= src[offset + 2] << 16;
			data |= src[offset + 3] << 24;
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeUInt32(byte[] src, int offset, out uint data)
		{
			data = 0;
			data |= (uint)src[offset + 0] << 0;
			data |= (uint)src[offset + 1] << 8;
			data |= (uint)src[offset + 2] << 16;
			data |= (uint)src[offset + 3] << 24;
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeInt64(byte[] src, int offset, out long data)
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
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeUInt64(byte[] src, int offset, out ulong data)
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
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeFloat(byte[] src, int offset, out float data)
		{
			data = BitConverter.ToSingle(src, offset);
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeDouble(byte[] src, int offset, out double data)
		{
			data = BitConverter.ToDouble(src, offset);
			return 8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeString(byte[] src, int offset, out string data)
		{
			DecodeUInt16(src, offset, out var dataLength);
			data = Encoding.UTF8.GetString(src, offset + STRING_DATA_LENGTH_COUNT_BYTE, dataLength);
			return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeBytes(byte[] src, int offset, out byte[] data)
		{
			DecodeUInt16(src, offset, out var dataLength);
			data = new byte[dataLength];
			Buffer.BlockCopy(src, offset + STRING_DATA_LENGTH_COUNT_BYTE, data, 0, dataLength);
			return dataLength + STRING_DATA_LENGTH_COUNT_BYTE;
		}
	}
}