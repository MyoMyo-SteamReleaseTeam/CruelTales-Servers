using BenchmarkDotNet.Running;

namespace CT.Benchmark
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//BenchmarkRunner.Run<StringSerializationBenchmark>();
			//BenchmarkRunner.Run<LogBenchmark>();
			//ActionGC.Test();
			//BufferPoolDataRaceTest.Test();
			//BenchmarkRunner.Run<StringBufferBenchmark>();
			//BenchmarkRunner.Run<BranchBenchmark>();

			//StringBufferBenchmark sb = new();
			//sb.SBToByteArrayByIter();
			InheritReflectionTest.Test();

		}
	}
}