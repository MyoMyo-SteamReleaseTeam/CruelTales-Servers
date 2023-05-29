using System;
using System.Diagnostics;
using CT.Common.DataType;

namespace CT.Common.Serialization
{
	public class ByteBuffer : IPacketReader, IPacketWriter
	{
		public ArraySegment<byte> ByteSegment { get; private set; }
		public bool IsReadEnd => ReadPosition >= Capacity;
		public bool IsWriteEnd => Size >= Capacity;
		public int ReadPosition { get; private set; }
		public int Size { get; private set; }
		public int Capacity { get; private set; }

		public ByteBuffer()
		{
		}

		//[Obsolete("Need to pool")]
		public ByteBuffer(int count)
		{
			byte[] bytes = new byte[count];
			ByteSegment = new ArraySegment<byte>(bytes);
			Capacity = count;
		}

		public ByteBuffer(ArraySegment<byte> buffer, int size)
		{
			Initialize(buffer, size);
		}

		public void Initialize(ArraySegment<byte> buffer, int size)
		{
			ByteSegment = buffer;
			Size = size;
			ReadPosition = 0;
			Capacity = ByteSegment.Count;
		}

		public void Reset()
		{
			ReadPosition = 0;
			Size = 0;
		}

		public void ResetReader()
		{
			ReadPosition = 0;
		}

		public void ResetWriter()
		{
			Size = 0;
		}

		#region Write

		public void SetSize(int size)
		{
			Size = size;
		}

		public void OffsetSize(int offset)
		{
			Size += offset;
		}

		public bool CanPut(int putSize)
		{
			return Size + putSize <= Capacity;
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
			BinaryConverter.WriteBool(ByteSegment, Size, value);
			Size += sizeof(byte);
		}

		public void Put(byte value)
		{
			BinaryConverter.WriteByte(ByteSegment, Size, value);
			Size += sizeof(byte);
		}

		public void Put(sbyte value)
		{
			BinaryConverter.WriteSByte(ByteSegment, Size, value);
			Size += sizeof(sbyte);
		}

		public void Put(short value)
		{
			BinaryConverter.WriteInt16(ByteSegment, Size, value);
			Size += sizeof(short);
		}

		public void Put(ushort value)
		{
			BinaryConverter.WriteUInt16(ByteSegment, Size, value);
			Size += sizeof(ushort);
		}

		public void Put(int value)
		{
			BinaryConverter.WriteInt32(ByteSegment, Size, value);
			Size += sizeof(int);
		}

		public void Put(uint value)
		{
			BinaryConverter.WriteUInt32(ByteSegment, Size, value);
			Size += sizeof(uint);
		}

		public void Put(long value)
		{
			BinaryConverter.WriteInt64(ByteSegment, Size, value);
			Size += sizeof(long);
		}

		public void Put(ulong value)
		{
			BinaryConverter.WriteUInt64(ByteSegment, Size, value);
			Size += sizeof(ulong);
		}

		public void Put(float value)
		{
			BinaryConverter.WriteSingle(ByteSegment, Size, value);
			Size += sizeof(float);
		}

		public void Put(double value)
		{
			BinaryConverter.WriteDouble(ByteSegment, Size, value);
			Size += sizeof(double);
		}

		public void Put(NetString value)
		{
			Size += BinaryConverter.WriteString(ByteSegment, Size, value);
		}

		public void Put(NetStringShort value)
		{
			BinaryConverter.WriteByte(ByteSegment, Size, (byte)value.ByteSize);
			Size += sizeof(byte);
			Size += BinaryConverter.WriteStringUnsafe(ByteSegment, Size, value.Value);
		}

		public void Put(byte[] value)
		{
			Size += BinaryConverter.WriteBytes(ByteSegment, Size, value);
		}

		public void Put(IPacketWriter writer)
		{
			var src = writer.ByteSegment.Array;
			int srcOffset = writer.ByteSegment.Offset;
			var dest = this.ByteSegment.Array;
			int destOffset = this.ByteSegment.Offset + Size;
			int count = writer.ByteSegment.Count;
#if NET
			System.Diagnostics.Debug.Assert(src != null);
			System.Diagnostics.Debug.Assert(dest != null);
#endif
			System.Buffer.BlockCopy(src, srcOffset, dest, destOffset, count);
			this.Size += count;
		}

		public void Put(ArraySegment<byte> buffer, int count)
		{
			Debug.Assert(buffer.Array != null);
			Debug.Assert(ByteSegment.Array != null);
			Buffer.BlockCopy(buffer.Array, buffer.Offset,
							 ByteSegment.Array, ByteSegment.Offset + Size, count);
			Size += count;
		}

