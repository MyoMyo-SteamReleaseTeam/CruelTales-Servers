using System.Diagnostics;
using System.Text;

namespace CT.Network.Serialization.Rerference
{
	public class NetPacketWriter
	{
		private NetPacket _netPacket;
		private ArraySegment<byte> _packetRawData;

		public int WriteIndex => _netPacket.Size;
		public byte[] RawMemory => _netPacket.RawMemory;

		public NetPacketWriter()
		{
			_netPacket = new NetPacket(0);
		}

		public NetPacketWriter(NetPacket packet)
		{
			_netPacket = packet;
			_packetRawData = _netPacket.Data;
		}

		public void SetNetPacket(NetPacket packet)
		{
			_netPacket = packet;
			_packetRawData = _netPacket.Data;
		}

		public void Release()
		{
			_netPacket = null;
			_packetRawData = null;
		}

		public int GetRemainingSize()
		{
			return _netPacket.RemainingSize;
		}

		public bool CanWriteString(string data)
		{
			return CanWrite(Encoding.UTF8.GetByteCount(data) + 2);
		}

		public bool CanWriteBytes(byte[] data)
		{
			return CanWrite(data.Length + 2);
		}

		public bool CanWrite(int size)
		{
			return _netPacket.Size + size <= _netPacket.MaxSize;
		}

		public void OffsetWriteIndex(int offset)
		{
			_netPacket.Size += offset;
		}

		public void MoveWriteIndex(int index)
		{
			_netPacket.Size = index;
		}

		// Header

		public void WriteAt(INetworkSerializable data, int index)
		{
			int tempSize = _netPacket.Size;
			_netPacket.Size = index;
			data.Serialize(this);

			_netPacket.Size = tempSize;
			if (_netPacket.Size < index + data.SerializeSize)
			{
				_netPacket.Size = index + data.SerializeSize;
			}
		}

		// Custom Write

		public bool TryWritePacket(NetPacket packet)
		{
			if (!CanWrite(packet.Size))
			{
				return false;
			}

			WritePacket(packet);
			return true;
		}

		public void WritePacket(NetPacket packet)
		{
			Debug.Assert(packet.Data.Array != null);
			Debug.Assert(_packetRawData.Array != null);

			Buffer.BlockCopy
			(
				packet.Data.Array,
				packet.Data.Offset,
				_packetRawData.Array,
				_packetRawData.Offset + _netPacket.Size,
				packet.Size
			);
			_netPacket.Size += packet.Size;
		}

		public void WritePacket(NetPacket packet, int offset, int count)
		{
			Debug.Assert(packet.Data.Array != null);
			Debug.Assert(_packetRawData.Array != null);

			Buffer.BlockCopy
			(
				packet.Data.Array,
				packet.Data.Offset + offset,
				_packetRawData.Array,
				_packetRawData.Offset,
				count
			);

			_netPacket.Size += count;
		}

		// Default Write

		public void WriteRawData(byte[] data, int offest, int count)
		{
			Debug.Assert(_packetRawData.Array != null);

			Buffer.BlockCopy(data, offest, _packetRawData.Array, _packetRawData.Offset, count);
			_netPacket.Size += count;
		}

		public void WriteBool(bool data)
		{
			_netPacket.Size += DataConverter.EncodeBool(_packetRawData, _netPacket.Size, data);
		}

		public void WriteInt8(sbyte data)
		{
			_netPacket.Size += DataConverter.EncodeInt8(_packetRawData, _netPacket.Size, data);
		}

		public void WriteUInt8(byte data)
		{
			_netPacket.Size += DataConverter.EncodeUInt8(_packetRawData, _netPacket.Size, data);
		}

		public void WriteInt16(short data)
		{
			_netPacket.Size += DataConverter.EncodeInt16(_packetRawData, _netPacket.Size, data);
		}

		public void WriteUInt16(ushort data)
		{
			_netPacket.Size += DataConverter.EncodeUInt16(_packetRawData, _netPacket.Size, data);
		}

		public void WriteInt32(int data)
		{
			_netPacket.Size += DataConverter.EncodeInt32(_packetRawData, _netPacket.Size, data);
		}

		public void WriteUInt32(uint data)
		{
			_netPacket.Size += DataConverter.EncodeUInt32(_packetRawData, _netPacket.Size, data);
		}

		public void WriteInt64(long data)
		{
			_netPacket.Size += DataConverter.EncodeInt64(_packetRawData, _netPacket.Size, data);
		}

		public void WriteUInt64(ulong data)
		{
			_netPacket.Size += DataConverter.EncodeUInt64(_packetRawData, _netPacket.Size, data);
		}

		public void WriteFloat(float data)
		{
			_netPacket.Size += DataConverter.EncodeFloat(_packetRawData, _netPacket.Size, data);
		}

		public void WriteDouble(double data)
		{
			_netPacket.Size += DataConverter.EncodeDouble(_packetRawData, _netPacket.Size, data);
		}

		public void WriteBytes(byte[] data)
		{
			_netPacket.Size += DataConverter.EncodeBytes(_packetRawData, _netPacket.Size, data);
		}

		public void WriteString(string data)
		{
			_netPacket.Size += DataConverter.EncodeString(_packetRawData, _netPacket.Size, data);
		}

		public void WriteUInt8(byte data, int offset)
		{
			DataConverter.EncodeUInt8(_packetRawData, offset, data);
		}

		public void WriteUInt16(ushort data, int offset)
		{	
			DataConverter.EncodeUInt16(_packetRawData, offset, data);
		}

		// Write Overloading
		public void Write(bool data) => WriteBool(data);
		public void Write(sbyte data) => WriteInt8(data);
		public void Write(byte data) => WriteUInt8(data);
		public void Write(short data) => WriteInt16(data);
		public void Write(ushort data) => WriteUInt16(data);
		public void Write(int data) => WriteInt32(data);
		public void Write(uint data) => WriteUInt32(data);
		public void Write(long data) => WriteInt64(data);
		public void Write(ulong data) => WriteUInt64(data);
		public void Write(float data) => WriteFloat(data);
		public void Write(double data) => WriteDouble(data);
		public void Write(string data) => WriteString(data);
		public void Write(byte[] data) => WriteBytes(data);
	}
}
