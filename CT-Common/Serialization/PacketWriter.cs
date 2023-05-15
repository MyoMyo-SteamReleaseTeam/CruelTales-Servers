using System;
using CT.Common.DataType;

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
		public int Count { get; private set; }
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
			Count = 0;
			Capacity = Buffer.Count;
		}

		public void Reset()
		{
			Count = 0;
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
			BinaryConverter.WriteBool(Buffer, Count, value);
			Count += sizeof(byte);
		}

		public void Put(byte value)
		{
			BinaryConverter.WriteByte(Buffer, Count, value);
			Count += sizeof(byte);
		}

		public void Put(sbyte value)
		{
			BinaryConverter.WriteSByte(Buffer, Count, value);
			Count += sizeof(sbyte);
		}

		public void Put(short value)
		{
			BinaryConverter.WriteInt16(Buffer, Count, value);
			Count += sizeof(short);
		}

		public void Put(ushort value)
		{
			BinaryConverter.WriteUInt16(Buffer, Count, value);
			Count += sizeof(ushort);
		}

		public void Put(int value)
		{
			BinaryConverter.WriteInt32(Buffer, Count, value);
			Count += sizeof(int);
		}

		public void Put(uint value)
		{
			BinaryConverter.WriteUInt32(Buffer, Count, value);
			Count += sizeof(uint);
		}

		public void Put(long value)
		{
			BinaryConverter.WriteInt64(Buffer, Count, value);
			Count += sizeof(long);
		}

		public void Put(ulong value)
		{
			BinaryConverter.WriteUInt64(Buffer, Count, value);
			Count += sizeof(ulong);
		}

		public void Put(float value)
		{
			BinaryConverter.WriteSingle(Buffer, Count, value);
			Count += sizeof(float);
		}

		public void Put(double value)
		{
			BinaryConverter.WriteDouble(Buffer, Count, value);
			Count += sizeof(double);
		}

		public void Put(NetString value)
		{
			Count += BinaryConverter.WriteString(Buffer, Count, value);
		}

		public void Put(NetStringShort value)
		{
			BinaryConverter.WriteByte(Buffer, Count, (byte)value.ByteSize);
			Count += sizeof(byte);
			Count += BinaryConverter.WriteStringUnsafe(Buffer, Count, value.Value);
		}

		public void Put(byte[] value)
		{
			Count += BinaryConverter.WriteBytes(Buffer, Count, value);
		}

		public void Put(PacketWriter writer)
		{
			var src = writer.Buffer.Array;
			int srcOffset = writer.Buffer.Offset;
			var dest = this.Buffer.Array;
			int destOffset = this.Buffer.Offset + Count;
			int count = writer.Buffer.Count;
#if NET
			System.Diagnostics.Debug.Assert(src != null);
			System.Diagnostics.Debug.Assert(dest != null);
#endif
			System.Buffer.BlockCopy(src, srcOffset, dest, destOffset, count);
			this.Count += count;
		}
	}
}
