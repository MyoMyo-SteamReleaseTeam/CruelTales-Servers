using System.Text;

namespace CT.Network.Serialization
{
	/// <summary>NetBuffer를 읽습니다.</summary>
	public class NetBufferReader
	{
		private NetBuffer mNetBuffer;
		public int Size => mSize;
		private int mSize = 0;

		public int ReadIndex { get; private set; } = 0;

		public NetBufferReader(in NetBuffer srcNetBuffer)
		{
			mNetBuffer = srcNetBuffer;
			mSize = mNetBuffer.Size;
		}

		public void ResetReadIndex()
		{
			ReadIndex = 0;
		}

		#region Read Operation

		public sbyte ReadInt8()
		{
			ReadIndex += mNetBuffer.ReadInt8(ReadIndex, out var data);
			return data;
		}

		public byte ReadUInt8()
		{
			ReadIndex += mNetBuffer.ReadUInt8(ReadIndex, out var data);
			return data;
		}

		public short ReadInt16()
		{
			ReadIndex += mNetBuffer.ReadInt16(ReadIndex, out var data);
			return data;
		}

		public ushort ReadUInt16()
		{
			ReadIndex += mNetBuffer.ReadUInt16(ReadIndex, out var data);
			return data;
		}

		public int ReadInt32()
		{
			ReadIndex += mNetBuffer.ReadInt32(ReadIndex, out var data);
			return data;
		}

		public uint ReadUInt32()
		{
			ReadIndex += mNetBuffer.ReadUInt32(ReadIndex, out var data);
			return data;
		}

		public long ReadInt64()
		{
			ReadIndex += mNetBuffer.ReadInt64(ReadIndex, out var data);
			return data;
		}

		public ulong ReadUInt64()
		{
			ReadIndex += mNetBuffer.ReadUInt64(ReadIndex, out var data);
			return data;
		}

		public float ReadFloat()
		{
			ReadIndex += mNetBuffer.ReadFloat(ReadIndex, out var data);
			return data;
		}

		public double ReadDouble()
		{
			ReadIndex += mNetBuffer.ReadDouble(ReadIndex, out var data);
			return data;
		}

		public byte[] ReadBytes()
		{
			int curReadIndex = ReadIndex;
			curReadIndex += mNetBuffer.ReadInt16(ReadIndex, out short dataLength);
			return mNetBuffer.CopyBytes(curReadIndex, dataLength);
		}

		public string ReadString()
		{
			var rawData = ReadBytes();
			return Encoding.UTF8.GetString(rawData);
		}

		#endregion

		#region Try Read Operation

		public bool TryReadInt8(out sbyte data)
		{
			if (Size < ReadIndex + 1)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadInt8(ReadIndex, out data);
			return true;
		}

		public bool TryReadUInt8(out byte data)
		{
			if (Size < ReadIndex + 1)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadUInt8(ReadIndex, out data);
			return true;
		}

		public bool TryReadInt16(out short data)
		{
			if (Size < ReadIndex + 2)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadInt16(ReadIndex, out data);
			return true;
		}

		public bool TryReadUInt16(out ushort data)
		{
			if (Size < ReadIndex + 2)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadUInt16(ReadIndex, out data);
			return true;
		}

		public bool TryReadInt32(out int data)
		{
			if (Size < ReadIndex + 4)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadInt32(ReadIndex, out data);
			return true;
		}

		public bool TryReadUInt32(out uint data)
		{
			if (Size < ReadIndex + 4)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadUInt32(ReadIndex, out data);
			return true;
		}

		public bool TryReadInt64(out long data)
		{
			if (Size < ReadIndex + 8)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadInt64(ReadIndex, out data);
			return true;
		}

		public bool TryReadUInt64(out ulong data)
		{
			if (Size < ReadIndex + 8)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadUInt64(ReadIndex, out data);
			return true;
		}

		public bool TryReadFloat(out float data)
		{
			if (Size < ReadIndex + 4)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadFloat(ReadIndex, out data);
			return true;
		}

		public bool TryReadDouble(out double data)
		{
			if (Size < ReadIndex + 8)
			{
				data = 0;
				return false;
			}

			ReadIndex += mNetBuffer.ReadDouble(ReadIndex, out data);
			return true;
		}

		public bool TryReadBytes(out byte[] data)
		{
			if (Size < ReadIndex + 2)
			{
				data = new byte[0];
				return false;
			}

			int curReadIndex = ReadIndex;
			curReadIndex += mNetBuffer.ReadInt16(ReadIndex, out short dataLength);

			if (mNetBuffer.TryCopyBytes(curReadIndex, dataLength, out data))
			{
				ReadIndex = curReadIndex + dataLength;
				return true;
			}

			data = new byte[0];
			return false;
		}

		public bool TryReadString(out string data)
		{
			if (TryReadBytes(out var rawData))
			{
				try
				{
					data = Encoding.UTF8.GetString(rawData);
					return true;
				}
				catch 
				{
					data = string.Empty;
					return false;
				}
			}

			data = string.Empty;
			return false;
		}

		#endregion

		#region Generic Read Operation

		public bool TryRead(out sbyte data) => TryReadInt8(out data);
		public bool TryRead(out byte data) => TryReadUInt8(out data);
		public bool TryRead(out short data) => TryReadInt16(out data);
		public bool TryRead(out ushort data) => TryReadUInt16(out data);
		public bool TryRead(out int data) => TryReadInt32(out data);
		public bool TryRead(out uint data) => TryReadUInt32(out data);
		public bool TryRead(out long data) => TryReadInt64(out data);
		public bool TryRead(out ulong data) => TryReadUInt64(out data);
		public bool TryRead(out float data) => TryReadFloat(out data);
		public bool TryRead(out double data) => TryReadDouble(out data);
		public bool TryRead(out byte[] data) => TryReadBytes(out data);
		public bool TryRead(out string data) => TryReadString(out data);

		#endregion
	}
}