		#endregion

		#region Read

		public bool CanRead(int readSize)
		{
			return ReadPosition + readSize <= Size;
		}

		public bool CanRead<T>(T serializeObject) where T : IPacketSerializable
		{
			return CanRead(serializeObject.SerializeSize);
		}

		public void SetReadPosition(int position)
		{
			ReadPosition = position;
		}

		public bool TryReadTo<T>(in T serializeObject) where T : IPacketSerializable
		{
			return serializeObject.TryDeserialize(this);
		}

		public void IgnoreAll()
		{
			ReadPosition = Capacity;
		}

		public void Ignore(int count)
		{
			ReadPosition += count;
		}

		public bool TryRead<T>(out T value) where T : IPacketSerializable, new()
		{
			value = new();
			return TryReadTo(value);
		}

		// Peek

		public bool PeekBool()
		{
			var value = BinaryConverter.ReadBool(ByteSegment, ReadPosition);
			return value;
		}

		public byte PeekByte()
		{
			var value = BinaryConverter.ReadByte(ByteSegment, ReadPosition);
			return value;
		}

		public sbyte PeekSByte()
		{
			var value = BinaryConverter.ReadSByte(ByteSegment, ReadPosition);
			return value;
		}

		public short PeekInt16()
		{
			var value = BinaryConverter.ReadInt16(ByteSegment, ReadPosition);
			return value;
		}

		public ushort PeekUInt16()
		{
			var value = BinaryConverter.ReadUInt16(ByteSegment, ReadPosition);
			return value;
		}

		public int PeekInt32()
		{
			var value = BinaryConverter.ReadInt32(ByteSegment, ReadPosition);
			return value;
		}

		public uint PeekUInt32()
		{
			var value = BinaryConverter.ReadUInt32(ByteSegment, ReadPosition);
			return value;
		}

		public long PeekInt64()
		{
			var value = BinaryConverter.ReadInt64(ByteSegment, ReadPosition);
			return value;
		}

		public ulong PeekUInt64()
		{
			var value = BinaryConverter.ReadUInt64(ByteSegment, ReadPosition);
			return value;
		}

		public float PeekSingle()
		{
			var value = BinaryConverter.ReadSingle(ByteSegment, ReadPosition);
			return value;
		}

		public double PeekDouble()
		{
			var value = BinaryConverter.ReadDouble(ByteSegment, ReadPosition);
			return value;
		}

		// Read

		public bool ReadBool()
		{
			var value = BinaryConverter.ReadBool(ByteSegment, ReadPosition);
			ReadPosition += sizeof(byte);
			return value;
		}

		public byte ReadByte()
		{
			var value = BinaryConverter.ReadByte(ByteSegment, ReadPosition);
			ReadPosition += sizeof(byte);
			return value;
		}

		public sbyte ReadSByte()
		{
			var value = BinaryConverter.ReadSByte(ByteSegment, ReadPosition);
			ReadPosition += sizeof(sbyte);
			return value;
		}

		public short ReadInt16()
		{
			var value = BinaryConverter.ReadInt16(ByteSegment, ReadPosition);
			ReadPosition += sizeof(short);
			return value;
		}

		public ushort ReadUInt16()
		{
			var value = BinaryConverter.ReadUInt16(ByteSegment, ReadPosition);
			ReadPosition += sizeof(ushort);
			return value;
		}

		public int ReadInt32()
		{
			var value = BinaryConverter.ReadInt32(ByteSegment, ReadPosition);
			ReadPosition += sizeof(int);
			return value;
		}

		public uint ReadUInt32()
		{
			var value = BinaryConverter.ReadUInt32(ByteSegment, ReadPosition);
			ReadPosition += sizeof(uint);
			return value;
		}

		public long ReadInt64()
		{
			var value = BinaryConverter.ReadInt64(ByteSegment, ReadPosition);
			ReadPosition += sizeof(long);
			return value;
		}

		public ulong ReadUInt64()
		{
			var value = BinaryConverter.ReadUInt64(ByteSegment, ReadPosition);
			ReadPosition += sizeof(ulong);
			return value;
		}

		public float ReadSingle()
		{
			var value = BinaryConverter.ReadSingle(ByteSegment, ReadPosition);
			ReadPosition += sizeof(float);
			return value;
		}

		public double ReadDouble()
		{
			var value = BinaryConverter.ReadDouble(ByteSegment, ReadPosition);
			ReadPosition += sizeof(double);
			return value;
		}

		public NetString ReadNetString()
		{
			NetString result = BinaryConverter.ReadString(ByteSegment, ReadPosition, out int read);
			ReadPosition += read;
			return result;
		}

