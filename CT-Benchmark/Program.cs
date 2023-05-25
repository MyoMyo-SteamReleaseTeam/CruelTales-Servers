using BenchmarkDotNet.Running;

namespace CT.Benchmark
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//BenchmarkRunner.Run<StringSerializationBenchmark>();
			BenchmarkRunner.Run<LogBenchmark>();
		}
	}
}