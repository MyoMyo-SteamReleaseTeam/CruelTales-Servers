using System.Diagnostics;

namespace CT.Network.Legacy.Rerference
{
	public class NetPacket : IManageable
	{
		private byte[] _rawData;
		public readonly ArraySegment<byte> Data;
		public int MaxSize { get; private set; }
		public int Size;
		public int RemainingSize => MaxSize - Size;
		public byte[] RawMemory
		{
			get
			{
				GetRawData(out var rawData, out int length);
				return rawData;
			}
		}

		public NetPacket(ArraySegment<byte> memory)
		{
			_rawData = new byte[0];
			Data = memory;
			MaxSize = Data.Count;

			OnInitialize();
		}

		public NetPacket(int maxSize)
		{
			MaxSize = maxSize;
			_rawData = new byte[MaxSize];
			Data = new ArraySegment<byte>(_rawData);

			OnInitialize();
		}

		public void Reserve(int capacity)
		{
			Debug.Assert(_rawData != null);

			if (capacity <= Size)
			{
				return;
			}

			var newData = new byte[capacity];
			Buffer.BlockCopy(_rawData, 0, newData, 0, Size);
			_rawData = newData;
		}

		public void OnInitialize() => Clear();

		public void OnFinalize() { }

		public void Clear()
		{
			Size = 0;
		}

		public void GetRawData(out byte[] rawData, out int length)
		{
			Debug.Assert(Data.Array != null);
			length = Size;
			rawData = new byte[length];
			Buffer.BlockCopy(Data.Array, Data.Offset, rawData, 0, length);
		}

		public NetPacketReader GetReader() => new NetPacketReader(this);
		public NetPacketWriter GetWriter() => new NetPacketWriter(this);

		public void WriteInt8(int offset, sbyte data) => DataConverter.EncodeInt8(Data, offset, data);
		public void WriteUInt8(int offset, byte data) => DataConverter.EncodeUInt8(Data, offset, data);
		public void WriteInt16(int offset, short data) => DataConverter.EncodeInt16(Data, offset, data);
		public void WriteUInt16(int offset, ushort data) => DataConverter.EncodeUInt16(Data, offset, data);
		public void WriteInt32(int offset, int data) => DataConverter.EncodeInt32(Data, offset, data);
		public void WriteUInt32(int offset, uint data) => DataConverter.EncodeUInt32(Data, offset, data);
		public void WriteInt64(int offset, long data) => DataConverter.EncodeInt64(Data, offset, data);
		public void WriteUInt64(int offset, ulong data) => DataConverter.EncodeUInt64(Data, offset, data);
		public void WriteFloat(int offset, float data) => DataConverter.EncodeFloat(Data, offset, data);
		public void WriteDouble(int offset, double data) => DataConverter.EncodeDouble(Data, offset, data);
		public void WriteBytes(int offset, byte[] data) => DataConverter.EncodeBytes(Data, offset, data);
		public void WriteString(int offset, string data) => DataConverter.EncodeString(Data, offset, data);

		public int ReadInt8(int offset, out sbyte data) => DataConverter.DecodeInt8(Data, offset, out data);
		public int ReadUInt8(int offset, out byte data) => DataConverter.DecodeUInt8(Data, offset, out data);
		public int ReadInt16(int offset, out short data) => DataConverter.DecodeInt16(Data, offset, out data);
		public int ReadUInt16(int offset, out ushort data) => DataConverter.DecodeUInt16(Data, offset, out data);
		public int ReadInt32(int offset, out int data) => DataConverter.DecodeInt32(Data, offset, out data);
		public int ReadUInt32(int offset, out uint data) => DataConverter.DecodeUInt32(Data, offset, out data);
		public int ReadInt64(int offset, out long data) => DataConverter.DecodeInt64(Data, offset, out data);
		public int ReadUInt64(int offset, out ulong data) => DataConverter.DecodeUInt64(Data, offset, out data);
		public int ReadFloat(int offset, out float data) => DataConverter.DecodeFloat(Data, offset, out data);
		public int ReadDouble(int offset, out double data) => DataConverter.DecodeDouble(Data, offset, out data);
		public int ReadString(int offset, out string data) => DataConverter.DecodeString(Data, offset, out data);
		public int ReadBytes(int offset, out byte[] data) => DataConverter.DecodeBytes(Data, offset, out data);
	}
}
