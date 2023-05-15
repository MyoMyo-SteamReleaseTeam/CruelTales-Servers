using System;
using CT.Common.DataType;

namespace CT.Common.Serialization
{
	public class PacketReader
	{
		public ArraySegment<byte> Buffer { get; private set; }
		public bool IsEnd
		{
			get
			{
				return Position >= Capacity;
			}
		}
		public int Position { get; private set; }
		public int Capacity { get; private set; }

		public PacketReader()
		{
		}

		public PacketReader(ArraySegment<byte> buffer)
		{
			Initialize(buffer);
		}

		public PacketReader(PacketSegment packet)
		{
			Initialize(packet.Buffer);
		}

		public void Initialize(ArraySegment<byte> buffer)
		{
			Buffer = buffer;
			Position = 0;
			Capacity = Buffer.Count;
		}

		public bool CanRead(int readSize)
		{
			return Position + readSize <= Capacity;
		}

		public bool CanRead<T>(T serializeObject) where T : IPacketSerializable
		{
			return CanRead(serializeObject.SerializeSize);
		}

		public void Reset()
		{
			Position = 0;
		}

		public void SetPosition(int position)
		{
			Position = position;
		}

		public void ReadTo<T>(T serializeObject) where T : IPacketSerializable
		{
			serializeObject.Deserialize(this);
		}

		public void IgnoreAll()
		{
			Position = Capacity;
		}

		public void Ignore(int count)
		{
			Position += count;
		}

		public T Read<T>() where T : IPacketSerializable, new()
		{
			T instance = new();
			ReadTo(instance);
			return instance;
		}

		public void ReadBufferToEnd(byte* buffer)
		{

		}

		#region Peek

		public bool PeekBool()
		{
			var value = BinaryConverter.ReadBool(Buffer, Position);
			return value;
		}

		public byte PeekByte()
		{
			var value = BinaryConverter.ReadByte(Buffer, Position);
			return value;
		}

		public sbyte PeekSByte()
		{
			var value = BinaryConverter.ReadSByte(Buffer, Position);
			return value;
		}

		public short PeekInt16()
		{
			var value = BinaryConverter.ReadInt16(Buffer, Position);
			return value;
		}

		public ushort PeekUInt16()
		{
			var value = BinaryConverter.ReadUInt16(Buffer, Position);
			return value;
		}

		public int PeekInt32()
		{
			var value = BinaryConverter.ReadInt32(Buffer, Position);
			return value;
		}

		public uint PeekUInt32()
		{
			var value = BinaryConverter.ReadUInt32(Buffer, Position);
			return value;
		}

		public long PeekInt64()
		{
			var value = BinaryConverter.ReadInt64(Buffer, Position);
			return value;
		}

		public ulong PeekUInt64()
		{
			var value = BinaryConverter.ReadUInt64(Buffer, Position);
			return value;
		}

		public float PeekSingle()
		{
			var value = BinaryConverter.ReadFloat(Buffer, Position);
			return value;
		}

		public double PeekDouble()
		{
			var value = BinaryConverter.ReadDouble(Buffer, Position);
			return value;
		}

		#endregion

		#region Read

		public bool ReadBool()
		{
			var value = BinaryConverter.ReadBool(Buffer, Position);
			Position += sizeof(byte);
			return value;
		}

		public byte ReadByte()
		{
			var value = BinaryConverter.ReadByte(Buffer, Position);
			Position += sizeof(byte);
			return value;
		}

		public sbyte ReadSByte()
		{
			var value = BinaryConverter.ReadSByte(Buffer, Position);
			Position += sizeof(sbyte);
			return value;
		}

		public short ReadInt16()
		{
			var value = BinaryConverter.ReadInt16(Buffer, Position);
			Position += sizeof(short);
			return value;
		}

		public ushort ReadUInt16()
		{
			var value = BinaryConverter.ReadUInt16(Buffer, Position);
			Position += sizeof(ushort);
			return value;
		}

		public int ReadInt32()
		{
			var value = BinaryConverter.ReadInt32(Buffer, Position);
			Position += sizeof(int);
			return value;
		}

		public uint ReadUInt32()
		{
			var value = BinaryConverter.ReadUInt32(Buffer, Position);
			Position += sizeof(uint);
			return value;
		}

		public long ReadInt64()
		{
			var value = BinaryConverter.ReadInt64(Buffer, Position);
			Position += sizeof(long);
			return value;
		}

		public ulong ReadUInt64()
		{
			var value = BinaryConverter.ReadUInt64(Buffer, Position);
			Position += sizeof(ulong);
			return value;
		}

		public float ReadSingle()
		{
			var value = BinaryConverter.ReadFloat(Buffer, Position);
			Position += sizeof(float);
			return value;
		}

		public double ReadDouble()
		{
			var value = BinaryConverter.ReadDouble(Buffer, Position);
			Position += sizeof(double);
			return value;
		}

		public NetString ReadNetString()
		{
			NetString result = BinaryConverter.ReadString(Buffer, Position, out int read);
			Position += read;
			return result;
		}

		public NetStringShort ReadNetStringShort()
		{
			int stringSize = BinaryConverter.ReadByte(Buffer, Position);
			Position++;
			NetStringShort result = BinaryConverter.ReadStringByLength(Buffer, Position, stringSize);
			Position += stringSize;
			return result;
		}

		public void ReadBytesCopy(ArraySegment<byte> dest, int offset)
		{
			Position += BinaryConverter.ReadBytesCopy(Buffer, Position, dest, offset);
		}

		public byte[] ReadBytes()
		{
			var result = BinaryConverter.ReadBytes(Buffer, Position, out int read);
			Position += read;
			return result;
		}

		#endregion
	}
}
