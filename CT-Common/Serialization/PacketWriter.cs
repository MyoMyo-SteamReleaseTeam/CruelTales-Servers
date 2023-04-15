using System;
using CT.Common.Serialization.Type;

namespace CT.Common.Serialization
{
	public class PacketWriter
	{
		public ArraySegment<byte> Buffer { get; private set; }
		public bool IsEnd
		{
			get
			{
				return Count >= Capacity;
			}
		}
		public int Count => Position - Start;
		public int Start { get; private set; }
		public int Position { get; private set; }
		public int Capacity { get; private set; }

		public PacketWriter(PacketSegment packet)
		{
			Initialize(packet.Buffer);
		}

		public PacketWriter(ArraySegment<byte> buffer)
		{
			Initialize(buffer);
		}

		public void Initialize(ArraySegment<byte> buffer)
		{
			Buffer = buffer;
			Start = Buffer.Offset;
			Position = Buffer.Offset;
			Capacity = Buffer.Count;
		}

		public void Reset()
		{
			Position = Buffer.Offset;
		}

		public bool CanPut(int putSize)
		{
			return Count + putSize <= Capacity;
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
			BinaryConverter.WriteBool(Buffer, Position, value);
			Position += sizeof(byte);
		}

		public void Put(byte value)
		{
			BinaryConverter.WriteByte(Buffer, Position, value);
			Position += sizeof(byte);
		}

		public void Put(sbyte value)
		{
			BinaryConverter.WriteSByte(Buffer, Position, value);
			Position += sizeof(sbyte);
		}

		public void Put(short value)
		{
			BinaryConverter.WriteInt16(Buffer, Position, value);
			Position += sizeof(short);
		}

		public void Put(ushort value)
		{
			BinaryConverter.WriteUInt16(Buffer, Position, value);
			Position += sizeof(ushort);
		}

		public void Put(int value)
		{
			BinaryConverter.WriteInt32(Buffer, Position, value);
			Position += sizeof(int);
		}

		public void Put(uint value)
		{
			BinaryConverter.WriteUInt32(Buffer, Position, value);
			Position += sizeof(uint);
		}

		public void Put(long value)
		{
			BinaryConverter.WriteInt64(Buffer, Position, value);
			Position += sizeof(long);
		}

		public void Put(ulong value)
		{
			BinaryConverter.WriteUInt64(Buffer, Position, value);
			Position += sizeof(ulong);
		}

		public void Put(float value)
		{
			BinaryConverter.WriteSingle(Buffer, Position, value);
			Position += sizeof(float);
		}

		public void Put(double value)
		{
			BinaryConverter.WriteDouble(Buffer, Position, value);
			Position += sizeof(double);
		}

		public void Put(NetString value)
		{
			Position += BinaryConverter.WriteString(Buffer, Position, value);
		}

		public void Put(NetStringShort value)
		{
			BinaryConverter.WriteByte(Buffer, Position, (byte)value.ByteSize);
			Position += sizeof(byte);
			Position += BinaryConverter.WriteStringUnsafe(Buffer, Position, value.Value);
		}

		public void Put(byte[] value)
		{
			Position += BinaryConverter.WriteBytes(Buffer, Position, value);
		}
	}
}
