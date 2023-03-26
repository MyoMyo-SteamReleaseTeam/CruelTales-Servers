using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace CT.Network.Serialization
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
	public static class BinaryConverter
	{
		static BinaryConverter()
		{
			if (BitConverter.IsLittleEndian == false)
			{
				throw new NotSupportedException($"Currently, big endian is not supported.");
			}
		}

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
		public static unsafe void WriteInt16(in ArraySegment<byte> dest, int offset, short data)
		{
			fixed (byte* ptr = dest.Array)
			{
				*(short*)(ptr + offset) = data;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void WriteUInt16(in ArraySegment<byte> dest, int offset, ushort data)
		{
			fixed (byte* ptr = dest.Array)
			{
				*(ushort*)(ptr + offset) = data;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void WriteInt32(in ArraySegment<byte> dest, int offset, int data)
		{
			fixed (byte* ptr = dest.Array)
			{
				*(int*)(ptr + offset) = data;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void WriteUInt32(in ArraySegment<byte> dest, int offset, uint data)
		{
			fixed (byte* ptr = dest.Array)
			{
				*(uint*)(ptr + offset) = data;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void WriteInt64(in ArraySegment<byte> dest, int offset, long data)
		{
			fixed (byte* ptr = dest.Array)
			{
				*(long*)(ptr + offset) = data;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void WriteUInt64(in ArraySegment<byte> dest, int offset, ulong data)
		{
			fixed (byte* ptr = dest.Array)
			{
				*(ulong*)(ptr + offset) = data;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void WriteFloat(in ArraySegment<byte> dest, int offset, float data)
		{
			fixed (byte* ptr = dest.Array)
			{
				*(float*)(ptr + offset) = data;
			}
			//MemoryLayoutUInt32 layout = new MemoryLayoutUInt32(data);
			//BinaryConverter.WriteUInt32(dest, offset, layout.LayoutValue);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void WriteDouble(in ArraySegment<byte> dest, int offset, double data)
		{
			fixed (byte* ptr = dest.Array)
			{
				*(double*)(ptr + offset) = data;
			}
			//MemoryLayoutUInt64 layout = new MemoryLayoutUInt64(data);
			//BinaryConverter.WriteUInt64(dest, offset, layout.LayoutValue);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int WriteString(in ArraySegment<byte> dest, int offset, string data)
		{
			Debug.Assert(dest.Array != null);
			int byteSize = Encoding.UTF8.GetBytes(data, 0, data.Length, dest.Array, dest.Offset + offset + 2);
			WriteUInt16(dest, offset, (ushort)byteSize);
			return byteSize + 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int WriteBytes(in ArraySegment<byte> dest, int offset, byte[] data)
		{
			Debug.Assert(dest.Array != null);
			int dataLength = data.Length;
			WriteUInt16(dest, offset, (ushort)dataLength);
			Buffer.BlockCopy(data, 0, dest.Array, dest.Offset + offset + 2, dataLength);
			return dataLength + 2;
		}

		// Decoding
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ReadBool(in ArraySegment<byte> src, int offset)
		{
			return src[offset] == 0 ? false : true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte ReadSByte(in ArraySegment<byte> src, int offset)
		{
			return (sbyte)src[offset];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte ReadByte(in ArraySegment<byte> src, int offset)
		{
			return src[offset];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe short ReadInt16(in ArraySegment<byte> src, int offset)
		{
			fixed (byte* ptr = src.Array)
			{
				return *(short*)(ptr + offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe ushort ReadUInt16(in ArraySegment<byte> src, int offset)
		{
			fixed (byte* ptr = src.Array)
			{
				return *(ushort*)(ptr + offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe int ReadInt32(in ArraySegment<byte> src, int offset)
		{
			fixed (byte* ptr = src.Array)
			{
				return *(int*)(ptr + offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe uint ReadUInt32(in ArraySegment<byte> src, int offset)
		{
			fixed (byte* ptr = src.Array)
			{
				return *(uint*)(ptr + offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe long ReadInt64(in ArraySegment<byte> src, int offset)
		{
			fixed (byte* ptr = src.Array)
			{
				return *(long*)(ptr + offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe ulong ReadUInt64(in ArraySegment<byte> src, int offset)
		{
			fixed (byte* ptr = src.Array)
			{
				return *(ulong*)(ptr + offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe float ReadFloat(in ArraySegment<byte> src, int offset)
		{
			fixed (byte* ptr = src.Array)
			{
				return *(float*)(ptr + offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe double ReadDouble(in ArraySegment<byte> src, int offset)
		{
			fixed (byte* ptr = src.Array)
			{
				return *(double*)(ptr + offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ReadString(in ArraySegment<byte> src, int offset, out int read)
		{
			Debug.Assert(src.Array != null);
			var byteLength = ReadUInt16(src, offset);
			read = byteLength + 2;
			return Encoding.UTF8.GetString(src.Array, src.Offset + offset + 2, byteLength);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] ReadBytesCopy(in ArraySegment<byte> src, int offset, out int read)
		{
			Debug.Assert(src.Array != null);
			var byteLength = ReadUInt16(src, offset);
			read = byteLength + 2;
			var buffer = new byte[byteLength];
			Buffer.BlockCopy(src.Array, src.Offset + offset + 2, buffer, 0, byteLength);
			return buffer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadBytes(in ArraySegment<byte> src, int srcOffset, 
										   in ArraySegment<byte> dest, int destOffset)
		{
			Debug.Assert(src.Array != null);
			Debug.Assert(dest.Array != null);

			var byteLength = ReadUInt16(src, srcOffset);
			Buffer.BlockCopy(src.Array,  src.Offset + srcOffset + 2,
							 dest.Array, dest.Offset + destOffset, byteLength);

			return byteLength + 2;
		}
	}
}