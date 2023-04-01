using System.Diagnostics;
using System.Text;

namespace CT.Network.Legacy.Rerference
{
	public class NetPacketReader
	{
		private NetPacket? _netPacket;
		private ArraySegment<byte> _packetRawData;

		public int ReadIndex { get; private set; }
		public byte[] RawMemory => _netPacket.RawMemory;

		public NetPacketReader(NetPacket packet)
		{
			SetNetPacket(packet);
		}

		public void SetNetPacket(NetPacket packet)
		{
			_netPacket = packet;
			_packetRawData = _netPacket.Data;
			ReadIndex = 0;
		}

		public void Release()
		{
			_netPacket = null;
			_packetRawData = null;
			ReadIndex = 0;
		}

		public void ResetReadIndex()
		{
			ReadIndex = 0;
		}

		public void OffsetReadIndex(int offset)
		{
			ReadIndex += offset;
		}

		public void SetIndex(int index)
		{
			ReadIndex = index;
		}

		public bool CanRead(int size)
		{
			return ReadIndex + size <= _netPacket.Size;
		}

		public bool CanReadString() => CanReadBytes();

		public bool CanReadBytes()
		{
			if (!CanRead(2))
				return false;

			int byteSize = PeekUInt16();

			return CanRead(2 + byteSize);
		}

		// Reading

		public bool ReadBool()
		{
			ReadIndex += DataConverter.DecodeBool(_packetRawData, ReadIndex, out bool data);
			return data;
		}

		public sbyte ReadInt8()
		{
			ReadIndex += DataConverter.DecodeInt8(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public byte ReadUInt8()
		{
			ReadIndex += DataConverter.DecodeUInt8(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public short ReadInt16()
		{
			ReadIndex += DataConverter.DecodeInt16(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public ushort ReadUInt16()
		{
			ReadIndex += DataConverter.DecodeUInt16(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public int ReadInt32()
		{
			ReadIndex += DataConverter.DecodeInt32(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public uint ReadUInt32()
		{
			ReadIndex += DataConverter.DecodeUInt32(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public long ReadInt64()
		{
			ReadIndex += DataConverter.DecodeInt64(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public ulong ReadUInt64()
		{
			ReadIndex += DataConverter.DecodeUInt64(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public float ReadFloat()
		{
			ReadIndex += DataConverter.DecodeFloat(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public double ReadDouble()
		{
			ReadIndex += DataConverter.DecodeDouble(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public byte[] ReadBytes()
		{
			Debug.Assert(_packetRawData.Array != null);

			int curReadIndex = ReadIndex;
			curReadIndex += DataConverter.DecodeInt16(_packetRawData, ReadIndex, out short dataLength);
			byte[] rawData = new byte[dataLength];
			Buffer.BlockCopy(_packetRawData.Array, _packetRawData.Offset + curReadIndex, rawData, 0, dataLength);
			ReadIndex = curReadIndex + dataLength;
			return rawData;
		}

		public string ReadString()
		{
			var rawData = ReadBytes();
			return Encoding.UTF8.GetString(rawData);
		}

		// Try Reading

		public bool TryReadBool(out bool data)
		{
			if (!CanRead(1))
			{
				data = false;
				return false;
			}

			data = ReadBool();
			return true;
		}

		public bool TryReadInt8(out sbyte data)
		{
			if (!CanRead(1))
			{
				data = 0;
				return false;
			}

			data = ReadInt8();
			return true;
		}

		public bool TryReadUInt8(out byte data)
		{
			if (!CanRead(1))
			{
				data = 0;
				return false;
			}

			data = ReadUInt8();
			return true;
		}

		public bool TryReadInt16(out short data)
		{
			if (!CanRead(2))
			{
				data = 0;
				return false;
			}

			data = ReadInt16();
			return true;
		}

		public bool TryReadUInt16(out ushort data)
		{
			if (!CanRead(2))
			{
				data = 0;
				return false;
			}

			data = ReadUInt16();
			return true;
		}

		public bool TryReadInt32(out int data)
		{
			if (!CanRead(4))
			{
				data = 0;
				return false;
			}

			data = ReadInt32();
			return true;
		}

		public bool TryReadUInt32(out uint data)
		{
			if (!CanRead(4))
			{
				data = 0;
				return false;
			}

			data = ReadUInt32();
			return true;
		}

		public bool TryReadInt64(out long data)
		{
			if (!CanRead(8))
			{
				data = 0;
				return false;
			}

			data = ReadInt64();
			return true;
		}

		public bool TryReadUInt64(out ulong data)
		{
			if (!CanRead(8))
			{
				data = 0;
				return false;
			}

			data = ReadUInt64();
			return true;
		}

		public bool TryReadFloat(out float data)
		{
			if (!CanRead(4))
			{
				data = 0;
				return false;
			}

			data = ReadFloat();
			return true;
		}

		public bool TryReadDouble(out double data)
		{
			if (!CanRead(8))
			{
				data = 0;
				return false;
			}

			data = ReadDouble();
			return true;
		}

		public bool TryReadBytes(out byte[] data)
		{
			if (!CanReadBytes())
			{
				data = new byte[0];
				return false;
			}

			data = ReadBytes();
			return true;
		}

		public bool TryReadString(out string data)
		{
			if (!CanReadString())
			{
				data = string.Empty;
				return false;
			}

			data = ReadString();
			return true;
		}

		// Peeking

		public sbyte PeekInt8()
		{
			DataConverter.DecodeInt8(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public byte PeekUInt8()
		{
			DataConverter.DecodeUInt8(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public short PeekInt16()
		{
			DataConverter.DecodeInt16(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public ushort PeekUInt16()
		{
			DataConverter.DecodeUInt16(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public int PeekInt32()
		{
			DataConverter.DecodeInt32(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public uint PeekUInt32()
		{
			DataConverter.DecodeUInt32(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public long PeekInt64()
		{
			DataConverter.DecodeInt64(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public ulong PeekUInt64()
		{
			DataConverter.DecodeUInt64(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public float PeekFloat()
		{
			DataConverter.DecodeFloat(_packetRawData, ReadIndex, out var data);
			return data;
		}

		public double PeekDouble()
		{
			DataConverter.DecodeDouble(_packetRawData, ReadIndex, out var data);
			return data;
		}
	}
}
