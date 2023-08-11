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
			//InheritReflectionTest.Test();

			Child a = new Child();
			Console.WriteLine(a.A);
			Console.WriteLine(a.B);

		}
	}

	public class Parent
	{
		public int A;

		public Parent()
		{
			A = 50;
		}
	}

	public class Child : Parent
	{
		public int B;

		public Child()
		{
			B = 20;
		}
	}
}