using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.Common.Serialization;

namespace CT.Benchmark
{
	internal class BufferPoolDataRaceTest
	{
		private static SingleThreadByteBufferPool _pool = new(12, 10, ignoreLOH: true);
		private static ConcurrentByteBufferPool _concurrentPool = new(12, 10, ignoreLOH: true);
		private static ConcurrentByteBufferPool _concurrentPoolMulti = new(12, 10, ignoreLOH: true);

		public static void Test()
		{
			int testCount = 10000000;

			Stopwatch sw = new Stopwatch();
			sw.Start();
			for (int i = 0; i < testCount; i++)
			{
				var buffer = _pool.Get();
			}
			Console.WriteLine($"Normal pool : {sw.ElapsedMilliseconds}");

			sw.Restart();
			for (int i = 0; i < testCount; i++)
			{
				var buffer = _concurrentPool.Get();
			}
			Console.WriteLine($"Concurrent pool : {sw.ElapsedMilliseconds}");

			sw.Restart();
			Parallel.For(0, testCount, (index) =>
			{
				_concurrentPoolMulti.Get();
			});
			Console.WriteLine($"Multithread Concurrent pool : {sw.ElapsedMilliseconds}");

			//Console.WriteLine($"{nameof(_pool.BarrowedCount)}:{_pool.BarrowedCount}");
			//Console.WriteLine($"{nameof(_pool.BufferCapacity)}:{_pool.BufferCapacity}");
			//Console.WriteLine($"{nameof(_pool.BufferCount)}:{_pool.BufferCount}");
		}
	}
}
