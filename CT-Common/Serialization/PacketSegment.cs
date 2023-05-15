using System;

namespace CT.Common.Serialization
{
	public class PacketSegment
	{
		private ArraySegment<byte> _buffer;
		public ArraySegment<byte> Buffer => _buffer;

		public int Capacity => _buffer.Count;
		public int Size { get; private set; }

		public PacketWriter Writer { get; private set; }
		public PacketReader Reader { get; private set; }

		/// <summary>Count 만큼 패킷을 할당받습니다. GC가 발생합니다.</summary>
		/// <param name="count"></param>
		public PacketSegment(int count)
		{
			_buffer = new ArraySegment<byte>(new byte[count]);
			Writer = new PacketWriter(this);
			Reader = new PacketReader(this);
		}

		public PacketSegment(byte[] buffer, int offset, int count)
		{
			_buffer = new ArraySegment<byte>(buffer, offset, count);
			Writer = new PacketWriter(this);
			Reader = new PacketReader(this);
		}

		public PacketSegment(ArraySegment<byte> buffer)
		{
			_buffer = buffer;
			Writer = new PacketWriter(this);
			Reader = new PacketReader(this);
		}

		public void Reset()
		{
			Writer.Reset();
			Reader.Reset();
		}
	}
}
