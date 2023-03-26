using CT.Network.Serialization.Type;

namespace CT.Network.Serialization
{
	public class PacketWriter
	{
		private PacketSegment _packet;
		private ArraySegment<byte> _buffer;

		public int Position { get; private set; }
		public int Capacity => _packet.Capacity;

		public PacketWriter()
		{
			_packet = new PacketSegment();
		}

		public PacketWriter(PacketSegment packet)
		{
			_packet = packet;
			_buffer = packet.Buffer;
			Position = 0;
		}

		public void SetPakcet(PacketSegment packet)
		{
			_packet = packet;
			_buffer = packet.Buffer;
			Position = 0;
		}

		public bool CanPut(int putSize)
		{
			return Position + putSize <= Capacity;
		}

		public bool CanPut<T>(T serializeObject) where T : IPacketSerializable
		{
			return CanPut(serializeObject.SerializeSize);
		}

		public void Put<T>(T serializeObject) where T : IPacketSerializable
		{
			serializeObject.Serialize(this);
		}

		public void Put(bool value)
		{
			BinaryConverter.WriteBool(_buffer, Position, value);
			Position += sizeof(byte);
		}

		public void Put(byte value)
		{
			BinaryConverter.WriteByte(_buffer, Position, value);
			Position += sizeof(byte);
		}

		public void Put(sbyte value)
		{
			BinaryConverter.WriteSByte(_buffer, Position, value);
			Position += sizeof(sbyte);
		}

		public void Put(short value)
		{
			BinaryConverter.WriteInt16(_buffer, Position, value);
			Position += sizeof(short);
		}

		public void Put(ushort value)
		{
			BinaryConverter.WriteUInt16(_buffer, Position, value);
			Position += sizeof(ushort);
		}

		public void Put(int value)
		{
			BinaryConverter.WriteInt32(_buffer, Position, value);
			Position += sizeof(int);
		}

		public void Put(uint value)
		{
			BinaryConverter.WriteUInt32(_buffer, Position, value);
			Position += sizeof(uint);
		}

		public void Put(long value)
		{
			BinaryConverter.WriteInt64(_buffer, Position, value);
			Position += sizeof(long);
		}

		public void Put(ulong value)
		{
			BinaryConverter.WriteUInt64(_buffer, Position, value);
			Position += sizeof(ulong);
		}

		public void Put(float value)
		{
			BinaryConverter.WriteFloat(_buffer, Position, value);
			Position += sizeof(float);
		}

		public void Put(double value)
		{
			BinaryConverter.WriteDouble(_buffer, Position, value);
			Position += sizeof(double);
		}

		public void Put(NetString value)
		{
			Position += BinaryConverter.WriteString(_buffer, Position, value);
		}

		public void Put(NetStringShort value)
		{
			BinaryConverter.WriteByte(_buffer, Position, (byte)value.ByteSize);
			Position += sizeof(byte);
			Position += BinaryConverter.WriteStringUnsafe(_buffer, Position, value.Value);
		}

		public void Put(byte[] value)
		{
			Position += BinaryConverter.WriteBytes(_buffer, Position, value);
		}
	}
}
