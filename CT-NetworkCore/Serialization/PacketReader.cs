#if NET7_0_OR_GREATER
#else
using System;
#endif
using CT.Network.Serialization.Type;

namespace CT.Network.Serialization
{
	public class PacketReader
	{
		private ArraySegment<byte> _buffer;
		public int Position { get; private set; }
		public int Capacity => _buffer.Count;
		public bool IsEnd => Capacity == Position;

		public PacketReader(ArraySegment<byte> buffer)
		{
			_buffer = buffer;
			Position = 0;
		}

		public PacketReader(PacketSegment packet)
		{
			_buffer = packet.Buffer;
			Position = 0;
		}

		public void Initialize(PacketSegment packet)
		{
			_buffer = packet.Buffer;
			Position = 0;
		}

		public void Initialize(ArraySegment<byte> buffer)
		{
			_buffer = buffer;
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

		public void ReadTo<T>(T serializeObject) where T : IPacketSerializable
		{
			serializeObject.Deserialize(this);
		}

		public T Read<T>() where T : IPacketSerializable, new()
		{
			T instance = new();
			this.ReadTo(instance);
			return instance;
		}

		#region Peek

		public bool PeekBool()
		{
			var value = BinaryConverter.ReadBool(_buffer, Position);
			return value;
		}

		public byte PeekByte()
		{
			var value = BinaryConverter.ReadByte(_buffer, Position);
			return value;
		}

		public sbyte PeekSByte()
		{
			var value = BinaryConverter.ReadSByte(_buffer, Position);
			return value;
		}

		public short PeekInt16()
		{
			var value = BinaryConverter.ReadInt16(_buffer, Position);
			return value;
		}

		public ushort PeekUInt16()
		{
			var value = BinaryConverter.ReadUInt16(_buffer, Position);
			return value;
		}

		public int PeekInt32()
		{
			var value = BinaryConverter.ReadInt32(_buffer, Position);
			return value;
		}

		public uint PeekUInt32()
		{
			var value = BinaryConverter.ReadUInt32(_buffer, Position);
			return value;
		}

		public long PeekInt64()
		{
			var value = BinaryConverter.ReadInt64(_buffer, Position);
			return value;
		}

		public ulong PeekUInt64()
		{
			var value = BinaryConverter.ReadUInt64(_buffer, Position);
			return value;
		}

		public float PeekSingle()
		{
			var value = BinaryConverter.ReadFloat(_buffer, Position);
			return value;
		}

		public double PeekDouble()
		{
			var value = BinaryConverter.ReadDouble(_buffer, Position);
			return value;
		}

		#endregion

		#region Read

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

		#endregion
	}
}