		public NetStringShort ReadNetStringShort()
		{
			int stringSize = BinaryConverter.ReadByte(ByteSegment, ReadPosition);
			ReadPosition++;
			NetStringShort result = BinaryConverter.ReadStringByLength(ByteSegment, ReadPosition, stringSize);
			ReadPosition += stringSize;
			return result;
		}

		public void ReadBytesCopy(ArraySegment<byte> dest, int offset)
		{
			ReadPosition += BinaryConverter.ReadBytesCopy(ByteSegment, ReadPosition, dest, offset);
		}

		public byte[] ReadBytes()
		{
			var result = BinaryConverter.ReadBytes(ByteSegment, ReadPosition, out int read);
			ReadPosition += read;
			return result;
		}

		#endregion

		#region TryRead

		public bool TryPeekBool(out bool value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = false;
				return false;
			}
			value = BinaryConverter.ReadBool(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekByte(out byte value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadByte(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekSByte(out sbyte value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadSByte(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekInt16(out short value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadInt16(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekUInt16(out ushort value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadUInt16(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekInt32(out int value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadInt32(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekUInt32(out uint value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadUInt32(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekInt64(out long value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadInt64(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekUInt64(out ulong value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadUInt64(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekSingle(out float value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadSingle(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryPeekDouble(out double value)
		{
			if (!CanRead(sizeof(bool)))
			{
				value = 0;
				return false;
			}
			value = BinaryConverter.ReadDouble(ByteSegment, ReadPosition);
			return true;
		}
		public bool TryReadBool(out bool value)
		{
			if (TryPeekBool(out value))
			{
				ReadPosition += sizeof(byte);
				return true;
			}
			return false;
		}
		public bool TryReadByte(out byte value)
		{
			if (TryPeekByte(out value))
			{
				ReadPosition += sizeof(byte);
				return true;
			}
			return false;
		}
		public bool TryReadSByte(out sbyte value)
		{
			if (TryPeekSByte(out value))
			{
				ReadPosition += sizeof(sbyte);
				return true;
			}
			return false;
		}
		public bool TryReadInt16(out short value)
		{
			if (TryPeekInt16(out value))
			{
				ReadPosition += sizeof(short);
				return true;
			}
			return false;
		}
		public bool TryReadUInt16(out ushort value)
		{
			if (TryPeekUInt16(out value))
			{
				ReadPosition += sizeof(ushort);
				return true;
			}
			return false;
		}
		public bool TryReadInt32(out int value)
		{
			if (TryPeekInt32(out value))
			{
				ReadPosition += sizeof(int);
				return true;
			}
			return false;
		}
		public bool TryReadUInt32(out uint value)
		{
			if (TryPeekUInt32(out value))
			{
				ReadPosition += sizeof(uint);
				return true;
			}
			return false;
		}
		public bool TryReadInt64(out long value)
		{
			if (TryPeekInt64(out value))
			{
				ReadPosition += sizeof(long);
				return true;
			}
			return false;
		}
		public bool TryReadUInt64(out ulong value)
		{
			if (TryPeekUInt64(out value))
			{
				ReadPosition += sizeof(ulong);
				return true;
			}
			return false;
		}
		public bool TryReadSingle(out float value)
		{
			if (TryPeekSingle(out value))
			{
				ReadPosition += sizeof(float);
				return true;
			}
			return false;
		}
		public bool TryReadDouble(out double value)
		{
			if (TryPeekDouble(out value))
			{
				ReadPosition += sizeof(double);
				return true;
			}
			return false;
		}
		public bool TryReadNetString(out string value)
		{
			if (!(TryPeekUInt16(out var byteLength) && CanRead(byteLength)))
			{
				value = string.Empty;
				return false;
			}

			ReadPosition += sizeof(ushort);
			Debug.Assert(ByteSegment.Array != null);
			value = BinaryConverter.Utf8Encoding.GetString(ByteSegment.Array,
														   ByteSegment.Offset + ReadPosition,
														   byteLength);
			return true;
		}
		public bool TryReadNetStringShort(out string value)
		{
			if (!(TryPeekByte(out var byteLength) && CanRead(byteLength)))
			{
				value = string.Empty;
				return false;
			}

			ReadPosition += sizeof(byte);
			Debug.Assert(ByteSegment.Array != null);
			value = BinaryConverter.Utf8Encoding.GetString(ByteSegment.Array,
														   ByteSegment.Offset + ReadPosition,
														   byteLength);
			return true;
		}

		#endregion
	}
}
