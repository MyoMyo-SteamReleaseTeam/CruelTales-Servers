using System;
using CT.Common.DataType;

namespace CT.Common.Serialization
{
	public interface IPacketWriter
	{
		public ArraySegment<byte> ByteSegment { get; }
		public bool IsWriteEnd { get; }
		public int Size { get; }
		public int Capacity { get; }
		public void ResetWriter();

		public void SetSize(int size);
		/// <summary>Offset 만큼 건너뜁니다.</summary>
		/// <param name="offset">건너뛸 크기 입니다.</param>
		/// <returns>건너뛰기 전의 Size 입니다.</returns>
		public int OffsetSize(int offset);
		public bool CanPut(int putSize);
		public bool CanPut<T>(T serializeObject) where T : IPacketSerializable;
		public void Put<T>(T serializeObject) where T : IPacketSerializable;
		public void CopyFromReader(IPacketReader reader);
		public void PutTo(byte value, int position);
		public void Put(bool value);
		public void Put(byte value);
		public void Put(sbyte value);
		public void Put(short value);
		public void Put(ushort value);
		public void Put(int value);
		public void Put(uint value);
		public void Put(long value);
		public void Put(ulong value);
		public void Put(float value);
		public void Put(double value);
		public void Put(NetString value);
		public void Put(NetStringShort value);
		public void Put(byte[] value);
		public void Put(IPacketWriter writer);
		public void Put(ArraySegment<byte> buffer, int count);
	}
}
