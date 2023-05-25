using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Common.Serialization
{
	public class ByteBufferPoolManager
	{
		public int PoolCount { get; private set; }
		public int BufferSize { get; private set; }

		public ByteBufferPoolManager(int poolCount, int bufferSize)
		{
			PoolCount = poolCount;
			BufferSize = bufferSize;
		}

	}

	public class ByteBufferPool
	{
		public int BufferCount { get; private set; }
		public int BufferSize { get; private set; }
		byte[] _buffer;

		public ByteBufferPool(int bufferCount, int bufferSize)
		{
			BufferCount = bufferCount;
			BufferSize = bufferSize;
		}

		//public ArraySegment<byte> GetBuffer() 
		//{

		//}
	}
}
