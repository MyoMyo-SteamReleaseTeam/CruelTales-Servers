using CT.Network.Serialization.Type;

namespace CT.Network.Serialization
{
	public class PacketReader
	{
		private PacketSegment _packet;
		private ArraySegment<byte> _buffer;

		public int Position { get; private set; }
		public int Capacity => _packet.Capacity;

		public PacketReader()
		{
			_packet = new PacketSegment();
		}

		public PacketReader(PacketSegment packet)
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

		public bool CanRead(int readSize)
		{
			return Position + readSize <= Capacity;
		}

		public bool CanRead<T>(T serializeObject) where T : IPacketSerializable
		{
			return CanRead(serializeObject.SerializeSize);
		}

		public void SetPosition(int position)
		{
			Position = position;
		}

		public void Read<T>(T serializeObject) where T : IPacketSerializable
		{
			serializeObject.Deserialize(this);
		}

		public bool ReadBool()
		{
			var value = BinaryConverter.ReadBool(_buffer, Position);
			Position += sizeof(byte);
			return value;
		}

		public byte ReadByte()
		{
			var value = BinaryConverter.ReadByte(_buffer, Position);
			Position += sizeof(byte);
			return value;
		}

		public sbyte ReadSByte()
		{
			var value = BinaryConverter.ReadSByte(_buffer, Position);
			Position += sizeof(sbyte);
			return value;
		}

		public short ReadInt16()
		{
			var value = BinaryConverter.ReadInt16(_buffer, Position);
			Position += sizeof(short);
			return value;
		}

		public ushort ReadUInt16()
		{
			var value = BinaryConverter.ReadUInt16(_buffer, Position);
			Position += sizeof(ushort);
			return value;
		}

		public int ReadInt32()
		{
			var value = BinaryConverter.ReadInt32(_buffer, Position);
			Position += sizeof(int);
			return value;
		}

		public uint ReadUInt32()
		{
			var value = BinaryConverter.ReadUInt32(_buffer, Position);
			Position += sizeof(uint);
			return value;
		}

		public long ReadInt64()
		{
			var value = BinaryConverter.ReadInt64(_buffer, Position);
			Position += sizeof(long);
			return value;
		}

		public ulong ReadUInt64()
		{
			var value = BinaryConverter.ReadUInt64(_buffer, Position);
			Position += sizeof(ulong);
			return value;
		}

		public float ReadSingle()
		{
			var value = BinaryConverter.ReadFloat(_buffer, Position);
			Position += sizeof(float);
			return value;
		}

		public double ReadDouble()
		{
			var value = BinaryConverter.ReadDouble(_buffer, Position);
			Position += sizeof(double);
			return value;
		}

		public NetString ReadNetString()
		{
			NetString result = BinaryConverter.ReadString(_buffer, Position, out int read);
			Position += read;
			return result;
		}

		public NetStringShort ReadNetStringShort()
		{
			int stringSize = BinaryConverter.ReadByte(_buffer, Position);
			Position ++;
			NetStringShort result = BinaryConverter.ReadStringByLength(_buffer, Position, stringSize);
			Position += stringSize;
			return result;
		}

		public void ReadBytesCopy(ArraySegment<byte> dest, int offset)
		{
			Position += BinaryConverter.ReadBytesCopy(_buffer, Position, dest, offset);
		}

		public byte[] ReadBytes()
		{
			var result = BinaryConverter.ReadBytes(_buffer, Position, out int read);
			Position += read;
			return result;
		}
	}
}
