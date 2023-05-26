using System;
using System.Collections.Generic;

namespace CT.Common.Serialization
{
	public class ByteBufferPool
	{
		public const int LOH_MIN_SIZE = 85_000;
		public int BufferSize { get; private set; }
		public int BufferCapacity { get; private set; }
		public int BufferCount => 0;
		public int BarrowedCount { get; private set; }
		public bool IgnoreLOH { get; private set; }

		private Stack<ByteBuffer> _byteBufferPool;
		private List<byte[]> _memoryList;
		private int _initialMinBufferCount;

		public ByteBufferPool(int bufferSize, int minBufferCount, bool ignoreLOH = false)
		{
			_initialMinBufferCount = minBufferCount;
			IgnoreLOH = ignoreLOH;
			BufferSize = bufferSize;
			if (!IgnoreLOH && _initialMinBufferCount * BufferSize < LOH_MIN_SIZE)
			{
				_initialMinBufferCount = (int)Math.Ceiling(LOH_MIN_SIZE / (double)BufferSize);
			}
			_byteBufferPool = new(_initialMinBufferCount);
			_memoryList = new(4);
			BufferCapacity += allocate(BufferSize, _initialMinBufferCount, IgnoreLOH, 
									   _byteBufferPool, _memoryList);
		}

		/// <summary>LOH 이상 메모리를 할당합니다.</summary>
		/// <param name="minBufferCount">할당할 최소 개수 입니다.</param>
		/// <param name="bufferPool">할당될 Pool stack입니다.</param>
		/// <returns>할당한 개수 입니다.</returns>
		private static int allocate(int bufferSize, int minBufferCount, bool ignoreLOH,
									in Stack<ByteBuffer> bufferPool,
									in List<byte[]> memoryList)
		{
			if (!ignoreLOH && minBufferCount * bufferSize < LOH_MIN_SIZE)
			{
				minBufferCount = (int)Math.Ceiling(LOH_MIN_SIZE / (double)bufferSize);
			}

			var buffer = new byte[bufferSize * minBufferCount];
			memoryList.Add(buffer);
			int pivot = 0;
			for (int i = 0; i < minBufferCount; i++)
			{
				ArraySegment<byte> memSeg = new(buffer, pivot, bufferSize);
				ByteBuffer byteBuffer = new(memSeg, 0);
				bufferPool.Push(byteBuffer);
				pivot += bufferSize;
			}
			return minBufferCount;
		}

		public ByteBuffer Get()
		{
			if (_byteBufferPool.TryPop(out var byteBuffer))
			{
				BarrowedCount++;
				byteBuffer.Reset();
				return byteBuffer;
			}

			BufferCapacity += allocate(BufferSize, _initialMinBufferCount / 2, IgnoreLOH,
									   _byteBufferPool, _memoryList);
			return Get();
		}

		public void Return(ByteBuffer buffer)
		{
			if (buffer.Capacity == this.BufferSize)
			{
				BarrowedCount--;
				_byteBufferPool.Push(buffer);
			}
		}
	}
}
