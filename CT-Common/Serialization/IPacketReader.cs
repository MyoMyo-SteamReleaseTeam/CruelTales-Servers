using System;
using CT.Common.DataType;

namespace CT.Common.Serialization
{
	public interface IPacketReader
	{
		public ArraySegment<byte> ByteSegment { get; }
		public int Size { get; }
		public bool IsReadEnd { get; }
		public int ReadPosition { get; }
		public int Capacity { get; }
		public bool CanRead(int readSize);
		public bool CanRead<T>(T serializeObject) where T : IPacketSerializable;
		public void ResetReader();
		public void SetReadPosition(int position);
		public void ReadTo<T>(T serializeObject) where T : IPacketSerializable;
		public void IgnoreAll();
		public void Ignore(int count);
		public T Read<T>() where T : IPacketSerializable, new();
		public bool PeekBool();
		public byte PeekByte();
		public sbyte PeekSByte();
		public short PeekInt16();
		public ushort PeekUInt16();
		public int PeekInt32();
		public uint PeekUInt32();
		public long PeekInt64();
		public ulong PeekUInt64();
		public float PeekSingle();
		public double PeekDouble();
		public bool ReadBool();
		public byte ReadByte();
		public sbyte ReadSByte();
		public short ReadInt16();
		public ushort ReadUInt16();
		public int ReadInt32();
		public uint ReadUInt32();
		public long ReadInt64();
		public ulong ReadUInt64();
		public float ReadSingle();
		public double ReadDouble();
		public NetString ReadNetString();
		public NetStringShort ReadNetStringShort();
		public void ReadBytesCopy(ArraySegment<byte> dest, int offset);
		public byte[] ReadBytes();
	}
}
